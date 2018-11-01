using System.Collections;using System.Collections.Generic;using UnityEngine;using UniRx;using Quaternion2Humanoid;namespace Assets.Quaternion2Humanoid.Scripts {    public class HumanoidMain : MonoBehaviour {        [SerializeField] ReactiveHumanoidBones humanoidBones;        [SerializeField] UIReactiveQuaternion leftUpperArmReactiveQuaternion;        [SerializeField] UIReactiveQuaternion leftLowerArmReactiveQuaternion;        void Awake() {
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.LeftUpperArm, leftUpperArmReactiveQuaternion.ReactiveQuaternion).AddTo(this);
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.LeftLowerArm, leftLowerArmReactiveQuaternion.ReactiveQuaternion).AddTo(this);
            // observer
            humanoidBones.ReactiveBones.Subscribe(                bone => {
                    Debug.LogFormat("{0} : q={1}", bone.Bone, bone.Quaternion);
                }            );        }    }}