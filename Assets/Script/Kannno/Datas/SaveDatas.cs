﻿using FrontPerson.Manager;
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

        /// <summary>
        /// サウンド設定
        /// </summary>
        public AudioVolume SoundData;

        /// <summary>
        /// 初回起動かどうかを判断するフラグ(初回起動 = true)
        /// /// </summary>
        public bool FirstBoot = true;

        /// <summary>
        /// ランキングに表示するスコア(上位5位分)
        /// </summary>
        public List<int> RankingScore = new List<int>() { 100000, 50000, 20000, 10000, 5000 };

        /// <summary>
        /// ランキングに表示する最大コンボ数(上位5位分)
        /// </summary>
        public List<int> RankingComboNum = new List<int>() { 200, 150, 100, 50, 10 };

        /// <summary>
        /// ランキングに表示するミッションクリア数(上位5位分)
        /// </summary>
        public List<int> RankingClearMissionNum = new List<int>() { 30, 20, 15, 10, 5 };

        /// <summary>
        /// 上下の視点感度
        /// </summary>
        public int VerticalRotationSpeed = 5;

        /// <summary>
        /// 上下の視点感度
        /// </summary>
        public int HorizontalRotationSpeed = 5;
    }
}