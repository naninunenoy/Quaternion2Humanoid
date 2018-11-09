using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

namespace Assets.Quaternion2Humanoid.Scripts {
    public interface IOverwritableReactiveQuaternion : IReactiveQuaternion {
        void Validate(Component comp);
        void OverwriteQuaternion(Quaternion quaternion);
        void SetDefaultQuaternion(Quaternion quaternion);
    }
    public interface IReactiveQuaternion {
        IReadOnlyReactiveProperty<Quaternion> ReactiveQuaternion { get; }
    }

    public class ChainedReactiveQuaternion : IReactiveQuaternion {
        readonly IOverwritableReactiveQuaternion mine;

        public ChainedReactiveQuaternion(IOverwritableReactiveQuaternion mine) { this.mine = mine; }

        public void ChainToParent(IReactiveQuaternion parent) {
            parent.ReactiveQuaternion
                  .Subscribe(q => {
                      mine.SetDefaultQuaternion(q);
                  });
        }

        public void ChainToParents(params IReactiveQuaternion[] parents) {
            var parentQuats = parents.Select(x => x.ReactiveQuaternion);
            parentQuats.Select(x => x.AsObservable())
                       .CombineLatest()
                       .Subscribe(quats => {
                           var parentQuat = quats.Aggregate((q0, q1) => { return q0 * q1; });
                           mine.SetDefaultQuaternion(parentQuat);
                       });
        }

        public IReadOnlyReactiveProperty<Quaternion> ReactiveQuaternion { get { return mine.ReactiveQuaternion; } }
    }
}