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
        Nomal = 0,
        OrdinaryPeople,
        OldBattleaxe,
        Yakuza,
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
        public int CurrentScore { get; private set; } = 0;

        /// <summary>
        /// スコアが変化した際に処理を呼ぶ
        /// </summary>
        public UnityAction<int> on_add_score_;

        [Tooltip("コンボボーナスの効果時間")]
        [SerializeField]
        private float combo_bonus_effect_time_ = 5f;
        public float ComboBonusEffectTime { get { return combo_bonus_effect_time_; } }

        [Tooltip("フィーバータイムの効果時間")]
        [SerializeField]
        private float fever_effect_time_ = 15f;
        public float FeverEffectTime { get { return fever_effect_time_; } }

        [SerializeField]
        private BountyManager bounty_manager_ = null;

        /// <summary>
        /// コンボボーナスの制限時間
        /// </summary>
        public float ComboBonusTimer { get; private set; } = 0f;

        /// <summary>
        /// コンボ数
        /// </summary>
        public int ComboNum { get; private set; } = 0;

        /// <summary>
        /// コンボ中にもらったコンボ途中ボーナスの数
        /// </summary>
        int bonusInTheMiddleOfTheComboCounter = 1;

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

        /// <summary>
        /// エネミースポナー
        /// </summary>
        Enemy.Spawner spawner_ = null;

        /// <summary>
        /// コンボ中に健康にした一般人の数
        /// </summary>
        int ordinaryPeopleNum = 0;
        /// <summary>
        /// コンボ中に健康にしたおばちゃんの数
        /// </summary>
        int oldBattleaxeNum = 0;
        /// <summary>
        /// コンボ中に撃退した数
        /// </summary>
        int yakuzaNum = 0;

        /// <summary>
        /// １０コンボするのにかかった時間
        /// </summary>
        float comboTennTime_ = 0f;

        private void Start()
        {
            //エネミーの生成率は共通なのでシーン上のどれか一つもらえればいい
            spawner_ = FindObjectOfType<Enemy.Spawner>();
        }


        private void Update()
        {
            if(bounty_manager_ != null)
            {
                // バウンティマネージャーに現在のコンボ数を伝える
                bounty_manager_.SetNowCombo(ComboNum);
            }

            if(ComboNum > 0)
            {
                comboTennTime_ += Time.deltaTime;
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
                        // コンボを1増やす
                        AddComboBonus(1);

                        // 加算スコアにコンボボーナスを追加する
                        add_score += BonusInTheMiddleOfTheCombo();
                        break;
                    }

                case Score.ReasonForAddition.OrdinaryPeople:
                    {
                        //元気にした一般市民の数をカウントアップ
                        ordinaryPeopleNum++;

                        // コンボを1増やす
                        AddComboBonus(1);

                        // 加算スコアにコンボボーナスを追加する
                        add_score += BonusInTheMiddleOfTheCombo();
                        break;
                    }

                case Score.ReasonForAddition.OldBattleaxe:
                    {
                        //元気にしたおばちゃんの数をカウントアップ
                        oldBattleaxeNum++;

                        // コンボを1増やす
                        AddComboBonus(1);

                        // 加算スコアにコンボボーナスを追加する
                        add_score += BonusInTheMiddleOfTheCombo();
                        break;
                    }

                case Score.ReasonForAddition.Yakuza:
                    {
                        //元気にしたヤクザの数をカウントアップ
                        yakuzaNum++;

                        // コンボを1増やす
                        AddComboBonus(1);

                        // 加算スコアにコンボボーナスを追加する
                        add_score += BonusInTheMiddleOfTheCombo();
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

        /// <summary>
        /// コンボ数の加算
        /// </summary>
        /// <param name="combo"></param>
        public void AddComboBonus(int combo)
        {
            ComboNum += combo;
        }


        /// <summary>
        /// コンボボーナスが途切れた
        /// </summary>
        public void LostComboBonus()
        {
            // コンボボーナスが無かったら返す
            if(ComboNum == 0)
            {
                return;
            }

            // コンボ保険中だったら返す
            if (ComboInsuranceIsInEffect())
            {
                return;
            }

            // 途切れた際のコンボ数でボーナススコア加算
            int addScore = ComboBreakBonus();
            CurrentScore += addScore;
            if (on_add_score_ != null)
            {
                on_add_score_.Invoke(addScore);
            }


            // コンボボーナスにかかわる数値を初期化
            bonusInTheMiddleOfTheComboCounter = 1;
            ordinaryPeopleNum = 0;
            oldBattleaxeNum = 0;
            yakuzaNum = 0;


            ComboNum = 0;
        }


        /// <summary>
        /// コンボボーナスの時間を設定
        /// </summary>
        public void SetComboBonusTimer()
        {
            ComboBonusTimer = combo_bonus_effect_time_;
        }


        /// <summary>
        /// コンボボーナスを開始
        /// </summary>
        public void StartComboBonus()
        {
            StartCoroutine(TimerDuringComboBonus());
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
        /// コンボ途中ボーナス
        /// </summary>
        /// <returns></returns>
        private int BonusInTheMiddleOfTheCombo()
        {
            //コンボ数が0じゃない
            //または、コンボ制限時間が0じゃない
            if(ComboNum == 0 || combo_bonus_effect_time_ == 0)
            {
                return 0;
            }

            // コンボ数が十の倍数だったら
            if(!(ComboNum == (bonusInTheMiddleOfTheComboCounter * 10)))
            {
                return 0;
            }

            // コンボ途中ボーナス獲得！
            // 計算式：(100 ÷ 前回のコンボ途中ボーナスからかかった時間) × コンボ数
            var bonus = (int)(100f / (1f + comboTennTime_) * ComboNum);
            // カウントを一つ進める
            bonusInTheMiddleOfTheComboCounter++;
            // １０コンボにかかった時間を初期化
            comboTennTime_ = 0f;

            return bonus;
        }


        /// <summary>
        /// コンボ途切れボーナス
        /// </summary>
        /// <returns></returns>
        private int ComboBreakBonus()
        {
            return (int)(50 * ComboNum
                * (1f + ((1f - spawner_.ProbabilityOrdinaryPeople) * ordinaryPeopleNum))
                * (1f + ((1f - spawner_.ProbabilityOldBattleaxe)   * oldBattleaxeNum))
                * (1f + ((1f - spawner_.ProbabilityYakuza)         * yakuzaNum))
                );


            //return 50 * combo_bonus_ * ((2 - 一般市民の出現確率) * コンボ中に健康にした一般市民の数)
            //    * ((2 - ヤクザの出現確率) * コンボ中に健康にしたヤクザの数)
            //    * ((2 - おばちゃんの出現確率) * コンボ中に健康にしたおばちゃんの数);
        }


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
