using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    /// <summary>
    /// メニュー画面内のタブボタンを押した際、それに対応したメニューを開くためのクラス
    /// </summary>
    [Serializable]
    public class OptionUI
    {
        [Tooltip("オプション画面のメニュー切り替えボタン")]
        public Button tab_button_;

        [Tooltip("オプション画面の切り替わるメニュー画面")]
        public GameObject menu_ui_;
    }

    public class OptionUIController : MonoBehaviour
    {
        /// <summary>
        /// メニューを開いた際に表示させたいUIの親オブジェクトを入れておく
        /// </summary>
        [SerializeField]
        private GameObject[] active_menu_;

        /// <summary>
        /// オプション画面をタブ分けして、対応したタブに対応したUIを開く
        /// </summary>
        [SerializeField]
        private OptionUI[] option_ui_;

        /// <summary>
        /// 現在選択中のタブとそれに対応したUI
        /// </summary>
        private OptionUI current_selected_ui_;

        private EventSystem event_sytem_;

        // Start is called before the first frame update
        private void Start()
        {
            event_sytem_ = EventSystem.current;
            SelectedOptionButtonSettings();
        }

        private void Update()
        {
            // メニューを開く
            if (Input.GetButtonDown(Constants.InputName.PAUSE))
            {
                foreach (var obj in active_menu_)
                {
                    // メニューを開いている場合は閉じる、閉じている場合は開く
                    obj.SetActive(!obj.activeSelf);
                }
            }
        }
        /// <summary>
        /// オプションのタブボタンに対応したメニューを開くための設定
        /// </summary>
        private void SelectedOptionButtonSettings()
        {
            // オプション画面を開いた際に選択できるUIのタブの数を調べる
            if (option_ui_.Length != 0)
            {
                // 配列の1番目の物を現在選択中のタブにする
                current_selected_ui_ = option_ui_[0];
                current_selected_ui_.menu_ui_.SetActive(true);
                event_sytem_.SetSelectedGameObject(current_selected_ui_.tab_button_.gameObject);
            }

            foreach (var ui in option_ui_)
            {
                // ボタンを押した際にどのメニューを開くのかを設定
                ui.tab_button_.onClick.AddListener(
                    () =>
                    {
                        // 前回開いていたメニューを閉じる
                        current_selected_ui_.menu_ui_.SetActive(false);

                        // ボタンに対応したメニューを開く
                        ui.menu_ui_.SetActive(true);

                        // 現在選択中のUIを変更
                        current_selected_ui_ = ui;
                    }
                    );
            }
        }
    }
}
