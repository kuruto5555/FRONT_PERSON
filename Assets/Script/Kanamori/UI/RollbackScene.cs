using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    public class RollbackScene : MonoBehaviour
    {
        [SerializeField]
        private Button back_button_ = null;

        // Start is called before the first frame update
        void Start()
        {
            if(back_button_ != null)
            {
                back_button_.onClick.AddListener(
                    () =>
                    {
                        Manager.SceneManager.Instance.SceneChange(Manager.SceneManager.Instance.last_scene_name_, 0.5f);
                    }
                    );
            }
        }
    }
}