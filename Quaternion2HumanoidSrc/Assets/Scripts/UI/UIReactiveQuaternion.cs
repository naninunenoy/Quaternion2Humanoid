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

        Quaternion defaultQuaternion = Quaternion.identity;
        public void SetDefaultQuaternion(Quaternion quaternion) { defaultQuaternion = quaternion; }

        public void InitQuaternion() { SetQuaternion(defaultQuaternion); }

        public void OverwriteQuaternion(Quaternion quaternion) { SetQuaternion(quaternion); }

        public Quaternion LocalQuaternion {
            get { return Quaternion.Inverse(defaultQuaternion) * ReactiveQuaternion.Value; }
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
