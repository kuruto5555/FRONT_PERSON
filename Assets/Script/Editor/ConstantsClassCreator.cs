#if UNITY_EDITOR

namespace StealthPackMan.Constants
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    /// <summary>
    /// 定数管理するクラスを生成するクラス
    /// </summary>
    public static class ConstantsClassCreator
    {
        private const string STRING_NAME = "string";
        private const string INT_NAME = "int";
        private const string FLOAT_NAME = "float";

        private const string SCRIPT_EXTENSION = ".cs";

        public const string PATH = "Assets/Script/Functions/constants";

        /// <summary>
        /// 定数管理するクラスを自動生成
        /// </summary>
        public static void Create<T>(string class_name, string summary, Dictionary<string, T> value_dict, string export_dictionary_path, string name_space = "")
        {
            // 入力された型の判定
            string type_name = null;

            if (typeof(T) == typeof(string))
            {
                type_name = STRING_NAME;
            }
            else if (typeof(T) == typeof(int))
            {
                type_name = INT_NAME;
            }
            else if (typeof(T) == typeof(float))
            {
                type_name = FLOAT_NAME;
            }
            else
            {
                Debug.Log(class_name + SCRIPT_EXTENSION + "の作成に失敗しました。想定外の型" + typeof(T).Name + "が入力されました。");
                return;
            }

            // ディクショナリーをソートしたもの
            SortedDictionary<string, T> sort_dic = new SortedDictionary<string, T>(value_dict);

            // 入力された辞書のkeyから無効な文字列を削除して、大文字に_を設定した体数名と同じものに変更し、新たな辞書に登録
            Dictionary<string, T> new_value_dict = new Dictionary<string, T>();

            foreach (var item in sort_dic)
            {
                string new_key = RemoveInvalidChars(item.Key);
                new_key = SetDelimiterBeforeUppercase(new_key);
                new_value_dict[new_key] = item.Value;
            }

            // 定数名の最大長を取得し、空白数を決定
            int key_length_max = 0;
            if (0 < new_value_dict.Count)
            {
                key_length_max = 1 + new_value_dict.Keys.Select(key => key.Length).Max();
            }

            StringBuilder builder = new StringBuilder();

            // 生成するスクリプト全文
            {
                // 名前空間があれば入力
                if (!string.IsNullOrEmpty(name_space))
                {
                    builder.AppendLine("namespace " + name_space + "\n{");
                }

                // コメント分とクラス名を入力
                builder.AppendLine("/// <summary>");
                builder.AppendFormat("/// {0}", summary).AppendLine();
                builder.AppendLine("/// </summary>");
                builder.AppendFormat("public static class {0}", class_name).AppendLine("\n{").AppendLine();

                // 入力された定数とその値のペアを書き出していく
                string[] key_array = new_value_dict.Keys.ToArray();
                foreach (var key in key_array)
                {
                    if (string.IsNullOrEmpty(key))
                    {
                        continue;
                    }
                    // 数字だけのkeyだったらスルー
                    else if (System.Text.RegularExpressions.Regex.IsMatch(key, @"^[0-9]+$"))
                    {
                        continue;
                    }
                    // keyに反英数字と_以外が含まれていたらスルー
                    else if (!System.Text.RegularExpressions.Regex.IsMatch(key, @"^[_a-zA-Z0-9]+$"))
                    {
                        continue;
                    }

                    // イコールが並ぶように空白を調整
                    string equal = String.Format("{0, " + (key_length_max - key.Length).ToString() + "}", "=");

                    // 上記で判定した型と定数名を入力
                    builder.Append("\t").AppendFormat(@"public const {0} {1} {2}", type_name, key, equal);

                    // 値を入力
                    // Tがstringの場合は値の前後に"をつける
                    if (type_name == STRING_NAME)
                    {
                        builder.AppendFormat(@"""{0}"";", new_value_dict[key]).AppendLine();
                    }
                    else
                    {
                        builder.AppendFormat(@"{0};", new_value_dict[key]).AppendLine();
                    }
                }

                builder.AppendLine().AppendLine("}");

                // 名前空間があれば最後にカッコ追加
                if (!string.IsNullOrEmpty(name_space))
                {
                    builder.AppendLine().AppendLine("}");
                }
            }// スクリプト全文の書き出し完了


            // 書き出し、ファイル名はクラス名.cs
            string export_path = Path.Combine(export_dictionary_path, class_name + SCRIPT_EXTENSION);
            string export_text = builder.ToString();

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
                bool is_same = sr.ReadToEnd() == export_text;
                sr.Close();

                if (is_same)
                {
                    return;
                }
            }

            // 書き出し
            File.WriteAllText(export_path, export_text, Encoding.UTF8);
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

            Debug.Log(class_name + SCRIPT_EXTENSION + "の作成が完了しました");
        }

        //無効な文字を管理する配列
        private static readonly string[] INVALID_CHARS = {
            " ", "!", "\"", "#", "$",
            "%", "&", "\'", "(", ")",
            "-", "=", "^",  "~", "\\",
            "|", "[", "{",  "@", "`",
            "]", "}", ":",  "*", ";",
            "+", "/", "?",  ".", ">",
            ",", "<"
       };
        
        /// <summary>
        /// 無効な文字列を削除します
        /// </summary>
        private static string RemoveInvalidChars(string str)
        {
            Array.ForEach(INVALID_CHARS, c => str.Replace(c, string.Empty));
            return str;
        }

        //定数の区切り文字
        private const char DELIMITER = '_';

        /// <summary>
        /// 区切り文字を大文字の前に設定する
        /// </summary>
        private static string SetDelimiterBeforeUppercase(string str)
        {
            string conversion_str = "";

            for(int num = 0; num < str.Length; num++)
            {
                bool is_set_delimiter = true;

                // 最初は設定しない
                if(num == 0)
                {
                    is_set_delimiter = false;
                }
                // 小文字か数字なら設定しない
                else if(char.IsLower(str[num]) || char.IsNumber(str[num]))
                {
                    is_set_delimiter = false;
                }
                // 判定する前が大文字なら設定しない（連続大文字の時）
                else if(char.IsUpper(str[num -1]) && !char.IsNumber(str[num]))
                {
                    is_set_delimiter = false;
                }
                // 判定してる文字かその文字の前が区切り文字なら設定しない
                else if(str[num] == DELIMITER || str[num -1] == DELIMITER)
                {
                    is_set_delimiter = false;
                }

                // 文字設定
                if(is_set_delimiter)
                {
                    conversion_str += DELIMITER.ToString();
                }

                conversion_str += str.ToUpper()[num];
            }

            return conversion_str;
        }
    }
}

#endif