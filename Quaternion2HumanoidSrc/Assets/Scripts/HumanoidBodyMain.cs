using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Quaternion2Humanoid.Scripts {
    public class HumanoidBodyMain : MonoBehaviour {
        [SerializeField] Button leftArmSceneButton;
        [SerializeField] Transform toggleParent;
        [SerializeField] Transform panelParent;

        void Awake() {
            var toggles = Enumerable.Range(0, toggleParent.childCount)
                                    .Select(i => toggleParent.GetChild(i))
                                    .Select(t => t.GetComponent<Toggle>()).ToArray();
            var panels = Enumerable.Range(0, panelParent.childCount)
                                   .Select(i => panelParent.GetChild(i))
                                   .Select(t => t.GetComponent<Transform>()).ToArray();;
            if (toggles.Length == panels.Length) {
                var len = toggles.Length;
                // toggleのon/offでパネルを切り替える
                for (var i = 0; i < len; i++) {
                    var t = toggles[i];
                    var p = panels[i];
                    t.onValueChanged.AddListener(p.gameObject.SetActive);
                }
            }
        }

        void Start() {
            // load scene event
            leftArmSceneButton.onClick.AddListener(() => { SceneManager.LoadScene("LeftArmScene"); });
        }
    }
}
