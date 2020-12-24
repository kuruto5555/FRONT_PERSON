using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    public class FirstTouchSelectable
    {
        public static void Select(EventSystem event_system, Selectable select_object)
        {
            // メニュー画面を表示した際、ボタンの一つ目を選択する
            event_system.SetSelectedGameObject(select_object.gameObject);
            BaseEventData baseEventData = new BaseEventData(event_system);
            baseEventData.selectedObject = event_system.currentSelectedGameObject;
            select_object.OnSelect(baseEventData);
            baseEventData.Reset();
        }
    }
}
