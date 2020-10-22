using FrontPerson.common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrontPerson.Manager
{
    public class SceneManager : SingletonMonoBehaviour<SceneManager>
    {
        public void SceneChange(string scene_name, float interval_time)
        {
            FadeManager.Instance.LoadScene(scene_name, interval_time);
        }
    }
}
