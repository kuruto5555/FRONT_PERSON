﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace FrontPerson.UI
{
    [Serializable]
    public class UIController
    {
        [Tooltip("指定したメニュー画面を開くボタン")]
        public Button button_to_open_the_menu_;

        [Tooltip("上で指定するメニュー画面")]
        public GameObject menu_;
    }

    public class InGameOptionMenu : MonoBehaviour
    {
        /// <summary>
        /// メニュー画面のコントローラ
        /// ボタンを押すと対応したUIを表示したり
        /// </summary>
        [SerializeField]
        private List<UIController> ui_controllers_ = new List<UIController>();

        /// <summary>
        /// メニューを開いた際に表示させるUIの親オブジェクトを入れておく
        /// </summary>
        [SerializeField]
        private GameObject active_menu_ = null;

        [Tooltip("メニューを開閉ボタンを入力した際に対応するメニューを格納")]
        [SerializeField]
        private List<GameObject> opened_menu_ = new List<GameObject>();

        [Tooltip("タイトルに戻るボタン")]
        [SerializeField]
        private Button return_to_title_button_ = null;

        private EventSystem event_system_;

        private bool is_opened_and_closed_ = true;

        // Start is called before the first frame update
        private void Start()
        {
            event_system_ = EventSystem.current;
            event_system_.SetSelectedGameObject(ui_controllers_[0].button_to_open_the_menu_.gameObject);

            SelectedOptionButtonSettings();
        }

        // Update is called once per frame
        private void Update()
        {
            // メニューを開く
            if (Input.GetButtonDown(Constants.InputName.PAUSE) && is_opened_and_closed_)
            {
                Time.timeScale = (Time.timeScale <= 0) ? 1f : 0f;
                print(Time.timeScale);

                // メニューを開いている場合は閉じる、閉じている場合は開く
                foreach (var menu in opened_menu_)
                {
                    menu.SetActive(!menu.activeSelf);
                }
            }
        }

        /// <summary>
        /// メニューを閉じれるようにする
        /// </summary>
        public void AllowsTheMenuClose()
        {
            is_opened_and_closed_ = true;
            event_system_.SetSelectedGameObject(ui_controllers_[0].button_to_open_the_menu_.gameObject);
        }

        /// <summary>
        /// メニューを閉じれないようにする
        /// </summary>
        private void PreventMenusClosing()
        {
            is_opened_and_closed_ = false;
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
                    // タイトルシーンを呼ぶ
                    Manager.SceneManager.Instance.SceneChange(Constants.SceneName.TITLE_SCENE, .1f);
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

                        // ポーズボタンとBボタンでメニュー画面を閉じれなくする
                        PreventMenusClosing();
                    }
                    );
            }
        }
    }
}
