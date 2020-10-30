using FrontPerson.common;
using FrontPerson.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace FrontPerson.Manager
{
    public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
    {
        /// <summary>
        /// 現在のスコア数
        /// </summary>
        public int CurrentScore
        {
            get;
            private set;
        } = 0;

        public UnityAction<int> on_add_score_;

        // コンボボーナスと時間ボーナスは仕様が分からないので、決まってから作ります。
        // コンボ数いくつからどれだけの倍率なのか？
        // 時間ボーナスについてはクリア時間なのか、それ以外なのか, 倍率はどうなのか
        /// <summary>
        /// コンボボーナス
        /// </summary>
        private int combo_bonus_ = 0;

        /// <summary>
        /// 時間ボーナス
        /// </summary>
        private float time_bonus_ = 0f;

        /// <summary>
        /// スコアを加算、減算
        /// </summary>
        /// <param name="score"></param>
        public void AddScore(int score)
        {
            CurrentScore += score;

            if(on_add_score_ != null)
            {
                on_add_score_.Invoke(score);
            }
        }
    }
}
