﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using Quaternion2Humanoid;
using Assets.Quaternion2Humanoid.Scripts.UI;

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
            // toggleとタブ切り替えのイベント
            var toggles = Enumerable.Range(0, toggleParent.childCount)
                                    .Select(i => toggleParent.GetChild(i))
                                    .Select(t => t.GetComponent<Toggle>()).ToArray();
            var panels = Enumerable.Range(0, panelParent.childCount)
                                   .Select(i => panelParent.GetChild(i))
                                   .Select(t => t.GetComponent<Transform>()).ToArray();
            if (toggles.Length == panels.Length) {
                var len = toggles.Length;
                // toggleのon/offでパネルを切り替える
                for (var i = 0; i < len; i++) {
                    var t = toggles[i];
                    var p = panels[i];
                    t.onValueChanged.AddListener(p.gameObject.SetActive);
                }
            }
            // UIReactiveQuaternioの有効化
            AllQuats.ForEach(x => x.Validate(this));
        }

        void Start() {
            // body
            var root = rootQuat;
            var spine = new ChainedReactiveQuaternion(chestQuat);
            spine.ChainToParent(root);
            var neck = new ChainedReactiveQuaternion(neckQuat);
            neck.ChainToParent(spine);
            var world2Root = humanoidBones.GetTransfrom(HumanBodyBones.Hips).rotation;
            var world2Spine = humanoidBones.GetTransfrom(HumanBodyBones.Spine).rotation;
            humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.Hips, root.ReactiveQuaternion.ToRelativeQuaternionObservable(world2Root)).AddTo(this);
            humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.Spine, spine.ReactiveQuaternion.ToRelativeQuaternionObservable(world2Root)).AddTo(this);
            humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.Neck, neck.ReactiveQuaternion.ToRelativeQuaternionObservable(world2Root)).AddTo(this);
            // leftArm
            {
                var world2LeftArm = humanoidBones.GetTransfrom(HumanBodyBones.LeftUpperArm).rotation;
                var leftArm2Root = Quaternion.Inverse(world2LeftArm) * world2Root;
                var upperArm = new ChainedReactiveQuaternion(leftUpperArmQuat);
                upperArm.ChainToParent(spine.ReactiveQuaternion.ToRelativeQuaternionObservable(leftArm2Root).ToReactiveQuaternion());
                var lowerArm = new ChainedReactiveQuaternion(leftArmQuat);
                lowerArm.ChainToParent(upperArm);
                var hand = new ChainedReactiveQuaternion(leftHandQuat);
                hand.ChainToParent(lowerArm);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.LeftUpperArm, upperArm.ReactiveQuaternion.ToRelativeQuaternionObservable(world2LeftArm)).AddTo(this);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.LeftLowerArm, lowerArm.ReactiveQuaternion.ToRelativeQuaternionObservable(world2LeftArm)).AddTo(this);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.LeftHand, hand.ReactiveQuaternion.ToRelativeQuaternionObservable(world2LeftArm)).AddTo(this);
            }
            // rightArm
            {
                var world2RightArm = humanoidBones.GetTransfrom(HumanBodyBones.RightUpperArm).rotation;
                var rightArm2Root = Quaternion.Inverse(world2RightArm) * world2Root;
                var upperArm = new ChainedReactiveQuaternion(rightUpperArmQuat);
                upperArm.ChainToParent(spine.ReactiveQuaternion.ToRelativeQuaternionObservable(rightArm2Root).ToReactiveQuaternion());
                var lowerArm = new ChainedReactiveQuaternion(rightArmQuat);
                lowerArm.ChainToParent(upperArm);
                var hand = new ChainedReactiveQuaternion(rightHandQuat);
                hand.ChainToParent(lowerArm);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.RightUpperArm, upperArm.ReactiveQuaternion.ToRelativeQuaternionObservable(world2RightArm)).AddTo(this);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.RightLowerArm, lowerArm.ReactiveQuaternion.ToRelativeQuaternionObservable(world2RightArm)).AddTo(this);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.RightHand, hand.ReactiveQuaternion.ToRelativeQuaternionObservable(world2RightArm)).AddTo(this);
            }
            // leftLeg
            {
                var world2LeftLeg = humanoidBones.GetTransfrom(HumanBodyBones.LeftUpperLeg).rotation;
                var world2LeftFoot = humanoidBones.GetTransfrom(HumanBodyBones.LeftFoot).rotation;
                var leftLeg2Root = Quaternion.Inverse(world2LeftLeg) * world2Root;
                var foot2Leg = Quaternion.Inverse(world2LeftFoot) * world2LeftLeg;
                var upperLeg = new ChainedReactiveQuaternion(leftThighQuat);
                upperLeg.ChainToParent(root.ReactiveQuaternion.ToRelativeQuaternionObservable(leftLeg2Root).ToReactiveQuaternion());
                var lowerLeg = new ChainedReactiveQuaternion(leftShinQuat);
                lowerLeg.ChainToParent(upperLeg);
                var foot = new ChainedReactiveQuaternion(leftFootQuat);
                foot.ChainToParent(lowerLeg.ReactiveQuaternion.ToRelativeQuaternionObservable(foot2Leg).ToReactiveQuaternion());
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.LeftUpperLeg, upperLeg.ReactiveQuaternion.ToRelativeQuaternionObservable(world2LeftLeg)).AddTo(this);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.LeftLowerLeg, lowerLeg.ReactiveQuaternion.ToRelativeQuaternionObservable(world2LeftLeg)).AddTo(this);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.LeftFoot, foot.ReactiveQuaternion.ToRelativeQuaternionObservable(world2LeftFoot)).AddTo(this);
            }
            // rightLeg
            {
                var world2RightLeg = humanoidBones.GetTransfrom(HumanBodyBones.RightUpperLeg).rotation;
                var world2RightFoot = humanoidBones.GetTransfrom(HumanBodyBones.RightFoot).rotation;
                var rightLeg2Root = Quaternion.Inverse(world2RightLeg) * world2Root;
                var foot2Leg = Quaternion.Inverse(world2RightFoot) * world2RightLeg;
                var upperLeg = new ChainedReactiveQuaternion(rightThighQuat);
                upperLeg.ChainToParent(root.ReactiveQuaternion.ToRelativeQuaternionObservable(rightLeg2Root).ToReactiveQuaternion());
                var lowerLeg = new ChainedReactiveQuaternion(rightShinQuat);
                lowerLeg.ChainToParent(upperLeg);
                var foot = new ChainedReactiveQuaternion(rightFootQuat);
                foot.ChainToParent(lowerLeg.ReactiveQuaternion.ToRelativeQuaternionObservable(foot2Leg).ToReactiveQuaternion());
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.RightUpperLeg, upperLeg.ReactiveQuaternion.ToRelativeQuaternionObservable(world2RightLeg)).AddTo(this);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.RightLowerLeg, lowerLeg.ReactiveQuaternion.ToRelativeQuaternionObservable(world2RightLeg)).AddTo(this);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.RightFoot, foot.ReactiveQuaternion.ToRelativeQuaternionObservable(world2RightFoot)).AddTo(this);
            }
            // observe bone rotaion update
            humanoidBones.ReactiveBones.Subscribe(
                bone => {
                    //Debug.LogFormat("{0} : q={1}", bone.Bone, bone.Quaternion);
                }
            ).AddTo(this);
            // load scene event
            leftArmSceneButton.onClick.AddListener(() => { SceneManager.LoadScene("RightArmScene"); });
            // root reset
            rootQuat.InitQuaternion();
        }

        List<UIReactiveQuaternion> AllQuats {
            get {
                return new List<UIReactiveQuaternion> {
                    rootQuat, chestQuat, neckQuat,
                    leftUpperArmQuat, leftArmQuat, leftHandQuat,
                    rightUpperArmQuat, rightArmQuat, rightHandQuat,
                    leftThighQuat, leftShinQuat, leftFootQuat,
                    rightThighQuat, rightShinQuat, rightFootQuat
                };
            }
        }
    }
}
