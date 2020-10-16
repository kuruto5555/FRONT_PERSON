#if UNITY_EDITOR

using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

using FrontPerson.Constants;

namespace FrontPerson.Manager
{
    /// <summary>
    /// オーディオのファイル名を定数で管理するクラスを作成するスクリプト
    /// </summary>
    public static class AudioNameCreator
    {
        private const string NAME_SPACE = "FrontPerson.Constants";

        private const string COMMAND_NAME = "Tools/Create/Audio Name";        // コマンド名

        public const string BGM_DIRECTORY_PATH = "Resources/BGM";
        public const string SE_DIRECTORY_PATH = "Resources/SE";

        /// <summary>
        /// オーディオのファイル名を定数で管理するクラスを作成します
        /// </summary>
        [MenuItem(COMMAND_NAME)]
        public static void Create()
        {
            if (!CanCreate())
            {
                return;
            }

            Creator(BGM_DIRECTORY_PATH);
            Creator(SE_DIRECTORY_PATH);

//            EditorUtility.DisplayDialog("Complete", "作成が完了しました", "OK");
        }

        public static void Creator(string directory_path)
        {
            //オーディオファイルへのパスを抽出
            string directory_name = Path.GetFileName(directory_path);
            var audio_path_dict = new Dictionary<string, string>();

            foreach (var audio_clip in Resources.LoadAll<AudioClip>(directory_name))
            {
                //アセットへのパスを取得
                var asset_path = AssetDatabase.GetAssetPath(audio_clip);

                //オーディオ名の重複チェック
                var audio_name = audio_clip.name;
                if (audio_path_dict.ContainsKey(audio_name))
                {
                    Debug.LogError(audio_name + " is duplicate!\n1 : " + directory_name + "/" + audio_name + "\n2 : " + audio_path_dict[audio_name]);
                }
                audio_path_dict[audio_name] = directory_name + "/" + audio_name;
            }

            //定数クラス作成
            ConstantsClassCreator.Create(directory_name + "Path", directory_name + "ファイルへのパスを定数で管理するクラス", audio_path_dict, ConstantsClassCreator.PATH, NAME_SPACE);

        }

        /// <summary>
        /// オーディオのファイル名を定数で管理するクラスを作成できるかどうかを取得します
        /// </summary>
        [MenuItem(COMMAND_NAME, true)]
        private static bool CanCreate()
        {
            return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
        }

    }
}
#endif