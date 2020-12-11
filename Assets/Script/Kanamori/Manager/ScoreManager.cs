using FrontPerson.common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace FrontPerson.Score
{
    public enum ADD_SCORE_TYPE
    {
        BASIC_SCORE = 0,
        BOUNTY_SCORE,
        COMBO_SCORE
    }
}
namespace FrontPerson.Manager
{
    public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
    {
        //スコア最大数
        const int SCORE_MAX = 99999999;
        const int SCORE_MIN = 0;

        /// <summary>
        /// 現在のスコア
        /// </summary>
        private int currentScore_ = 0;
        /// <summary>
        /// 現在のスコア
        /// </summary>
        public int CurrentScore {
            get 
            { 
                return currentScore_; 
            }
            private set 
            {
                currentScore_ = value;
                currentScore_ = Mathf.Min(SCORE_MAX, Mathf.Max(SCORE_MIN, currentScore_));
            } 
        }

        /// <summary>
        /// フィーバー中のスコア
        /// </summary>
        private int feverScore_ = 0;
        /// <summary>
        /// フィーバー中のスコア
        /// </summary>
        public int FeverScore {
            get
            {
                return feverScore_;
            }
            private set
            {
                feverScore_ = value;
                feverScore_ = Mathf.Min(SCORE_MAX, Mathf.Max(SCORE_MIN, feverScore_));
            }
        }

        /// <summary>
        /// 基本スコアが変化した際に処理を呼ぶ
        /// </summary>
        public UnityAction<int> on_add_score_;
        /// <summary>
        /// フィーバー中にスコアが変化したときに呼ぶ処理
        /// </summary>
        public UnityAction<int> on_add_fever_score_;

        /// <summary>
        /// フィーバー中かどうか
        /// </summary>
        public bool IsFever { get; private set; }
        /// <summary>
        /// フィーバータイム継続時間
        /// </summary>
        public float FeverEffectTime { get; private set; } = 15;
        /// <summary>
        /// フィーバー中のタイマー
        /// </summary>
        public float FeverTimer { get; private set; }
        /// <summary>
        /// フィーバー中のスコア倍率
        /// </summary>
        float scoreMagnification_ = 1.5f;
        /// <summary>
        /// フィーバータイムを発動する
        /// </summary>
        public void ActiveFeverTime(float feverTimeDuration, float scoreMagnification) => StartCoroutine(FeverTime(feverTimeDuration, scoreMagnification));

        /// <summary>
        /// フィーバータイムUI
        /// </summary>
        [Header("ふぃーばーたいむUI")]
        [SerializeField]
        UI.UI_FeverTime feverTimeUI_ = null;



        /// <summary>
        /// スコアを加算、減算
        /// </summary>
        /// <param name="score"></param>
        public void AddScore(int score, Score.ADD_SCORE_TYPE addScoreType)
        {
            switch (addScoreType)
            {
                case Score.ADD_SCORE_TYPE.BASIC_SCORE:
                    //フィーバー中は現在スコアに加算せずにフィーバー用のスコアに加算していく
                    if (IsFever)
                    {
                        AddFeverScore((int)(score * scoreMagnification_));
                    }
                    // フィーバー中でないなら普通に加算していく
                    else
                    {
                        AddBasicScore(score);
                    }
                    break;

                case Score.ADD_SCORE_TYPE.BOUNTY_SCORE:
                case Score.ADD_SCORE_TYPE.COMBO_SCORE:
                    AddBasicScore(score);
                    break;
            }
        }


        /// <summary>
        /// 基本スコアに加算
        /// </summary>
        /// <param name="score">加算するスコア</param>
        private void AddBasicScore(int score)
        {
            // スコアを更新
            CurrentScore += score;
            if(on_add_score_ != null)
                on_add_score_?.Invoke(score);
        }


        /// <summary>
        /// フィーバースコア加算
        /// </summary>
        /// <param name="score">加算するスコア</param>
        private void AddFeverScore(int score)
        {
            //フィーバースコアを更新
            FeverScore += score;
            if(on_add_fever_score_ != null)
                on_add_fever_score_?.Invoke(score);
        }


        /// <summary>
        /// フィーバータイム発動
        /// </summary>
        /// <returns></returns>
        private IEnumerator FeverTime(float feverTimeDuration, float scoreMagnification)
        {
            // フィーバー中かどうか
            if (FeverTimer <= 0)
            {
                // フィーバーの時間を設定する
                FeverEffectTime = feverTimeDuration;
                FeverTimer = feverTimeDuration;
                // スコア倍率
                scoreMagnification_ = scoreMagnification;
                // フィーバーUIを表示
                feverTimeUI_.FeverStart();
                // フラグを立てる
                IsFever = true;

                // フィーバーが終了するまで更新する
                while(0 < FeverTimer)
                {
                    yield return null;

                    FeverTimer -= Time.deltaTime;
                }

                // フィーバー中に取った基本スコアをトータルスコアに加算しフィーバーを終了する
                AddBasicScore(FeverScore);
                feverScore_ = 0;
                IsFever = false;
            }
            else
            {
                // フィーバー中なので時間を更新する
                FeverTimer = feverTimeDuration;
            }
        }
    }
}
