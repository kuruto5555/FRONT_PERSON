using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;
using FrontPerson.Constants;


namespace FrontPerson.UI
{
    public class BackButton : MonoBehaviour
    {
        bool isCrick = false;

        public void OnCrick()
        {
            if (isCrick) return;
            SceneManager.Instance.SceneChange(SceneName.TITLE_SCENE, 2.0f, Color.black);
            isCrick = true;
        }
    }
}
