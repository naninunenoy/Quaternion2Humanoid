﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

namespace Assets.Quaternion2Humanoid.Scripts {
    public interface IOverwritableReactiveQuaternion : IReactiveQuaternion {
        void OverwriteQuaternion(Quaternion quaternion);
        void SetDefaultQuaternion(Quaternion quaternion);
        void SetParentQuaternion(Quaternion quaternion);
        Quaternion LocalQuaternion { get; }
    }
    public interface IReactiveQuaternion {
        IReadOnlyReactiveProperty<Quaternion> ReactiveQuaternion { get; }
    }

    public static class IObsevableQuaternionExtension {
        class InnerQuaternionSource : IReactiveQuaternion {
            IReadOnlyReactiveProperty<Quaternion> reactiveQuaternion;
            public IReadOnlyReactiveProperty<Quaternion> ReactiveQuaternion { get { return reactiveQuaternion; } }
            public InnerQuaternionSource(IObservable<Quaternion> source) {
                reactiveQuaternion = source.ToReactiveProperty();
            }
        }
        public static IReactiveQuaternion ToReactiveQuaternion(this IObservable<Quaternion> source) {
            return new InnerQuaternionSource(source);
        }
    }

    public class ChainedReactiveQuaternion : IReactiveQuaternion {
        readonly IOverwritableReactiveQuaternion mine;

        public ChainedReactiveQuaternion(IOverwritableReactiveQuaternion mine) { this.mine = mine; }

        public void ChainToParent(IReactiveQuaternion parent) {
            parent.ReactiveQuaternion
                  .Select(q => { return q; })
                  .Select(q0 => { return new { q0, q1 = q0 * mine.LocalQuaternion }; })
                  .Subscribe(q => {
                      mine.SetParentQuaternion(q.q0);
                      mine.OverwriteQuaternion(q.q1);
                  });
        }

        public IReadOnlyReactiveProperty<Quaternion> ReactiveQuaternion { get { return mine.ReactiveQuaternion; } }
    }
}