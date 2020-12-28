using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FrontPerson.Manager;

namespace FrontPerson.UI
{
    public class InGameOptionMenu : OptionMenu
    {
        /// <summary>
        /// 開いたオプション画面をとじれるかどうか（閉じれる <=> 閉じれない）
        /// </summary>
        private bool is_opened_and_closed_ = true;

        /// <summary>
        /// 現在開いているかどうか
        /// true -> メニューを開いている
        /// float -> メニューを閉じている
        /// </summary>
        public bool IsOpen { get; private set; } = false;

        /// <summary>
        /// タイトルへ戻るが押されたかどうか
        /// true -> タイトルへ戻るが押された
        /// false -> タイトルへ戻るがまだ押されていない
        /// </summary>
        public bool IsGoToTitle { get; private set; } = false;

        /// <summary>
        /// アプリケーションマネージャー
        /// </summary>
        ApplicationManager appManager_ = null;

        /// <summary>
        /// メニューを閉じれるようにする
        /// </summary>
        public void AllowsTheMenuClose()
        {
            is_opened_and_closed_ = true;

            FirstTouchSelectable.Select(event_system_, ui_controllers_[0].button_to_open_the_menu_);
        }

        /// <summary>
        /// メニューを閉じれないようにする
        /// </summary>
        private void PreventMenusClosing()
        {
            is_opened_and_closed_ = false;
        }

        protected override void OnStart()
        {
            appManager_ = FindObjectOfType<ApplicationManager>();

            return_option_scene_ += AllowsTheMenuClose;

            return_to_title_button_.onClick.AddListener(
                () =>
                {
                    Time.timeScale = 1f;
                    appManager_.SetIsInput(false);
                    IsGoToTitle = true;
                    Manager.SceneManager.Instance.SceneChange(Constants.SceneName.TITLE_SCENE, 0.5f);
                }
                );

            foreach (var ui in ui_controllers_)
            {
                // ボタンを押した際にどのメニューを開くのかを設定
                ui.button_to_open_the_menu_.onClick.AddListener(
                    () =>
                    {
                        // ポーズボタンとBボタンでメニュー画面を閉じれなくする
                        PreventMenusClosing();
                    }
                    );
            }
        }

        protected override void OnUpdate()
        {
            // タイトルへが押されていたら処理しない
            if (!appManager_.IsInput) return;
            if (IsGoToTitle) return;

            // メニュー開閉
            if (Input.GetButtonDown(Constants.InputName.PAUSE) && is_opened_and_closed_)
            {
                // メニュー画面を開いている間、時間を止める
                Time.timeScale = (opened_menu_[0].activeSelf) ? 1f : 0f;
                print(Time.timeScale);

                // メニューを開いている場合は閉じる、閉じている場合は開く
                foreach (var menu in opened_menu_)
                {
                    menu.SetActive(!menu.activeSelf);
                    IsOpen = menu.activeSelf;
                }

                FirstTouchSelectable.Select(event_system_, ui_controllers_[0].button_to_open_the_menu_);
            }
        }
    }
}
