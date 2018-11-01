using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Assets.Quaternion2Humanoid.Scripts {
    public class ValueSlider : MonoBehaviour {
        [SerializeField] Slider slider;
        [SerializeField] Text text;

        ReactiveProperty<float> reactiveValue = new ReactiveProperty<float>();
        public IReadOnlyReactiveProperty<float> ReactiveValue { get { return reactiveValue; } }

        public void SetValue(float value) { slider.value = value; }

        void Awake() {
            // スライダーのイベント
            slider.OnValueChangedAsObservable()
                  .Subscribe(val => {
                      // スライダーの値と表示を一致させる
                      text.text = string.Format("{0:F2}", val);
                      // 値を設定
                      reactiveValue.Value = val;
                  })
                  .AddTo(this);
        }
    }
}