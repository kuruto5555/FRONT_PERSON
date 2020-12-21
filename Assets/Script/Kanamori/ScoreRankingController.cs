using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrontPerson.Score
{
    public class ScoreRanking
    {
        [SerializeField]
        public Text score_text_ = null;

        /// <summary>
        /// スコア数
        /// </summary>
        public float score_;
        /// <summary>
        /// プレイヤーネーム
        /// </summary>
        public float player_name_;
    }

    public class ScoreRankingController : MonoBehaviour
    {
        private ScoreRanking[] ranking_ = new ScoreRanking[5];

        // Start is called before the first frame update
        private void Start()
        {

        }
    }
}