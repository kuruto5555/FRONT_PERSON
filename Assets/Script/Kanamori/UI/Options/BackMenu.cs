using FrontPerson.Constants;
using FrontPerson.Manager;
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
        private OptionMenu menu_cs_ = null;

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetButtonDown(Constants.InputName.CANCEL))
            {
                gameObject.SetActive(false);
                menu_.SetActive(true);

                // 戻るSE再生
                AudioManager.Instance.Play2DSE(menu_, SEPath.COMMON_SE_BACK);

                if (menu_cs_ != null)
                {
                    menu_cs_.ReturnOptionScene();
                }
            }
        }
    }
}
