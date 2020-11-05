using FrontPerson.common;
using FrontPerson.State;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
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

        /// <summary>
        /// スコアが変化した際に処理を呼ぶ
        /// </summary>
        public UnityAction<int> on_add_score_;

        [Tooltip("コンボボーナスの制限時間の設定値")]
        [SerializeField]
        private float combo_bonus_time_limit_ = 0f;

        private float combo_bonus_timer_ = 0f;
        /// <summary>
        /// コンボボーナスの制限時間
        /// </summary>
        public float ComboBonusTimer
        {
            get { return combo_bonus_timer_; }
        }

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
            // スコアを更新
            CurrentScore += score;

            if(on_add_score_ != null)
            {
                on_add_score_.Invoke(score);

                AddComboBonus();

                // タイマーが動いているかどうか
                if(combo_bonus_timer_ <= 0)
                {
                    // タイマーが動いていないのでコルーチンを開始
                    StartCoroutine(TimerDuringComboBonus());
                }
                else
                {
                    // タイマーが動いているので制限時間を再度設定
                    combo_bonus_timer_ = combo_bonus_time_limit_;
                }

            }
        }

        public void AddComboBonus()
        {
            combo_bonus_++;
        }

        /// <summary>
        /// コンボボーナスが途切れた
        /// </summary>
        public void LostComboBonus()
        {
            // コンボボーナスが無かったら返す
            if(combo_bonus_ == 0)
            {
                return;
            }

            combo_bonus_ = 0;

            // 途切れた際のコンボ数でボーナススコア加算
            
        }

        /// <summary>
        /// コンボボーナス中のタイマー
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator TimerDuringComboBonus()
        {
            // コンボの制限時間を設定
            combo_bonus_timer_ = combo_bonus_time_limit_;

            // コンボが続かず制限時間がきれたら抜ける
            while (0 < combo_bonus_timer_)
            {
                yield return null;

                // 毎フレームタイマーを減らす
                combo_bonus_timer_ -= Time.deltaTime;

                // コンボ中の時間ボーナスを入れておく
                time_bonus_ += Time.deltaTime;
            }

            // コンボが続かなかったのでコンボボーナスを消す
            LostComboBonus();
        }
    }
}
