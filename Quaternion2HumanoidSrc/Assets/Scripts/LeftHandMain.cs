﻿using System.Collections;using System.Collections.Generic;using UnityEngine;using UnityEngine.UI;using UnityEngine.SceneManagement;using UniRx;using Quaternion2Humanoid;namespace Assets.Quaternion2Humanoid.Scripts {    public class LeftHandMain : MonoBehaviour {        [SerializeField] ReactiveHumanoidBones humanoidBones;        [SerializeField] UIReactiveQuaternion leftUpperArmReactiveQuaternion;        [SerializeField] UIReactiveQuaternion leftLowerArmReactiveQuaternion;        [SerializeField] Button sampleSceneButton;        [SerializeField] Button humanoidSceneButton;        void Start() {
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.LeftUpperArm, leftUpperArmReactiveQuaternion.ReactiveQuaternion).AddTo(this);
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.LeftLowerArm, leftLowerArmReactiveQuaternion.ReactiveQuaternion).AddTo(this);
            // observer
            humanoidBones.ReactiveBones.Subscribe(                bone => {
                    Debug.LogFormat("{0} : q={1}", bone.Bone, bone.Quaternion);
                }            ).AddTo(this);            // load scene event
            sampleSceneButton.onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); });            humanoidSceneButton.onClick.AddListener(() => { SceneManager.LoadScene("HumanoidBodyScene"); });            // validate auto slider update            leftUpperArmReactiveQuaternion.ValidateAutoUpdate(this);            leftLowerArmReactiveQuaternion.ValidateAutoUpdate(this);        }    }}