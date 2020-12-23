using FrontPerson.Data;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace FrontPerson.Manager
{
    public class DataManager
    {
        /// <summary>
        /// データの書き出し
        /// </summary>
        /// <param name="data"></param>
        public static void Save<T>(T data, string data_name)
        {
            // フォルダパス
            string FolderPath = null;

            // ファイルパス
#if UNITY_EDITOR
            //Editor上では普通にカレントディレクトリを確認
            FolderPath = Directory.GetCurrentDirectory();

            string export_path = FolderPath + "/Save/" + data_name + ".sav";
#else
            //EXEを実行したカレントディレクトリ (ショートカット等でカレントディレクトリが変わるのでこの方式で)
            FolderPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');

            string export_path = FolderPath + "/Save/" + data_name + ".sav";
#endif

            // バイナリ形式でシリアル化
            BinaryFormatter bf = new BinaryFormatter();

            // 書き出し先のディレクトリが無ければ作成
            string directory_name = Path.GetDirectoryName(export_path);
            if (!Directory.Exists(directory_name))
            {
                Directory.CreateDirectory(directory_name);
            }

            // 書き出し先のファイルがあるかチェック
            if (File.Exists(export_path))
            {
                // 同名ファイルの中身をチェック、全く同じだったら書き出さない
                StreamReader sr = new StreamReader(export_path, Encoding.UTF8);
                bool is_same = sr.ReadToEnd() == data.ToString();
                sr.Close();

                if (is_same)
                {
                    return;
                }
            }

            // 指定パスにファイルを作成
            FileStream file = File.Create(export_path);

            try
            {
                // 指定オブジェクトをシリアル化
                bf.Serialize(file, data);
            }
            finally
            {
                if(file != null)
                {
                    file.Close();
                }
            }
        }

        /// <summary>
        /// データの読み込み
        /// </summary>
        /// <param name="data"></param>
        /// <param name="data_name">拡張子不要</param>
        public static T Load<T>(string data_name)
        {
            T data;

            // フォルダパス
            string FolderPath = null;

            // ファイルパス
#if UNITY_EDITOR
            //Editor上では普通にカレントディレクトリを確認
            FolderPath = Directory.GetCurrentDirectory();

            string save_file_path = FolderPath + "/Save/" + data_name + ".sav";
#else
            //EXEを実行したカレントディレクトリ (ショートカット等でカレントディレクトリが変わるのでこの方式で)
            FolderPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');

            string save_file_path = FolderPath + "/Save/" + data_name + ".sav";
#endif

            // バイナリ形式でシリアル化
            BinaryFormatter bf = new BinaryFormatter();

            // 指定パスのファイルを開く
            FileStream file = File.Open(save_file_path, FileMode.Open);

            try
            {
                // 指定ファイルをオブジェクトにシリアライズ
                data = (T)bf.Deserialize(file);

                Debug.Log("読み込んだデータ" + data);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
            return data;
        }
    }
}