using System.Collections;
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
        }

        void Start() {
            // body
            var root = rootQuat;
            var spine = new ChainedReactiveQuaternion(chestQuat);
            spine.ChainToParent(root);
            var neck = new ChainedReactiveQuaternion(neckQuat);
            neck.ChainToParents(root, spine);
            var world2Boby = humanoidBones.GetTransfrom(HumanBodyBones.Hips).rotation;
            humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.Hips, root.ReactiveQuaternion.ToRelativedQuaternionObservable(world2Boby)).AddTo(this);
            humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.Spine, spine.ReactiveQuaternion.ToRelativedQuaternionObservable(world2Boby)).AddTo(this);
            humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.Neck, neck.ReactiveQuaternion.ToRelativedQuaternionObservable(world2Boby)).AddTo(this);
            // leftArm
            {
                var upperArm = new ChainedReactiveQuaternion(leftUpperArmQuat);
                upperArm.ChainToParents(root, spine);
                var lowerArm = new ChainedReactiveQuaternion(leftArmQuat);
                lowerArm.ChainToParents(root, spine, upperArm);
                var hand = new ChainedReactiveQuaternion(leftHandQuat);
                hand.ChainToParents(root, spine, upperArm, lowerArm);
                var world2LeftArm = humanoidBones.GetTransfrom(HumanBodyBones.LeftUpperArm).rotation;
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.LeftUpperArm, upperArm.ReactiveQuaternion.ToRelativedQuaternionObservable(world2LeftArm)).AddTo(this);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.LeftLowerArm, lowerArm.ReactiveQuaternion.ToRelativedQuaternionObservable(world2LeftArm)).AddTo(this);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.LeftHand, hand.ReactiveQuaternion.ToRelativedQuaternionObservable(world2LeftArm)).AddTo(this);
            }
            // rightArm
            {
                var upperArm = new ChainedReactiveQuaternion(rightUpperArmQuat);
                upperArm.ChainToParents(root, spine);
                var lowerArm = new ChainedReactiveQuaternion(rightArmQuat);
                lowerArm.ChainToParents(root, spine, upperArm);
                var hand = new ChainedReactiveQuaternion(rightHandQuat);
                hand.ChainToParents(root, spine, upperArm, lowerArm);
                var world2RightArm = humanoidBones.GetTransfrom(HumanBodyBones.RightUpperArm).rotation;
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.RightUpperArm, upperArm.ReactiveQuaternion.ToRelativedQuaternionObservable(world2RightArm)).AddTo(this);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.RightLowerArm, lowerArm.ReactiveQuaternion.ToRelativedQuaternionObservable(world2RightArm)).AddTo(this);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.RightHand, hand.ReactiveQuaternion.ToRelativedQuaternionObservable(world2RightArm)).AddTo(this);
            }
            // leftLeg
            {
                var upperLeg = new ChainedReactiveQuaternion(leftThighQuat);
                upperLeg.ChainToParents(root, spine);
                var lowerLeg = new ChainedReactiveQuaternion(leftShinQuat);
                lowerLeg.ChainToParents(root, spine, upperLeg);
                var foot = new ChainedReactiveQuaternion(leftFootQuat);
                foot.ChainToParents(root, spine, upperLeg, lowerLeg);
                var world2LeftLeg = humanoidBones.GetTransfrom(HumanBodyBones.LeftUpperLeg).rotation;
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.LeftUpperLeg, upperLeg.ReactiveQuaternion.ToRelativedQuaternionObservable(world2LeftLeg)).AddTo(this);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.LeftLowerLeg, lowerLeg.ReactiveQuaternion.ToRelativedQuaternionObservable(world2LeftLeg)).AddTo(this);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.LeftFoot, foot.ReactiveQuaternion.ToRelativedQuaternionObservable(world2LeftLeg)).AddTo(this);
            }
            // rightLeg
            {
                var upperLeg = new ChainedReactiveQuaternion(rightThighQuat);
                upperLeg.ChainToParents(root, spine);
                var lowerLeg = new ChainedReactiveQuaternion(rightShinQuat);
                lowerLeg.ChainToParents(root, spine, upperLeg);
                var foot = new ChainedReactiveQuaternion(rightFootQuat);
                foot.ChainToParents(root, spine, upperLeg, lowerLeg);
                var world2RightLeg = humanoidBones.GetTransfrom(HumanBodyBones.RightUpperLeg).rotation;
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.RightUpperLeg, upperLeg.ReactiveQuaternion.ToRelativedQuaternionObservable(world2RightLeg)).AddTo(this);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.RightLowerLeg, lowerLeg.ReactiveQuaternion.ToRelativedQuaternionObservable(world2RightLeg)).AddTo(this);
                humanoidBones.SubscribeAsGlobalQuaternionTo(HumanBodyBones.RightFoot, foot.ReactiveQuaternion.ToRelativedQuaternionObservable(world2RightLeg)).AddTo(this);
            }
            // observe bone rotaion update
            humanoidBones.ReactiveBones.Subscribe(
                bone => {
                    //Debug.LogFormat("{0} : q={1}", bone.Bone, bone.Quaternion);
                }
            ).AddTo(this);
            // load scene event
            leftArmSceneButton.onClick.AddListener(() => { SceneManager.LoadScene("RightArmScene"); });
            // validate auto slider update
            rootQuat.ValidateAutoUpdate(this);
            chestQuat.ValidateAutoUpdate(this);
            neckQuat.ValidateAutoUpdate(this);
            leftUpperArmQuat.ValidateAutoUpdate(this);
            leftArmQuat.ValidateAutoUpdate(this);
            leftHandQuat.ValidateAutoUpdate(this);
            rightUpperArmQuat.ValidateAutoUpdate(this);
            rightArmQuat.ValidateAutoUpdate(this);
            rightHandQuat.ValidateAutoUpdate(this);
            leftThighQuat.ValidateAutoUpdate(this);
            leftShinQuat.ValidateAutoUpdate(this);
            leftFootQuat.ValidateAutoUpdate(this);
            rightThighQuat.ValidateAutoUpdate(this);
            rightShinQuat.ValidateAutoUpdate(this);
            rightFootQuat.ValidateAutoUpdate(this);
        }
    }
}
