using FrontPerson.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.UI;
using FrontPerson.Constants;

namespace FrontPerson.Data
{
    /// <summary>
    /// 保存するデータ情報
    /// </summary>
    [System.Serializable]
    public class SaveDatas
    {
        static public readonly string SAVE_DATA_NAME = "SaveData";

        public SoundVolumeData SoundData;

        // サウンド設定（リリースした後もリソースフォルダから取得できるか分からないので後で確認）
        //public SoundManagerSetting SoundSetting;

        /// <summary>
        /// 初回起動かどうかを判断するフラグ(初回起動 = true)
        /// /// </summary>
        public bool FirstBoot = true;

        /// <summary>
        /// ランキングに表示するスコア(上位5位分)
        /// </summary>
        public List<int> RankingScore = new List<int>() { 0, 0, 0, 0, 0 };
    }
}