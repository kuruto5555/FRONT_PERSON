using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    public class InTitleOption : MonoBehaviour
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

        public bool MenuActive { get; private set; } = false;

        private EventSystem event_system_;

        // Start is called before the first frame update
        private void Start()
        {
            event_system_ = EventSystem.current;
            
            SelectedOptionButtonSettings();
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void OpenMenu()
        {
            // メニューを開く
            foreach (var menu in opened_menu_)
            {
                menu.SetActive(true);
            }

            FirstTouchSelectable();

            MenuActive = true;
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
                        MenuActive = false;
                    }
                }
                );

            FirstTouchSelectable();

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

        /// <summary>
        /// Selectableオブジェクトを選択中にする
        /// </summary>
        private void FirstTouchSelectable()
        {
            // メニュー画面を表示した際、ボタンの一つ目を選択する
            event_system_.SetSelectedGameObject(ui_controllers_[0].button_to_open_the_menu_.gameObject);
            BaseEventData baseEventData = new BaseEventData(event_system_);
            baseEventData.selectedObject = event_system_.currentSelectedGameObject;
            ui_controllers_[0].button_to_open_the_menu_.OnSelect(baseEventData);
            baseEventData.Reset();
        }
    }
}
