﻿using System.Collections;
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.LeftUpperArm, leftUpperArmReactiveQuaternion.ReactiveQuaternion).AddTo(this);
            humanoidBones.SbscribeAsGlobalQuaternionTo(HumanBodyBones.LeftLowerArm, leftLowerArmReactiveQuaternion.ReactiveQuaternion).AddTo(this);
            // observer
            humanoidBones.ReactiveBones.Subscribe(
                    Debug.LogFormat("{0} : q={1}", bone.Bone, bone.Quaternion);
                }
            sampleSceneButton.onClick.AddListener(() => { SceneManager.LoadScene("SampleScene"); });