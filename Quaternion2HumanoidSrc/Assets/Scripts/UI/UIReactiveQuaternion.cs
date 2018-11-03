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
        public void SetDefaultQuaternion(Quaternion quaternion) {
            sliderW.Slider.value = quaternion.w;
            sliderX.Slider.value = quaternion.x;
            sliderY.Slider.value = quaternion.y;
            sliderZ.Slider.value = quaternion.z;
            defaultQuaternion = quaternion;
        }

        public void InitQuaternion() { SetQuaternionToSliders(defaultQuaternion); }

        protected override void Awake() {
            base.Awake();
            // リセットボタン
            resetButton.onClick.AddListener(InitQuaternion);
            ReactiveQuaternion.Subscribe(q => {
                var euler = q.eulerAngles;
                rollText.text = string.Format("{0:F1}", euler.x);
                pitchText.text = string.Format("{0:F1}", euler.y);
                yawText.text = string.Format("{0:F1}", euler.z);
            });
        }
    }
}
