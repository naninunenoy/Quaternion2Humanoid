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
                  .WithLatestFrom(mine.ReactiveQuaternion, (q0, q1) => { return new { parent = q0, mine = q0 * q1 }; })
                  .Subscribe(q => {
                      mine.SetDefaultQuaternion(q.parent);
                      mine.OverwriteQuaternion(q.mine);
                  });
        }

        public void ChainToParents(params IReactiveQuaternion[] parents) {
            parents.Select(x => x.ReactiveQuaternion.AsObservable())
                   .CombineLatest()
                   .WithLatestFrom(mine.ReactiveQuaternion,
                                   (parentQuats, q) => {
                                       var parentQuat = parentQuats.Aggregate((q0, q1) => { return q0 * q1; });
                                       return new { parent = parentQuat, mine = parentQuat * q };
                                   })
                   .Subscribe(q => {
                       mine.SetDefaultQuaternion(q.parent);
                       mine.OverwriteQuaternion(q.mine);
                   });
        }

        public IReadOnlyReactiveProperty<Quaternion> ReactiveQuaternion { get { return mine.ReactiveQuaternion; } }
    }
}