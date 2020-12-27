﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;
using FrontPerson.Constants;
using UnityEngine.EventSystems;

namespace FrontPerson.UI
{
    public class BackButton : MonoBehaviour
    {
        bool isCrick = false;


        public void OnCrick()
        {
            if (isCrick) return;

            EventSystem.current.enabled = false;
            AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_BACK);
            SceneManager.Instance.SceneChange(SceneName.TITLE_SCENE, 2.0f, Color.black);
            isCrick = true;
        }
    }
}
