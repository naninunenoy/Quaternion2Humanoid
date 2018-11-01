using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Assets.Quaternion2Humanoid.Scripts {
    public class ValueSlider : MonoBehaviour {
        [SerializeField] Slider slider;
        [SerializeField] Text text;

        public ReactiveProperty<float> reactiveValue = new ReactiveProperty<float>();
        public IReactiveProperty<float> ReactiveValue { get { return reactiveValue; } }

        void Awake() {
            // スライダーのイベント
            slider.OnValueChangedAsObservable()
                  .Subscribe(val => {
                      // スライダーの値と表示を一致させる
                      text.text = string.Format("{0:F1}", val);
                      // 値を設定
                      reactiveValue.Value = val;
                  })
                  .AddTo(this);
        }
    }
}