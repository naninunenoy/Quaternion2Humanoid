﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using Quaternion2Humanoid;
using Assets.Quaternion2Humanoid.Scripts.UI;

namespace Assets.Quaternion2Humanoid.Scripts {
    public class SampleMain : MonoBehaviour {
        [SerializeField] Transform mainObjectTransform;
        [SerializeField] UIReactiveQuaternion reactiveQuaternion;
        [SerializeField] Button humanoidSceneButton;

        void Awake() {
            reactiveQuaternion.Validate(this);
        }

        void Start() {
            reactiveQuaternion.ReactiveQuaternion.SubscribeToLocalRotation(mainObjectTransform).AddTo(this);
            // load scene event
            humanoidSceneButton.onClick.AddListener(() => { SceneManager.LoadScene("RightArmScene"); });
        }
    }
}
