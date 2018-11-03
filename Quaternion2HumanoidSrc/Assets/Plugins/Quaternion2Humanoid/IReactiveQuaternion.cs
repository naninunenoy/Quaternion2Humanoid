using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

namespace Quaternion2Humanoid {
    public interface IOverwritableReactiveQuaternion : IReactiveQuaternion {
        void OverwriteQuaternion(Quaternion quaternion);
        void SetDefaultQuaternion(Quaternion quaternion);
    }
    public interface IReactiveQuaternion {
        IReadOnlyReactiveProperty<Quaternion> ReactiveQuaternion { get; }
    }

    public class ChainedReactiveQuaternion : IOverwritableReactiveQuaternion {
        readonly IOverwritableReactiveQuaternion mine;

        public ChainedReactiveQuaternion(IOverwritableReactiveQuaternion mine) { this.mine = mine; }

        public void ChainToParent(IReactiveQuaternion parent) {
            Observable.CombineLatest(parent.ReactiveQuaternion, mine.ReactiveQuaternion)
                      .Subscribe(quats => {
                          mine.SetDefaultQuaternion(quats[0]);
                          mine.OverwriteQuaternion(quats[0] * quats[1]);
                      });
        }

        public void ChainToParents(params IReactiveQuaternion[] parents) {
            var parentQuats = parents.Select(x => x.ReactiveQuaternion)
                                     .Concat(new IReadOnlyReactiveProperty<Quaternion>[] { mine.ReactiveQuaternion });
            parentQuats.Select(x => x.AsObservable())
                       .CombineLatest()
                       .Subscribe(quats => {
                           var q = quats.Aggregate((q0, q1) => { return q0 * q1; });
                           mine.OverwriteQuaternion(q);
                       });
        }

        public void OverwriteQuaternion(Quaternion quaternion) { mine.OverwriteQuaternion(quaternion); }
        public void SetDefaultQuaternion(Quaternion quaternion) { mine.SetDefaultQuaternion(quaternion); }
        public IReadOnlyReactiveProperty<Quaternion> ReactiveQuaternion { get { return mine.ReactiveQuaternion; } }
    }
}