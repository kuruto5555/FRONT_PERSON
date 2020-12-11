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
        /// 前回のシーン
        /// </summary>
        public string last_scene_name_ { get; private set; } = null;

        /// <summary>
        /// 現在のシーン
        /// タイトルシーンからゲームが始まるはずなので、初期値はタイトルシーンを入れておきます。
        /// </summary>
        public string current_scene_name_ { get; private set; } = Constants.SceneName.TITLE_SCENE;

        /// <summary>
        /// シーンを変更
        /// </summary>
        /// <param name="scene_name"></param>
        /// <param name="interval_time"></param>
        /// <param name="fade_color"></param>
        public void SceneChange(string scene_name, float interval_time, Color fade_color = default)
        {
            FadeManager.Instance.LoadScene(scene_name, interval_time, fade_color);

            // 前回のシーンを保存
            last_scene_name_ = current_scene_name_;
            // 現在のシーンを保存
            current_scene_name_ = scene_name;
        }
    }
}
