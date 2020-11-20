using FrontPerson.common;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace FrontPerson.Score
{
    public enum ReasonForAddition
    {
        Nomal,
        Bounty,

    }
}
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

        [Tooltip("コンボボーナスの効果時間")]
        [SerializeField]
        private float combo_bonus_effect_time_ = 5f;

        [Tooltip("フィーバータイムの効果時間")]
        [SerializeField]
        private float fever_effect_time_ = 15f;

        [SerializeField]
        private BountyManager bounty_manager_ = null;

        /// <summary>
        /// コンボボーナスの制限時間
        /// </summary>
        public float ComboBonusTimer { get; private set; } = 0f;

        /// <summary>
        /// コンボボーナス
        /// </summary>
        public int ComboBonus { get; private set; } = 0;

        /// <summary>
        /// コンボ保険中かどうか
        /// </summary>
        public bool IsComboInsurance { get; private set; } = false;
        /// <summary>
        /// コンボ保険を発動する
        /// </summary>
        public void ActiveComboInsurance() => IsComboInsurance = true;

        /// <summary>
        /// フィーバー中かどうか
        /// </summary>
        public bool IsFever { get; private set; }
        /// <summary>
        /// フィーバー中のタイマー
        /// </summary>
        public float FeverTimer { get; private set; }
        /// <summary>
        /// フィーバータイムを発動する
        /// </summary>
        public void ActiveFeverTime() => StartCoroutine(FeverTime());

        private void Update()
        {
            if(bounty_manager_ != null)
            {
                // バウンティマネージャーに現在のコンボ数を伝える
                bounty_manager_.SetNowCombo(ComboBonus);
            }
        }

        /// <summary>
        /// スコアを加算、減算
        /// </summary>
        /// <param name="score"></param>
        public void AddScore(int score, Score.ReasonForAddition reason)
        {
            int add_score = score;

            switch (reason)
            {
                // 通常の加算
                case Score.ReasonForAddition.Nomal:
                    {
                        // 加算スコアにコンボボーナスを追加する
                        add_score += BonusInTheMiddleOfTheCombo();

                        // コンボを1増やす
                        AddComboBonus(1);
                        break;
                    }

                // バウンティによる加算
                case Score.ReasonForAddition.Bounty:
                    {
                        break;
                    }
            }

            // スコアを更新
            CurrentScore += add_score;

            if (on_add_score_ != null)
            {
                on_add_score_.Invoke(add_score);
            }

            // コンボ持続タイマー開始
            StartCoroutine(TimerDuringComboBonus());
        }

        public void AddComboBonus(int combo)
        {
            ComboBonus += combo;
        }

        /// <summary>
        /// コンボボーナスが途切れた
        /// </summary>
        public void LostComboBonus()
        {
            // コンボボーナスが無かったら返す
            if(ComboBonus == 0)
            {
                return;
            }

            // コンボ保険中だったら返す
            if (ComboInsuranceIsInEffect())
            {
                return;
            }

            // 途切れた際のコンボ数でボーナススコア加算
//            ComboBreakBonus();

            ComboBonus = 0;
        }

        /// <summary>
        /// コンボボーナス中のタイマー
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator TimerDuringComboBonus()
        {
            // タイマーが動いているかどうか
            if (ComboBonusTimer <= 0)
            {
                // コンボの制限時間を設定
                SetComboBonusTimer();

                // コンボが続かず制限時間がきれたら抜ける
                while (0 < ComboBonusTimer)
                {
                    yield return null;

                    // 毎フレームタイマーを減らす
                    ComboBonusTimer -= Time.deltaTime;
                }

                // コンボ保険発動中か確認
                if (ComboInsuranceIsInEffect())
                {
                    // タイマー再設定
                    SetComboBonusTimer();
                    // もう一度
                    StartCoroutine(TimerDuringComboBonus());
                }
                else
                {
                    // コンボが続かなかったのでコンボボーナスを消す
                    LostComboBonus();
                    ComboBonusTimer = 0;
                }
            }
            else
            {
                // タイマーが動いているので制限時間を戻す
                SetComboBonusTimer();
            }
        }

        /// <summary>
        /// コンボボーナスの時間を設定
        /// </summary>
        public void SetComboBonusTimer()
        {
            ComboBonusTimer = combo_bonus_effect_time_;
        }

        /// <summary>
        /// コンボ途中ボーナス
        /// </summary>
        /// <returns></returns>
        private int BonusInTheMiddleOfTheCombo()
        {
            if(ComboBonus == 0)
            {
                return 0;
            }

            // 計算式：(100 ÷ 前回のコンボ途中ボーナスからかかった時間) × コンボ数
            return (int)(100 / (combo_bonus_effect_time_ - ComboBonusTimer) * ComboBonus);
        }

        /// <summary>
        /// コンボ途切れボーナス
        /// </summary>
        /// <returns></returns>
        //        private int ComboBreakBonus()
        //        {
        //            return 50 * combo_bonus_ * ((2 - 一般市民の出現確率) * コンボ中に健康にした一般市民の数)
        //                * ((2 - ヤクザの出現確率) * コンボ中に健康にしたヤクザの数)
        //                * ((2 - おばちゃんの出現確率) * コンボ中に健康にしたおばちゃんの数);
        //        }


        /// <summary>
        /// コンボ保険発動中かどうか
        /// </summary>
        /// <returns></returns>
        private bool ComboInsuranceIsInEffect()
        {
            // 保険発動中は保険を使ってコンボを持続
            if (IsComboInsurance)
            {
                IsComboInsurance = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// フィーバータイム発動
        /// </summary>
        /// <returns></returns>
        private IEnumerator FeverTime()
        {
            // フィーバー中かどうか
            if (FeverTimer <= 0)
            {
                // フィーバーの時間を設定する
                FeverTimer = fever_effect_time_;

                // フィーバーが終了するまで更新する
                while(0 < FeverTimer)
                {
                    yield return null;

                    FeverTimer -= Time.deltaTime;
                }
                // フィーバーを終了する
                IsFever = false;

            }
            else
            {
                // フィーバー中なので時間を更新する
                FeverTimer = fever_effect_time_;
            }
        }

    }
}
