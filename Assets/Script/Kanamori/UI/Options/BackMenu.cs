using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.UI
{
    public class BackMenu : MonoBehaviour
    {
        [Tooltip("戻りたいメニュー")]
        [SerializeField]
        private GameObject menu_ = null;

        [SerializeField]
        private InGameOptionMenu menu_cs_ = null;

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetButtonDown(Constants.InputName.CANCEL))
            {
                gameObject.SetActive(false);
                menu_.SetActive(true);

                if(menu_cs_ != null)
                {
                    menu_cs_.AllowsTheMenuClose();
                }
            }
        }
    }
}
