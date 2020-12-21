using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.UI
{
    public class RollbackScene : MonoBehaviour
    {
        [SerializeField]
        private string input_button_ = null;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown(input_button_))
            {
                Manager.SceneManager.Instance.SceneChange(Manager.SceneManager.Instance.last_scene_name_, 0.5f);
            }
        }
    }
}