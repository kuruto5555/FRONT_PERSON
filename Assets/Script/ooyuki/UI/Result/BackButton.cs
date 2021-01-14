using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;
using FrontPerson.Constants;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace FrontPerson.UI
{
    public class BackButton : MonoBehaviour
    {
        bool isCrick = false;

        [SerializeField]
        private Button button = null;

        private void Start()
        {
            // イベントトリガーの取得
            var trigger = gameObject.AddComponent<EventTrigger>();

            // 登録するイベントを設定する
            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => {
                if (EventSystem.current == null) return;
                if (EventSystem.current.currentSelectedGameObject == gameObject) return;
                EventSystem.current.SetSelectedGameObject(gameObject); });

            // イベントを登録する
            trigger.triggers.Add(entry);
        }

        public void OnCrick()
        {
            if (isCrick) return;

            EventSystem.current.enabled = false;
            AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_BACK);
            SceneManager.Instance.SceneChange(SceneName.TITLE_SCENE, 2.0f, Color.black);
            isCrick = true;

            button.enabled = false;
        }



    }

}
