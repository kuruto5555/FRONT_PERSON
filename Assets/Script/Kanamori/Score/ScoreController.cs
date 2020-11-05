using FrontPerson.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FrontPerson.State
{
    public class ScoreController : MonoBehaviour
    {
        private ScoreManager score_manager_;

        [Header("スコアを表示するためのUIのテキスト")]
        [SerializeField]
        private Text score_text_ = null;

        private void Start()
        {
            score_manager_ = ScoreManager.Instance;
            score_manager_.on_add_score_ += ChangedScore;
        }

        /// <summary>
        /// スコアが変わった際に呼ばれる
        /// </summary>
        /// <param name="score"></param>
        private void ChangedScore(int score)
        {
            if(score_text_ != null)
            {
                StartCoroutine(ScoreAnimation(score_manager_.CurrentScore - score, score_manager_.CurrentScore, 1f));
            }
        }

        /// <summary>
        /// スコアアニメーション
        /// </summary>
        /// <param name="before_score">加算前のスコア</param>
        /// <param name="after_score">加算後のスコア</param>
        /// <param name="duration">アニメーションの時間</param>
        /// <returns></returns>
        private IEnumerator ScoreAnimation(int before_score, int after_score, float duration)
        {
            // 開始時間
            float start_time = Time.time;

            // 終了時間
            float end_time = start_time + duration;

            while(Time.time < end_time)
            {
                // 現在の時間の割合
                float time_rate = (Time.time - start_time) / duration;

                // スコアを更新
                int update_score_value = (int)((after_score - before_score) * time_rate + before_score);

                score_text_.text = update_score_value.ToString();

                yield return null;
            }

            score_text_.text = after_score.ToString();
        }
    }
}
