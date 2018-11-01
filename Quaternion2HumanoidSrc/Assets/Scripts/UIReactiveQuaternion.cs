﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Assets.Quaternion2Humanoid.Scripts {
    public class UIReactiveQuaternion : MonoBehaviour {
        [SerializeField] Button resetButton;
        [SerializeField] ValueSlider sliderW;
        [SerializeField] ValueSlider sliderX;
        [SerializeField] ValueSlider sliderY;
        [SerializeField] ValueSlider sliderZ;

        ReactiveProperty<Quaternion> reactiveQuaternion = new ReactiveProperty<Quaternion>();
        public IReadOnlyReactiveProperty<Quaternion> ReactiveQuaternion { get { return reactiveQuaternion; } }
        bool isLockReactiveQuaternion = true;

        Quaternion defaultQuaternion = Quaternion.identity;
        public void SetDefaultQuaternion(Quaternion quaternion) {
            sliderW.Slider.value = quaternion.w;
            sliderX.Slider.value = quaternion.x;
            sliderY.Slider.value = quaternion.y;
            sliderZ.Slider.value = quaternion.z;
            defaultQuaternion = quaternion;
        }

        public void InitQuaternion() { SetQuaternion(defaultQuaternion); }

        private void SetQuaternion(Quaternion quaternion) {
            sliderW.Slider.value = quaternion.w;
            sliderX.Slider.value = quaternion.x;
            sliderY.Slider.value = quaternion.y;
            sliderZ.Slider.value = quaternion.z;
        }

        void Awake() {
            // リセットボタン
            resetButton.onClick.AddListener(InitQuaternion);
            // sliderによるQuaternion更新を通知
            Observable.CombineLatest(
                sliderW.ReactiveValue,
                sliderX.ReactiveValue,
                sliderY.ReactiveValue,
                sliderZ.ReactiveValue
            )
            .Where(_ => { return isLockReactiveQuaternion; })
            .Subscribe(values => {
                var quat = new Quaternion(values[1], values[2], values[3], values[0]).normalized;
                reactiveQuaternion.Value = quat;
                // scriptからの上書きはReactivePropertyとして通知しない
                isLockReactiveQuaternion = false;
                SetQuaternion(quat);
                isLockReactiveQuaternion = true;
            }).AddTo(this);
        }
    }
}