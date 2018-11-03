﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using Quaternion2Humanoid;
using Assets.Quaternion2Humanoid.Scripts.UI;

namespace Assets.Quaternion2Humanoid.Scripts {
    public class LeftHandMain : MonoBehaviour {
        [SerializeField] ReactiveHumanoidBones humanoidBones;
        [SerializeField] UIReactiveQuaternion leftUpperArmReactiveQuaternion;
        [SerializeField] UIReactiveQuaternion leftLowerArmReactiveQuaternion;
        [SerializeField] Button sampleSceneButton;
        [SerializeField] Button humanoidSceneButton;

        void Start() {
            // rotation of lowerArm depends on rotation of upperArm
            var upperArmQuaternion = leftUpperArmReactiveQuaternion;
            var lowerArmQuaternion = new ChainedReactiveQuaternion(leftLowerArmReactiveQuaternion);
            lowerArmQuaternion.ChainToParent(upperArmQuaternion);
            humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.LeftUpperArm, upperArmQuaternion.ReactiveQuaternion).AddTo(this);
            humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.LeftLowerArm, lowerArmQuaternion.ReactiveQuaternion).AddTo(this);
            // observe update bone rotaion
            humanoidBones.ReactiveBones.Subscribe(
                bone => {
                    Debug.LogFormat("{0} : q={1}", bone.Bone, bone.Quaternion);
                }
            ).AddTo(this);
            // load scene event
            sampleSceneButton.onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); });
            humanoidSceneButton.onClick.AddListener(() => { SceneManager.LoadScene("HumanoidBodyScene"); });
            // validate auto slider update
            leftUpperArmReactiveQuaternion.ValidateAutoUpdate(this);
            leftLowerArmReactiveQuaternion.ValidateAutoUpdate(this);
        }
    }
}
