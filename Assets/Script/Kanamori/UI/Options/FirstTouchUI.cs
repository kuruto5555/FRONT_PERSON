using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FrontPerson.UI
{
    public class FirstTouchUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject first_touch_object_ = null;

        // Start is called before the first frame update
        void Start()
        {
            EventSystem.current.SetSelectedGameObject(first_touch_object_);
        }
        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(first_touch_object_);
        }
    }
}