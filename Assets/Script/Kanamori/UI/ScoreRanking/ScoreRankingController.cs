using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    /// <summary>
    /// スコアランキングに表示するデータ
    /// </summary>
    [Serializable]
    public class RankingData
    {
        public Text score_value_;
        public Text combo_value_;
    }

    public class ScoreRankingController : MonoBehaviour
    {
        private readonly int RANKING_MAX = 5;

        [SerializeField]
        private RankingData[] ranking_data_ = new RankingData[5];

        // Start is called before the first frame update
        private void Start()
        {
            var manager = Manager.ApplicationManager.Instance;

            // セーブデータを取得して、ランキングの表示を更新
            for (int i = 0; i < RANKING_MAX; i++)
            {
                ranking_data_[i].score_value_.text = manager.save_data_.RankingScore[i].ToString();
                ranking_data_[i].combo_value_.text = manager.save_data_.RankingComboNum[i].ToString();
            }
        }
    }
}