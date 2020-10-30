using FrontPerson.common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Manager
{
    public class FadeManager : SingletonMonoBehaviour<FadeManager>
    {
        /// <summary>
        /// フェード中の透明度
        /// </summary>
        private float fade_alpha_ = 0f;

        /// <summary>
        /// フェード遷移中かどうか
        /// </summary>
        private bool is_fade_ = false;

        /// <summary>
        /// フェード遷移の色
        /// </summary>
        private Color fade_color_ = Color.white;

        public void OnGUI()
        {
            if (is_fade_)
            {
                fade_color_.a = fade_alpha_;
                GUI.color = fade_color_;
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            }
        }

        /// <summary>
        /// 画面遷移
        /// </summary>
        /// <param name="scene_name"></param>
        /// <param name="interval_time"></param>
        public void LoadScene(string scene_name, float interval_time, Color fade_color = default)
        {
            fade_color_ = fade_color;
            if (fade_color_ == default)
            {
                fade_color_ = Color.white;
            }

            StartCoroutine(Fade(scene_name, interval_time));
        }

        /// <summary>
        /// フェード遷移
        /// </summary>
        /// <param name="scene_name"></param>
        /// <param name="interval_time"></param>
        /// <returns></returns>
        private IEnumerator Fade(string scene_name, float interval_time)
        {
            // FadeIn
            is_fade_ = true;
            float time = 0;
            while(time <= interval_time)
            {
                fade_alpha_ = Mathf.Lerp(0f, 1f, time / interval_time);
                time += Time.deltaTime;
                yield return 0;
            }

            // シーン遷移
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene_name);

            // FadeOut
            time = 0;
            while(time <= interval_time)
            {
                fade_alpha_ = Mathf.Lerp(1f, 0f, time / interval_time);
                time += Time.deltaTime;
                yield return 0;
            }

            is_fade_ = false;
        }
    }
}
