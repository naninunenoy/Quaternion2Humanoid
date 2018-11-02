using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Quaternion2Humanoid {
    public interface IOverwritableReactiveQuaternion : IReactiveQuaternion {
        void OverwriteQuaternion(Quaternion quaternion);
    }
    public interface IReactiveQuaternion {
        IReadOnlyReactiveProperty<Quaternion> ReactiveQuaternion { get; }
    }

    public class ChainedReactiveQuaternion : IReactiveQuaternion {
        readonly IOverwritableReactiveQuaternion mine;

        public ChainedReactiveQuaternion(IOverwritableReactiveQuaternion mine) { this.mine = mine; }

        public void ChainToParent(IReactiveQuaternion parent) {
            parent.ReactiveQuaternion.Subscribe(
                parentQuat => {
                    var q = mine.ReactiveQuaternion.Value;
                    mine.OverwriteQuaternion(parentQuat * q);
                }
            );
        }

        public IReadOnlyReactiveProperty<Quaternion> ReactiveQuaternion { get { return mine.ReactiveQuaternion; } }
    }
}