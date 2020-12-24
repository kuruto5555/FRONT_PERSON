using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    public class InTitleOption : OptionMenu
    {
        [SerializeField]
        private Title TitleMenu = null;

        public void OpenMenu()
        {
            // メニューを開く
            foreach (var menu in opened_menu_)
            {
                menu.SetActive(true);
            }

            FirstTouchSelectable.Select(event_system_, ui_controllers_[0].button_to_open_the_menu_);
        }

        protected override void OnStart()
        {
            return_to_title_button_.onClick.AddListener(
                   () =>
                   {
                       // メニューを閉じる
                       foreach (var menu in opened_menu_)
                       {
                           TitleMenu.OpenMenu();
                       }
                   }
                   );
        }

        protected override void OnUpdate()
        {
        }
    }
}
