using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    public class FirstTouchUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject first_touch_object_ = null;
        private EventSystem event_system_;

        // Start is called before the first frame update
        void Start()
        {
            event_system_ = EventSystem.current;
            event_system_.SetSelectedGameObject(first_touch_object_);
            BaseEventData baseEventData = new BaseEventData(event_system_);
            baseEventData.selectedObject = event_system_.currentSelectedGameObject;
            first_touch_object_.GetComponent<Selectable>().OnSelect(baseEventData);
            baseEventData.Reset();
        }

        private void OnEnable()
        {
            if (event_system_ == null)
            {
                event_system_ = EventSystem.current;
            }

            event_system_.SetSelectedGameObject(first_touch_object_);
            BaseEventData baseEventData = new BaseEventData(event_system_);
            baseEventData.selectedObject = event_system_.currentSelectedGameObject;
            first_touch_object_.GetComponent<Selectable>().OnSelect(baseEventData);
            baseEventData.Reset();
        }
    }
}