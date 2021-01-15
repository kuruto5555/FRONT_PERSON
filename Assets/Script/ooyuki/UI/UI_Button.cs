using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;
using FrontPerson.Constants;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace FrontPerson.UI
{
    public class UI_Button : MonoBehaviour
    {
        GameObject selectObjectPrev_ = null;


        // Start is called before the first frame update
        private void Start()
        {
            // イベントトリガーの取得
            var trigger = gameObject.AddComponent<EventTrigger>();

            // 登録するイベントを設定する
            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) =>
            {
                if (EventSystem.current == null) return;
                if (EventSystem.current.currentSelectedGameObject == gameObject) return;
                EventSystem.current.SetSelectedGameObject(gameObject);
            });

            // イベントを登録する
            trigger.triggers.Add(entry);

            // 現在選択のオブジェクトを取得
            if (EventSystem.current != null)
            {
                selectObjectPrev_ = EventSystem.current.firstSelectedGameObject;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (EventSystem.current == null) return;

            
            if (gameObject != selectObjectPrev_ && gameObject == EventSystem.current.currentSelectedGameObject)
            {
                //selectObjectPrev_ = EventSystem.current.currentSelectedGameObject;
                AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_CURSOR);
            }

            if (selectObjectPrev_ != EventSystem.current.currentSelectedGameObject)
            {
                selectObjectPrev_ = EventSystem.current.currentSelectedGameObject;
            }
        }
    }
}
