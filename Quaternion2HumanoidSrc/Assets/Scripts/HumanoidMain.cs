using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Assets.Quaternion2Humanoid.Scripts {
    public class HumanoidMain : MonoBehaviour {
        [Header("* leftUpperArm")]
        [SerializeField] Transform leftUpperArmTransform;
        [SerializeField] UIReactiveQuaternion leftUpperArmReactiveQuaternion;
        [Header("* leftForeArm")]
        [SerializeField] Transform leftForeArmTransform;
        [SerializeField] UIReactiveQuaternion leftForeArmReactiveQuaternion;

        void Awake() {
            // leftUpperArm
            leftUpperArmReactiveQuaternion.SetDefaultQuaternion(leftUpperArmTransform.rotation);
            leftUpperArmReactiveQuaternion.ReactiveQuaternion.Subscribe(
                q => { 
                    leftUpperArmTransform.rotation = q;
                }
            );
            // leftForeArm
            leftForeArmReactiveQuaternion.SetDefaultQuaternion(leftForeArmTransform.rotation);
            leftForeArmReactiveQuaternion.ReactiveQuaternion.Subscribe(
                q => {
                    leftForeArmTransform.rotation = q; 
                }
            );
        }
    }
}
