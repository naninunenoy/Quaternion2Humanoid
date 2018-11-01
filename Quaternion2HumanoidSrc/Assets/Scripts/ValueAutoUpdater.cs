using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

namespace Assets.Quaternion2Humanoid.Scripts {
    public class ValueAutoUpdater : MonoBehaviour {
        enum State { Add = 0, Dec };
        State state;

        [SerializeField] ValueSlider slider;
        [SerializeField] Toggle toggle;
        [SerializeField] float speed = 0.05F;

        float max;
        float min;
        float val;

        void Awake() {
            max = slider.Slider.maxValue;
            min = slider.Slider.minValue;
            val = slider.Slider.value;
            state = (val < max) ? State.Add : State.Dec;
            // auto中は手動変更を不可に
            toggle.onValueChanged.AddListener(isOn => { slider.Slider.interactable = !isOn; });
            // auto
            this.UpdateAsObservable()
                .Where(_ => toggle != null && toggle.isOn)
                .Subscribe(_ => {
                    if (state == State.Add) {
                        val = Mathf.Min(val + speed, max);
                        state = (val >= max) ? State.Dec : state; // 減らすに切り替え
                    } else {
                        val = Mathf.Max(val - speed, min);
                        state = (val <= min) ? State.Add : state; // 増やすに切り替え
                    }
                    slider.Slider.value = val;
                });
        }
    }
}
