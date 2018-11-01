using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Assets.Quaternion2Humanoid.Scripts {
    public class HumanoidMain : MonoBehaviour {
        [SerializeField] Animator humanoidAnimator;
        [SerializeField] UIReactiveQuaternion leftUpperArmReactiveQuaternion;
        [SerializeField] UIReactiveQuaternion leftForeArmReactiveQuaternion;

        void Awake() {
            // leftUpperArm
            var leftUpperArmTransform = humanoidAnimator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            leftUpperArmReactiveQuaternion.SetDefaultQuaternion(leftUpperArmTransform.rotation);
            leftUpperArmReactiveQuaternion.ReactiveQuaternion.Subscribe(
                q => { 
                    leftUpperArmTransform.rotation = q;
                }
            );
            // leftForeArm
            var leftForeArmTransform = humanoidAnimator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            leftForeArmReactiveQuaternion.SetDefaultQuaternion(leftForeArmTransform.rotation);
            leftForeArmReactiveQuaternion.ReactiveQuaternion.Subscribe(
                q => {
                    leftForeArmTransform.rotation = q; 
                }
            );
        }
    }
}
