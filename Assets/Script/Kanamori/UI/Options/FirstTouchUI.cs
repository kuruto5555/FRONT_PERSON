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
        private Selectable first_touch_object_ = null;
        private EventSystem event_system_;

        // Start is called before the first frame update
        void Start()
        {
            event_system_ = EventSystem.current;

            FirstTouchSelectable.Select(event_system_, first_touch_object_);
        }

        private void OnEnable()
        {
            if (event_system_ == null)
            {
                event_system_ = EventSystem.current;
            }

            FirstTouchSelectable.Select(event_system_, first_touch_object_);
        }
    }
}