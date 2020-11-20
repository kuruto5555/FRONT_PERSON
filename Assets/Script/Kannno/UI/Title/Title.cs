using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using FrontPerson.Manager;

namespace FrontPerson
{
    public class Title : MonoBehaviour
    {
        [Header("スタートボタン")]
        [SerializeField]
        private Button StartButton = null;

        [Header("オプションボタン")]
        [SerializeField]
        private Button OptionButton = null;

        [Header("終了ボタン")]
        [SerializeField]
        private Button ExitButton = null;

        void Start()
        {
#if UNITY_EDITOR
            if (null == StartButton || null == OptionButton || null == ExitButton)
            {
                Debug.LogError("Buttonオブジェクトが設定されていません");
            }
#endif

            StartButton.onClick.AddListener( () => { SceneManager.Instance.SceneChange("GameScene", 1.0f); });

            OptionButton.onClick.AddListener(() => { return; });

            ExitButton.onClick.AddListener(() => {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
            });
        }
    }
}