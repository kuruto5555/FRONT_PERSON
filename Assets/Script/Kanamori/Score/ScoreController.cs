using FrontPerson.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FrontPerson.Score
{
    /// <summary>
    /// スコアが変動した際のスコアの動作を操作する
    /// </summary>
    public class ScoreController : MonoBehaviour
    {
        private ScoreManager score_manager_;

        [Header("スコアを表示するためのUIのテキスト")]
        [SerializeField]
        private Text score_text_ = null;

        [Header("加算、減算分のスコアを表示するプレハブ")]
        [SerializeField]
        private GameObject add_score_text_prefab_ = null;

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
                // 増加、加算のスコアを表示するオブジェクトを生成
                AddScoreMotion motion = GameObject.Instantiate(add_score_text_prefab_, score_text_.transform.parent).GetComponent<AddScoreMotion>();

                if(motion != null)
                {
                    // 増減をテキストで表示する
                    Text score_text = motion.GetComponent<Text>();

                    // スコアに符号を付けてわかりやすくする
                    if (0 <= score)
                    {
                        score_text.text = "+" + score.ToString();
                    }
                    else
                    {
                        score_text.text = "-" + score.ToString();
                    }

                    // 増減のスコアの動作が終わったらスコアの
                    motion.end_motion_ += StartScoreAnimation;
                }
                
            }
        }

        /// <summary>
        /// スコア増減のアニメーションを開始する
        /// </summary>
        /// <param name="score"></param>
        private void StartScoreAnimation(int score)
        {
            StartCoroutine(ScoreAnimation(score_manager_.CurrentScore - score, score_manager_.CurrentScore, 1f));
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

                score_text_.text = update_score_value.ToString("00000000");

                yield return null;
            }

            score_text_.text = after_score.ToString("00000000");
        }
    }
}
