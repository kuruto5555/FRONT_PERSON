using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using FrontPerson.Manager;
using FrontPerson.Constants;
using UnityEngine.EventSystems;

namespace FrontPerson.UI
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

        [Header("フェイドの時間")]
        [SerializeField]
        private float FadeTime = 0f;

        /// <summary>
        /// イベントシステム
        /// </summary>
        private EventSystem event_system = null;

        private AudioManager audio_manager = null;

        private GameObject current_buttom = null;

        void Start()
        {
#if UNITY_EDITOR
            if (null == StartButton || null == OptionButton || null == ExitButton)
            {
                Debug.LogError("Buttonオブジェクトが設定されていません");
                return;
            }
#endif
            StartButton.onClick.AddListener( () => { 
                SceneManager.Instance.SceneChange(SceneName.GAME_SCENE, FadeTime);
                DecisionSound();
                });

            OptionButton.onClick.AddListener(() => { DecisionSound(); });

            ExitButton.onClick.AddListener(() => {
                DecisionSound();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
            });

            current_buttom = StartButton.gameObject;

            event_system = EventSystem.current;

            audio_manager = AudioManager.Instance;

            // BGMの再生
            audio_manager.PlayBGM(gameObject, BGMPath.TITLE_BGM_MAIN, 1f);
        }

        private void Update()
        {
            // 選択しているものが違う
            if (event_system.currentSelectedGameObject != current_buttom)
            {
                current_buttom = event_system.currentSelectedGameObject;

                audio_manager.Play2DSE(gameObject, SEPath.COMMON_SE_CURSOR);
            }
        }

        /// <summary>
        /// 決定音の再生
        /// </summary>
        private void DecisionSound()
        {
            audio_manager.Play2DSE(gameObject, SEPath.COMMON_SE_DECISION);
        }
    }
}