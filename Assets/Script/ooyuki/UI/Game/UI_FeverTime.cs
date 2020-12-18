using FrontPerson.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    public class UI_FeverTime : MonoBehaviour
    {
        [SerializeField]
        Text scoreText_ = null;

        [SerializeField]
        Outline scoreOutLine_ = null;

        [SerializeField]
        RectTransform gauge_ = null;

        [SerializeField]
        List<Color> gameingColor_ = null;

        [SerializeField]
        List<Color> outLineColor_ = null;

        [SerializeField, Range(0.1f, 2.0f)]
        float changeTimeLate_ = 0.5f;

        float changeTime_ =0.0f;

        int changeColorIndex_ = 0;

        ScoreManager scoreManager_ = null;

        // Start is called before the first frame update
        void Start()
        {
            scoreManager_ = ScoreManager.Instance;
            scoreManager_.on_add_fever_score_ += StartScoreAnimation;
            scoreText_.text = "+0";
            gauge_.anchorMax = new Vector2(1.0f, gauge_.anchorMax.y);
            gameObject.SetActive(false);
        }


        // Update is called once per frame
        void Update()
        {
            //スコアマネージャーからフィーバー中のスコアをもらう
            //FeverScoreTextUpdate(scoreManager_.FeverScore);
            GaugeUpdate();

            changeTime_ += Time.deltaTime;
            if(changeTimeLate_ <= changeTime_)
            {
                scoreText_.color = gameingColor_[changeColorIndex_];
                scoreOutLine_.effectColor = outLineColor_[changeColorIndex_];
                changeColorIndex_++;
                changeColorIndex_ %= gameingColor_.Count;
                changeTime_ = 0f;
            }
        }


        /// <summary>
        /// フィーバーが開始するときに呼んでほしい
        /// </summary>
        public void FeverStart()
        {
            FeverScoreTextUpdate(scoreManager_.FeverScore);
            GaugeUpdate();
        }


        /// <summary>
        /// フィーバースコアテキストの更新
        /// </summary>
        /// <param name="score"></param>
        private void FeverScoreTextUpdate(int score)
        {
            scoreText_.text = "+" + score.ToString();
        }

            
        /// <summary>
        /// フィーバーゲージの更新
        /// </summary>
        private void GaugeUpdate()
        {
            gauge_.anchorMax = new Vector2((scoreManager_.FeverTimer / scoreManager_.FeverEffectTime), gauge_.anchorMax.y);
        }


        /// <summary>
        /// スコア増減のアニメーションを開始する
        /// </summary>
        /// <param name="score"></param>
        private void StartScoreAnimation(int score)
        {
            StartCoroutine(ScoreAnimation(scoreManager_.FeverScore - score, scoreManager_.FeverScore, 1f));
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

            while (Time.time < end_time)
            {
                // 現在の時間の割合
                float time_rate = (Time.time - start_time) / duration;

                // スコアを更新
                int update_score_value = (int)((after_score - before_score) * time_rate + before_score);

                FeverScoreTextUpdate(update_score_value);

                yield return null;
            }

            FeverScoreTextUpdate(after_score);
        }
    }
}
