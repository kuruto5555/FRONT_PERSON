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
        [SerializeField]
        private GameObject[] active_menu_;

        [SerializeField]
        private OptionUI[] option_ui_;

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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                foreach (var obj in active_menu_)
                {
                    obj.SetActive(!obj.activeSelf);
                }
            }
        }
        /// <summary>
        /// オプションのタブボタンに対応したメニューを開くための設定
        /// </summary>
        private void SelectedOptionButtonSettings()
        {
            if (option_ui_.Length != 0)
            {
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

                        current_selected_ui_ = ui;
                    }
                    );
            }
        }
    }
}
