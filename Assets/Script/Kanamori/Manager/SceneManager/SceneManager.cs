using FrontPerson.common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrontPerson.Manager
{
    public class SceneManager : SingletonMonoBehaviour<SceneManager>
    {
        /// <summary>
        /// シーンを変更
        /// </summary>
        /// <param name="scene_name"></param>
        /// <param name="interval_time"></param>
        /// <param name="fade_color"></param>
        public void SceneChange(string scene_name, float interval_time, Color fade_color = default)
        {
            FadeManager.Instance.LoadScene(scene_name, interval_time, fade_color);
        }
    }
}
