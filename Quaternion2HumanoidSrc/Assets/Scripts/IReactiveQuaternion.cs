using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

namespace Assets.Quaternion2Humanoid.Scripts {
    public interface IOverwritableReactiveQuaternion : IReactiveQuaternion {
        void OverwriteQuaternion(Quaternion quaternion);
        void SetDefaultQuaternion(Quaternion quaternion);
        Quaternion LocalQuaternion { get; }
    }
    public interface IReactiveQuaternion {
        IReadOnlyReactiveProperty<Quaternion> ReactiveQuaternion { get; }
    }

    public class ChainedReactiveQuaternion : IReactiveQuaternion {
        readonly IOverwritableReactiveQuaternion mine;

        public ChainedReactiveQuaternion(IOverwritableReactiveQuaternion mine) { this.mine = mine; }

        public void ChainToParent(IReactiveQuaternion parent, Quaternion parent2child = default(Quaternion)) {
            var convert = parent2child.Equals(default(Quaternion)) ? Quaternion.identity : parent2child;
            parent.ReactiveQuaternion
                  .Select(q => { return q * convert; })
                  .Select(q0 => { return new { q0, q1 = q0 * mine.LocalQuaternion }; })
                  .Subscribe(q => {
                      mine.SetDefaultQuaternion(q.q0);
                      mine.OverwriteQuaternion(q.q1);
                  });
        }

        public IReadOnlyReactiveProperty<Quaternion> ReactiveQuaternion { get { return mine.ReactiveQuaternion; } }
    }
}