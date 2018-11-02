using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Quaternion2Humanoid.Scripts {
    public class HumanoidBodyMain : MonoBehaviour {
        [SerializeField] Button leftArmSceneButton;

        void Start() {
            // load scene event
            leftArmSceneButton.onClick.AddListener(() => { SceneManager.LoadScene("LeftArmScene"); });
        }
    }
}
