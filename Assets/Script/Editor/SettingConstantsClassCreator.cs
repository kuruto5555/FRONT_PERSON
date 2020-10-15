//#if UNITY_EDITOR

using System.IO;
using System.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using StealthPackMan.Manager;

namespace StealthPackMan.Constants
{
    public class SettingConstantsClassCreator : AssetPostprocessor
    {
        // 読み込むファイルパス
        private const string TARGET_DIRECTORY_NAME = "ProjectSettings/";
        private const string TAG_PATH = "TagManager.asset";
        private const string SCENE_PATH = "EditorBuildSettings.asset";
        private const string INPUT_PATH = "InputManager.asset";

        // コマンド関連
        private const string COMMAND_NAME = "Tools/Create/ProjectSettings ConstantsClass";
        private const string COMMAND_ALL = COMMAND_NAME + "/All";
        private const string COMMAND_TAG = COMMAND_NAME + "/Tag";
        private const string COMMAND_LAYER = COMMAND_NAME + "/Layer & LayerMask";
        private const string COMMAND_SORTING_LAYERS = COMMAND_NAME + "/SortingLayer";
        private const string COMMAND_SCENE = COMMAND_NAME + "/Scene";
        private const string COMMAND_INPUT = COMMAND_NAME + "/Input";
        private const string COMMAND_STAGE = COMMAND_NAME + "/StageData";

        // 定数クラスの生成場所
        private const string DIRECTORY_PATH = "Assets/Script/Functions/Constants";

        // 定数クラス生成時の名前空間
        private const string NAME_SPACE = "StealthPackMan.Constants";


#if !UNITY_CLOUD_BUILD
        // プロジェクトセッティングに変更がかかったら自動でスクリプトを書き換える
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            var assets_list = new List<string[]>()
            {
                importedAssets, deletedAssets, movedAssets, movedFromAssetPaths
            };

            // プロジェクトセッティングの情報から定数管理のクラスを生成する
            if (ExistsDirectoryInAssets(assets_list, TARGET_DIRECTORY_NAME + TAG_PATH))
            {
                CreateTags();
                CreateLayerAndLayerMask();
                CreateSortingLayer();
            }

            if(ExistsDirectoryInAssets(assets_list, TARGET_DIRECTORY_NAME + SCENE_PATH))
            {
                CreateScene();
            }

            if(ExistsDirectoryInAssets(assets_list, TARGET_DIRECTORY_NAME + INPUT_PATH))
            {
                CreateInput();
            }

            // Resourcesのサウンド系の情報変更から定数管理のクラスを生成する
            if(ExistsDirectoryInAssets(assets_list, SoundNameCreator.BGM_DIRECTORY_PATH))
            {
                SoundNameCreator.Creator(SoundNameCreator.BGM_DIRECTORY_PATH);
            }
            if (ExistsDirectoryInAssets(assets_list, SoundNameCreator.SE_DIRECTORY_PATH))
            {
                SoundNameCreator.Creator(SoundNameCreator.SE_DIRECTORY_PATH);
            }
        }

        [MenuItem(COMMAND_ALL)]
        private static void CreateAll()
        {
            if(!CanCreate())
            {
                return;
            }

            // タグ
            CreateTags();

            // レイヤーとレイヤーマスク
            CreateLayerAndLayerMask();

            // ソーティングレイヤー
            CreateSortingLayer();

            // シーン
            CreateScene();

            // ステージ
            CreateStageName();
        }

        /// <summary>
        /// タグの定数管理クラスを生成
        /// </summary>
        [MenuItem(COMMAND_TAG)]
        private static void CreateTags()
        {
            var tag_dic = InternalEditorUtility.tags.ToDictionary(value => value);
            ConstantsClassCreator.Create("TagName", "タグ名を定数管理するクラス", tag_dic, DIRECTORY_PATH, NAME_SPACE);
        }

        /// <summary>
        /// レイヤーとレイヤーマスクの定数管理クラスを生成
        /// </summary>
        [MenuItem(COMMAND_LAYER)]
        private static void CreateLayerAndLayerMask()
        {
            var layer_num_dic = InternalEditorUtility.layers.ToDictionary(layer => layer, layer => LayerMask.NameToLayer(layer));
            var layer_mask_num_dic = InternalEditorUtility.layers.ToDictionary(layer => layer, layer => 1 << LayerMask.NameToLayer(layer));
            ConstantsClassCreator.Create("LayerNumber", "レイヤー番号を定数管理するクラス", layer_num_dic, DIRECTORY_PATH, NAME_SPACE);
            ConstantsClassCreator.Create("LayerMaskNumber", "レイヤーマスク番号を定数管理するクラス", layer_mask_num_dic, DIRECTORY_PATH, NAME_SPACE);
        }

        /// <summary>
        /// ソーティングレイヤーの定数管理クラスを生成
        /// </summary>
        [MenuItem(COMMAND_SORTING_LAYERS)]
        private static void CreateSortingLayer()
        {
            var sorting_layer = GetSortingLayerNames().ToDictionary(value => value);
            ConstantsClassCreator.Create("SortingLayer", "ソーティングレイヤー名を定数管理するクラス", sorting_layer, DIRECTORY_PATH, NAME_SPACE);
        }

        /// <summary>
        /// シーンの定数管理クラスを生成
        /// </summary>
        [MenuItem(COMMAND_SCENE)]
        private static void CreateScene()
        {
            var scenes_name_dic = new Dictionary<string, string>();
            for (int i = 0; i < EditorBuildSettings.scenes.Count(); i++)
            {
                var scene_name = Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path);
                scenes_name_dic[scene_name] = scene_name;
            }
            ConstantsClassCreator.Create("SceneName", "シーン名を定数管理するクラス", scenes_name_dic, DIRECTORY_PATH, NAME_SPACE);
        }
        
        /// <summary>
        /// インプットの定数管理クラスを生成
        /// </summary>
        [MenuItem(COMMAND_INPUT)]
        private static void CreateInput()
        {
            var input_name_dic = new Dictionary<string, string>();

            var serialized_obj = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath(TARGET_DIRECTORY_NAME + INPUT_PATH)[0]);
            var axes_property = serialized_obj.FindProperty("m_Axes");

            // InputManager.Asset内のm_Name情報を取得する
            for (int i = 0; i < axes_property.arraySize; ++i)
            {
                var axis_property = axes_property.GetArrayElementAtIndex(i);

                var name = GetChildProperty(axis_property, "m_Name").stringValue;
                input_name_dic[name] = name;
            }
            ConstantsClassCreator.Create("InputName", "インプット名を定数管理するクラス", input_name_dic, DIRECTORY_PATH, NAME_SPACE);
        }


        [MenuItem(COMMAND_STAGE)]
        private static void CreateStageName()
        {
            var stage_dic = new Dictionary<string, string>();

            foreach(var stage_data in Resources.LoadAll(StageLoad.filepath))
            {
                stage_dic[stage_data.name] = stage_data.name;
            }

            //定数クラス作成
            ConstantsClassCreator.Create("StageDataName", "ステージデータを定数管理するクラス", stage_dic, DIRECTORY_PATH, NAME_SPACE);
        }

#endif

        //sortinglayerの名前一覧を取得
        private static string[] GetSortingLayerNames()
        {
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            return (string[])sortingLayersProperty.GetValue(null, new object[0]);
        }

        /// <summary>
        /// 入力されたassetsの中に、ディレクトリのパスがdirectoryNameの物はあるか
        /// </summary>
        private static bool ExistsDirectoryInAssets(List<string[]> assetsList, List<string> targetDirectoryNameList)
        {
            return assetsList
                .Any(assets => assets                                       //入力されたassetsListに以下の条件を満たすか要素が含まれているか判定
                 .Select(asset => System.IO.Path.GetDirectoryName(asset))   //assetsに含まれているファイルのディレクトリ名だけをリストにして取得
                 .Intersect(targetDirectoryNameList)                         //上記のリストと入力されたディレクトリ名のリストの一致している物のリストを取得
                 .Count() > 0);                                              //一致している物があるか
        }

        /// <summary>
	    /// 入力されたassetsの中に、指定したパスが含まれるものが一つでもあるか
        /// </summary>
        private static bool ExistsDirectoryInAssets(List<string[]> assets_list, string target_directory_name)
        {
            return assets_list
                .Any(assetPaths => assetPaths
                .Any(assetPath => assetPath
                .Contains(target_directory_name)));
        }

        private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
        {
            SerializedProperty child = parent.Copy();
            child.Next(true);
            do
            {
                if (child.name == name) return child;
            }
            while (child.Next(false));
            return null;
        }

        /// <summary>
        /// ファイル名を定数で管理するクラスを作成できるかどうかを取得します
        /// </summary>
        [MenuItem(COMMAND_ALL, true)]
        [MenuItem(COMMAND_LAYER, true)]
        [MenuItem(COMMAND_SCENE, true)]
        [MenuItem(COMMAND_SORTING_LAYERS, true)]
        [MenuItem(COMMAND_TAG, true)]
        [MenuItem(COMMAND_STAGE, true)]
        private static bool CanCreate()
        {
            return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
        }
    }
}

//#endif
