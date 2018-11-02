using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Quaternion2Humanoid {
    [RequireComponent(typeof(Animator))]
    public class ReactiveHumanoidBones : MonoBehaviour {
        Animator humanoidAnimator;
        readonly Dictionary<HumanBodyBones, Transform> bonesTransformDict = new Dictionary<HumanBodyBones, Transform>();

        ReactiveProperty<BoneQaternion> reactiveBones = new ReactiveProperty<BoneQaternion>();
        public IReadOnlyReactiveProperty<BoneQaternion> ReactiveBones { get { return reactiveBones; } }

        void Awake() {
            humanoidAnimator = GetComponent<Animator>();
            if (humanoidAnimator == null || !humanoidAnimator.isHuman) {
                Debug.LogError("Humanoid Not Found.");
                return;
            }
            for (int i = 0; i < (int)HumanBodyBones.LastBone; i++) {
                var bone = (HumanBodyBones)i;
                bonesTransformDict.Add(bone, humanoidAnimator.GetBoneTransform(bone));
            }
        }

        public IDisposable SubscribeAsGlobalQuaternionTo(HumanBodyBones to, IObservable<Quaternion> source) {
            return source.Subscribe(q => {
                var boneTrans = bonesTransformDict[to];
                if (boneTrans != null) {
                    boneTrans.localRotation = q;
                    reactiveBones.Value = new BoneQaternion(to, q);
                }
            });
        }

        public IDisposable SubscribeAsLocalQuaternionTo(HumanBodyBones to, IObservable<Quaternion> source) {
            return source.Subscribe(q => {
                var boneTrans = bonesTransformDict[to];
                if (boneTrans != null) {
                    boneTrans.rotation = q;
                    reactiveBones.Value = new BoneQaternion(to, q);
                }
            });
        }
    }
}