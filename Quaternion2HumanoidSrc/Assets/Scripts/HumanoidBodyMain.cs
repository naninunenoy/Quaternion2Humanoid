﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using Quaternion2Humanoid;

namespace Assets.Quaternion2Humanoid.Scripts {
    public class HumanoidBodyMain : MonoBehaviour {
        [SerializeField] Button leftArmSceneButton;
        [SerializeField] Transform toggleParent;
        [SerializeField] Transform panelParent;
        [Header("* Humanoid Quaternion")]
        [SerializeField] ReactiveHumanoidBones humanoidBones;
        [SerializeField] UIReactiveQuaternion rootQuat;
        [SerializeField] UIReactiveQuaternion chestQuat;
        [SerializeField] UIReactiveQuaternion neckQuat;
        [SerializeField] UIReactiveQuaternion leftUpperArmQuat;
        [SerializeField] UIReactiveQuaternion leftArmQuat;
        [SerializeField] UIReactiveQuaternion leftHandQuat;
        [SerializeField] UIReactiveQuaternion rightUpperArmQuat;
        [SerializeField] UIReactiveQuaternion rightArmQuat;
        [SerializeField] UIReactiveQuaternion rightHandQuat;
        [SerializeField] UIReactiveQuaternion leftThighQuat;
        [SerializeField] UIReactiveQuaternion leftShinQuat;
        [SerializeField] UIReactiveQuaternion leftFootQuat;
        [SerializeField] UIReactiveQuaternion rightThighQuat;
        [SerializeField] UIReactiveQuaternion rightShinQuat;
        [SerializeField] UIReactiveQuaternion rightFootQuat;

        void Awake() {
            var toggles = Enumerable.Range(0, toggleParent.childCount)
                                    .Select(i => toggleParent.GetChild(i))
                                    .Select(t => t.GetComponent<Toggle>()).ToArray();
            var panels = Enumerable.Range(0, panelParent.childCount)
                                   .Select(i => panelParent.GetChild(i))
                                   .Select(t => t.GetComponent<Transform>()).ToArray();;
            if (toggles.Length == panels.Length) {
                var len = toggles.Length;
                // toggleのon/offでパネルを切り替える
                for (var i = 0; i < len; i++) {
                    var t = toggles[i];
                    var p = panels[i];
                    t.onValueChanged.AddListener(p.gameObject.SetActive);
                }
            }
        }

        void Start() {
            // set Quaternion stream
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.Hips, rootQuat.ReactiveQuaternion).AddTo(this);
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.Spine, chestQuat.ReactiveQuaternion).AddTo(this);
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.Neck, neckQuat.ReactiveQuaternion).AddTo(this);
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.LeftUpperArm, leftUpperArmQuat.ReactiveQuaternion).AddTo(this);
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.LeftLowerArm, leftArmQuat.ReactiveQuaternion).AddTo(this);
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.LeftHand, leftHandQuat.ReactiveQuaternion).AddTo(this);
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.RightUpperArm, rightUpperArmQuat.ReactiveQuaternion).AddTo(this);
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.RightLowerArm, rightArmQuat.ReactiveQuaternion).AddTo(this);
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.RightHand, rightHandQuat.ReactiveQuaternion).AddTo(this);
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.LeftUpperLeg, leftThighQuat.ReactiveQuaternion).AddTo(this);
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.LeftLowerLeg, leftShinQuat.ReactiveQuaternion).AddTo(this);
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.LeftFoot, leftFootQuat.ReactiveQuaternion).AddTo(this);
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.RightUpperLeg, rightThighQuat.ReactiveQuaternion).AddTo(this);
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.RightLowerLeg, rightShinQuat.ReactiveQuaternion).AddTo(this);
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.RightFoot, rightFootQuat.ReactiveQuaternion).AddTo(this);
            // observer
            humanoidBones.ReactiveBones.Subscribe(
                bone => {
                    Debug.LogFormat("{0} : q={1}", bone.Bone, bone.Quaternion);
                }
            ).AddTo(this);
            // load scene event
            leftArmSceneButton.onClick.AddListener(() => { SceneManager.LoadScene("LeftArmScene"); });
        }
    }
}
