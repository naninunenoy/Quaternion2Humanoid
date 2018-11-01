﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Assets.Quaternion2Humanoid.Scripts {
    public class HumanoidMain : MonoBehaviour {
        [SerializeField] Animator humanoidAnimator;
        [SerializeField] UIReactiveQuaternion leftUpperArmReactiveQuaternion;
        [SerializeField] UIReactiveQuaternion leftLowerArmReactiveQuaternion;

        void Awake() {
            // leftUpperArm
            var leftUpperArmTransform = humanoidAnimator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            leftUpperArmReactiveQuaternion.SetDefaultQuaternion(leftUpperArmTransform.rotation);
            leftUpperArmReactiveQuaternion.ReactiveQuaternion.Subscribe(
                q => { 
                    leftUpperArmTransform.rotation = q;
                }
            );
            // leftLowerArm
            var leftLowerArmTransform = humanoidAnimator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            leftLowerArmReactiveQuaternion.SetDefaultQuaternion(leftLowerArmTransform.rotation);
            leftLowerArmReactiveQuaternion.ReactiveQuaternion.Subscribe(
                q => {
                    leftLowerArmTransform.rotation = q;
                }
            );
        }
    }
}
