﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using Quaternion2Humanoid;
using Assets.Quaternion2Humanoid.Scripts.UI;

namespace Assets.Quaternion2Humanoid.Scripts {
    public class RightHandMain : MonoBehaviour {
        [SerializeField] ReactiveHumanoidBones humanoidBones;
        [SerializeField] UIReactiveQuaternion rightUpperArmReactiveQuaternion;
        [SerializeField] UIReactiveQuaternion rightLowerArmReactiveQuaternion;
        [SerializeField] Button sampleSceneButton;
        [SerializeField] Button humanoidSceneButton;

        void Awake() {
            rightUpperArmReactiveQuaternion.Validate(this);
            rightLowerArmReactiveQuaternion.Validate(this);
        }

        void Start() {
            var world2RightArm = humanoidBones.GetTransfrom(HumanBodyBones.RightUpperArm).rotation;
            // rotation of lowerArm depends on rotation of upperArm
            var upperArmQuaternion = rightUpperArmReactiveQuaternion;
            var lowerArmQuaternion = new ChainedReactiveQuaternion(rightLowerArmReactiveQuaternion);
            lowerArmQuaternion.ChainToParent(upperArmQuaternion);
            humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.RightUpperArm, upperArmQuaternion.ReactiveQuaternion.ToRelativeQuaternionObservable(world2RightArm)).AddTo(this);
            humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.RightLowerArm, lowerArmQuaternion.ReactiveQuaternion.ToRelativeQuaternionObservable(world2RightArm)).AddTo(this);
            // observe update bone rotaion
            humanoidBones.ReactiveBones.Subscribe(
                bone => {
                    //Debug.LogFormat("{0} : q={1}", bone.Bone, bone.Quaternion);
                }
            ).AddTo(this);
            // load scene event
            sampleSceneButton.onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); });
            humanoidSceneButton.onClick.AddListener(() => { SceneManager.LoadScene("HumanoidBodyScene"); });
        }
    }
}
