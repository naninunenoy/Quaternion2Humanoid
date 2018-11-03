using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Assets.Quaternion2Humanoid.Scripts.UI {
    public class UIReactiveQuaternion : QuaternionSliders {
        [SerializeField] Button resetButton;
        [SerializeField] Text rollText;
        [SerializeField] Text pitchText;
        [SerializeField] Text yawText;

        Quaternion defaultQuaternion = Quaternion.identity;
        public override void SetDefaultQuaternion(Quaternion quaternion) {
            defaultQuaternion = quaternion;
        }

        public void InitQuaternion() { SetQuaternionToSliders(defaultQuaternion); }

        public override void Validate() {
            base.Validate();
            // リセットボタン
            resetButton.onClick.AddListener(InitQuaternion);
            SliderQuaternion.Subscribe(q => {
                var euler = q.eulerAngles;
                rollText.text = string.Format("{0:F1}°", euler.x);
                pitchText.text = string.Format("{0:F1}°", euler.y);
                yawText.text = string.Format("{0:F1}°", euler.z);
            }).AddTo(this);
        }
    }
}
