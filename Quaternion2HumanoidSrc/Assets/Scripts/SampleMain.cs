﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Assets.Quaternion2Humanoid.Scripts {
    public class SampleMain : MonoBehaviour {
        [SerializeField] Transform mainObjectTransform;
        [SerializeField] UIReactiveQuaternion reactiveQuaternion;

        void Awake() {
            reactiveQuaternion.OverrideQuaternion(mainObjectTransform.rotation);
            reactiveQuaternion.ReactiveQuaternion.Subscribe(
                q => { mainObjectTransform.rotation = q; }
            );
        }
    }
}
