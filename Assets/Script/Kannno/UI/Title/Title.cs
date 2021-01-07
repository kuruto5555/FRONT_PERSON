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
        [Header("タイトルのオブジェクト")]
        [SerializeField]
        private GameObject TitleMenu = null;

        [Header("オプションのオブジェクト")]
        [SerializeField]
        private GameObject OptionMenu = null;

        [Header("スタートボタン")]
        [SerializeField]
        private Button StartButton = null;

        [Header("ランキングボタン")]
        [SerializeField]
        private Button RankingButton = null;

        [Header("オプションボタン")]
        [SerializeField]
        private Button OptionButton = null;

        [Header("終了ボタン")]
        [SerializeField]
        private Button ExitButton = null;

        [Header("フェイドの時間")]
        [SerializeField]
        private float FadeTime = 0f;

        [Header("AudioSouceのオブジェクト")]
        [SerializeField]
        private GameObject AudioSouce = null;

        [Tooltip("オーディオソースの為")]
        [Header("キャンバス")]
        [SerializeField]
        private GameObject Canvas = null;

        /// <summary>
        /// イベントシステム
        /// </summary>
        private EventSystem event_system = null;

        private AudioManager audio_manager = null;

        private GameObject current_buttom = null;

        /// <summary>
        /// オプション
        /// </summary>
        private InTitleOption menu = null;

        private ApplicationManager ApplicationManager = null;

        void Start()
        {
#if UNITY_EDITOR
            if (null == StartButton || null == RankingButton || null == OptionButton || null == ExitButton)
            {
                Debug.LogError("Buttonオブジェクトが設定されていません");
                return;
            }

            if(null == TitleMenu || null == OptionMenu)
            {
                Debug.LogError("タイトルオブジェクト or オプションオブジェクトが設定されていません");
                return;
            }

            if(null == Canvas)
            {
                Debug.LogError("キャンバスオブジェクトが設定されていません");
                return;
            }

            if (null == AudioSouce)
            {
                Debug.LogError("AudioSouceのオブジェクトが設定されていません");
                return;
            }
#endif

            menu = OptionMenu.GetComponent<InTitleOption>();

            event_system = EventSystem.current;

            audio_manager = AudioManager.Instance;

            ApplicationManager = ApplicationManager.Instance;

            TitleMenu.SetActive(true);

            SetButtonActive(true);

            StartButton.onClick.AddListener( () => {
                if (ApplicationManager.IsInput)
                {
                    SceneManager.Instance.SceneChange(SceneName.GAME_SCENE, FadeTime);
                    DecisionSound();
                    SetButtonActive(false);
                }
            });

            RankingButton.onClick.AddListener(() => {
                if (ApplicationManager.IsInput)
                {
                    SceneManager.Instance.SceneChange(SceneName.RANKING_SCENE, FadeTime);
                    DecisionSound();
                    SetButtonActive(false);
                }
            });

            OptionButton.onClick.AddListener(() => {
                if (ApplicationManager.IsInput)
                {
                    TitleMenu.SetActive(false);
                    menu.OpenMenu();
                    DecisionSound();
                    SetButtonActive(false);
                }
            });

            ExitButton.onClick.AddListener(() => {
                DecisionSound();
                SetButtonActive(false);
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
            });

            current_buttom = StartButton.gameObject;
            event_system.SetSelectedGameObject(StartButton.gameObject);

            // BGMの再生
            audio_manager.PlayBGM(Canvas, BGMPath.TITLE_BGM_MAIN, 1f);
        }

        private void Update()
        {
            if (ApplicationManager.IsInput)
            {
                // 選択しているものが違う
                if (event_system.currentSelectedGameObject != current_buttom)
                {
                    current_buttom = event_system.currentSelectedGameObject;

                    audio_manager.Play2DSE(gameObject, SEPath.COMMON_SE_CURSOR);
                }
            }
        }

        /// <summary>
        /// 決定音の再生
        /// </summary>
        private void DecisionSound()
        {
            audio_manager.Play2DSE(AudioSouce, SEPath.COMMON_SE_DECISION);
        }

        public void OpenMenu()
        {
            TitleMenu.SetActive(true);

            SetButtonActive(true);

            event_system.SetSelectedGameObject(StartButton.gameObject);
            current_buttom = StartButton.gameObject;
        }

        private void SetButtonActive(bool flag)
        {
            StartButton.enabled = flag;
            RankingButton.enabled = flag;
            OptionButton.enabled = flag;
            ExitButton.enabled = flag;
        }
    }
}