using FrontPerson.common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Manager
{
    public class ApplicationManager : MonoBehaviour
    {
        /// <summary>
        /// 今回のスコア
        /// </summary>
        public int Score = 0;

        /// <summary>
        /// 今回のコンボ数
        /// </summary>
        public int ComboNum = 0;

        /// <summary>
        /// 今回のクリアしたミッション数
        /// </summary>
        public int ClearMissionNum = 0;

        /// <summary>
        /// 入力を受け付けるかどうか
        /// </summary>
        public bool IsInput = true;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void RuntimeInit()
        {
            var go = new GameObject("ApplicationManager", typeof(ApplicationManager));

            // オーディオマネージャーを追加
            {
                var am = go.AddComponent<AudioManager>();
                am.Init();

                go.AddComponent<SceneManager>();
                go.AddComponent<FadeManager>();
            }

            DontDestroyOnLoad(go);
        }

        /// <summary>
        /// アプリ終了時呼ばれる
        /// </summary>
        private void OnApplicationQuit()
        {
        }
    }
}
