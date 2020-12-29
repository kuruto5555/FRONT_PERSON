using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace FrontPerson.UI
{
    [Serializable]
    public class UIController
    {
        [Tooltip("指定したメニュー画面を開くボタン")]
        public Button button_to_open_the_menu_;

        [Tooltip("上で指定したメニュー画面")]
        public GameObject menu_;
    }

    public abstract class OptionMenu : MonoBehaviour
    {
        /// <summary>
      /// メニュー画面のコントローラ
      /// ボタンを押すと対応したUIを表示したり
      /// </summary>
        [SerializeField]
        protected List<UIController> ui_controllers_ = new List<UIController>();

        /// <summary>
        /// メニューを開いた際に表示させるUIの親オブジェクトを入れておく
        /// </summary>
        [SerializeField]
        private GameObject active_menu_ = null;

        [Tooltip("メニューを開閉ボタンを入力した際に対応するメニューを格納")]
        [SerializeField]
        protected List<GameObject> opened_menu_ = new List<GameObject>();

        [Tooltip("タイトルに戻るボタン")]
        [SerializeField]
        protected Button return_to_title_button_ = null;

        [Tooltip("オーディオ設定ボタン")]
        [SerializeField]
        protected Button audioSetting_button_ = null;

        [Tooltip("感度設定ボタン")]
        [SerializeField]
        protected Button sensivity_button_ = null;

        [Tooltip("操作設定ボタン")]
        [SerializeField]
        protected Button operationSettings_button_ = null;

        protected EventSystem event_system_;

        protected UnityAction return_option_scene_;

        // Start is called before the first frame update
        private void Start()
        {
            event_system_ = EventSystem.current;

            SelectedOptionButtonSettings();

            return_option_scene_ += ()=> { FirstTouchSelectable.Select(event_system_, ui_controllers_[0].button_to_open_the_menu_); };

            OnStart();
        }

        // Update is called once per frame
        private void Update()
        {
            OnUpdate();
        }

        /// <summary>
        /// オプション画面に戻る
        /// </summary>
        public void ReturnOptionScene()
        {
            if (return_option_scene_ != null)
            {
                return_option_scene_.Invoke();
            }
        }

        /// <summary>
        /// 各種ボタンの設定
        /// </summary>
        private void SelectedOptionButtonSettings()
        {
            // タイトルに戻るボタン
            return_to_title_button_.onClick.AddListener(
                () =>
                {
                    // メニューを閉じる
                    foreach (var menu in opened_menu_)
                    {
                        menu.SetActive(false);
                    }
                }
                );

            foreach (var ui in ui_controllers_)
            {
                // ボタンを押した際にどのメニューを開くのかを設定
                ui.button_to_open_the_menu_.onClick.AddListener(
                    () =>
                    {
                        // メニューを閉じる
                        active_menu_.SetActive(false);

                        // ボタンに対応したメニューを開く
                        ui.menu_.SetActive(true);
                    }
                    );
            }
        }

        protected abstract void OnStart();
        protected abstract void OnUpdate();
    }
}