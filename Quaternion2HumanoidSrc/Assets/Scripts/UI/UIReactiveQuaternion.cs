using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Assets.Quaternion2Humanoid.Scripts.UI {
    public class UIReactiveQuaternion : QuaternionSliders, IOverwritableReactiveQuaternion {
        [SerializeField] Button resetButton;
        [SerializeField] Text rollText;
        [SerializeField] Text pitchText;
        [SerializeField] Text yawText;

        [SerializeField] UIReactiveQuaternion[] children;

        Quaternion defaultQuaternion = Quaternion.identity;
        public void SetDefaultQuaternion(Quaternion quaternion) { defaultQuaternion = quaternion; }
        Quaternion parentQuaternion = Quaternion.identity;
        public void SetParentQuaternion(Quaternion quaternion) { parentQuaternion = quaternion; }

        public void InitQuaternion() { 
            SetQuaternion(defaultQuaternion); 
            // 子のInitQuaternionを強制的に呼び出すための力技
            if (children != null && children.Length > 0) {
                foreach(var ch in children) {
                    ch.InitQuaternion();
                }
            }
        }

        public void OverwriteQuaternion(Quaternion quaternion) { SetQuaternion(quaternion); }

        public Quaternion LocalQuaternion {
            get { return Quaternion.Inverse(parentQuaternion) * ReactiveQuaternion.Value; }
        }

        public override void Validate(Component comp) {
            base.Validate(comp);
            // リセットボタン
            resetButton.onClick.AddListener(InitQuaternion);
            SliderQuaternion.Subscribe(q => {
                var euler = q.eulerAngles;
                rollText.text = string.Format("{0:F1}°", euler.x);
                pitchText.text = string.Format("{0:F1}°", euler.y);
                yawText.text = string.Format("{0:F1}°", euler.z);
            }).AddTo(comp);
        }
    }
}
