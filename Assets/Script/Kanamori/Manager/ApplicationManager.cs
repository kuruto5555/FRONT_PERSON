using FrontPerson.common;
using FrontPerson.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
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
