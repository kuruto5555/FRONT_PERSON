using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.common;
using FrontPerson.Enemy;
using FrontPerson.Constants;
using FrontPerson.UI;

namespace FrontPerson.Manager
{
    public enum ADD_COMBO_TYPE
    {
        ORDINATY_PEOPLE,
        OLD_BATTLEAXE,
        YAKUZA,
        ITEM,
    }

    public class ComboManager : SingletonMonoBehaviour<ComboManager>
    {
        //スコア最大最小
        const int COMBO_MAX = 999;
        const int COMBO_MIN = 0;

        /// <summary>
        /// コンボ数
        /// </summary>
        private int comboNum_ = 0;
        /// <summary>
        /// コンボ数　最大999　最小0
        /// </summary>
        public int ComboNum
        {
            get { return comboNum_; }
            private set
            {
                comboNum_ = Mathf.Min(COMBO_MAX, Mathf.Max(COMBO_MIN, value));
            }
        }
        /// <summary>
        /// 最大到達コンボ数
        /// </summary>
        public int ComboNumMax { get; private set; } = 0;

        /// <summary>
        /// コンボ中かどうか
        /// true -> コンボ中
        /// false -> コンボ中じゃない
        /// </summary>
        public bool IsCombo { get; private set; }

        /// <summary>
        /// コンボの継続時間
        /// </summary>
        [Header("コンボの継続時間")]
        [SerializeField, Range(1f, 20f)]
        float comboDuration_ = 5f;
        public float ComboDuration { get { return comboDuration_; } }
        /// <summary>
        /// コンボ残り時間
        /// </summary>
        public float ComboRemainingTime { get; private set; } = 0f;

        /// <summary>
        /// コンボ保険中かどうか
        /// </summary>
        public bool IsComboInsurance { get; private set; } = false;
        /// <summary>
        /// コンボ保険を発動する
        /// </summary>
        public void ActiveComboInsurance() => IsComboInsurance = true;

        /// <summary>
        /// コンボ中に健康にした一般人の数
        /// </summary>
        int ordinaryPeopleNum_ = 0;
        /// <summary>
        /// コンボ中に健康にしたおばちゃんの数
        /// </summary>
        int oldBattleaxeNum_ = 0;
        /// <summary>
        /// コンボ中に撃退した数
        /// </summary>
        int yakuzaNum_ = 0;

        [Header("コンボ途中ボーナスがもらえるコンボ間隔")]
        [SerializeField, Range(1, 50)]
        int bonusIntercal_ = 10;
        /// <summary>
        /// コンボ途中ボーナス用のカウント
        /// </summary>
        int comboMidwayBonusCount_ = 0;
        /// <summary>
        /// 前回のコンボ途中ボーナスからかかった時間
        /// </summary>
        float comboMidwayBonusTime_ = 0f;

        /// <summary>
        /// スコアマネージャー
        /// </summary>
        ScoreManager scoreManager_ = null;
        /// <summary>
        /// バウンティマネージャー
        /// </summary>
        BountyManager bountyManager_ = null;
        /// <summary>
        /// エネミーのスポナーの一つ
        /// </summary>
        Spawner spawner_ = null;

        [Header("コンボUI")]
        [SerializeField]
        UI_Combo comboUI_ = null;

        /// <summary>
        /// アニメーター
        /// </summary>
        Animator animator_ = null;

        /// <summary>
        /// ポップアップアニメーションのハッシュ
        /// </summary>
        readonly int popUpAnimHash = Animator.StringToHash("Combo");


        // Start is called before the first frame update
        void Start()
        {
            IsCombo = false;
            // コンボボーナス加算用にスコアマネージャー受け取っておく
            scoreManager_ = ScoreManager.Instance;
            // 生成率はどれも一緒なのでどれか一つを取ればいい
            spawner_ = FindObjectOfType<Spawner>();
            // バウンティーマネージャー
            bountyManager_ = BountyManager._instance;
            bountyManager_.SetNowCombo(comboNum_);
            //アニメーターの取得
            animator_ = GetComponent<Animator>();
        }


        /// <summary>
        /// コンボ加算
        /// </summary>
        /// <param name="addComboNum">加算コンボ数　敵なら1、アイテムならアイテムが持ってる変数分</param>
        /// <param name="enemyType">エネミーの種類</param>
        /// <returns>コンボ途中ボーナスがあればボーナススコアを返す</returns>
        public void AddCombo(int addComboNum, ADD_COMBO_TYPE enemyType)
        {
            // コンボ加算の種類が敵ならその敵の数を加算する
            switch (enemyType)
            {
                case ADD_COMBO_TYPE.ORDINATY_PEOPLE:
                    ordinaryPeopleNum_++;
                    break;

                case ADD_COMBO_TYPE.OLD_BATTLEAXE:
                    oldBattleaxeNum_++;
                    break;

                case ADD_COMBO_TYPE.YAKUZA:
                    yakuzaNum_++;
                    break;

                case ADD_COMBO_TYPE.ITEM:
                    break;

                default:
                    Debug.LogError("予期せぬ値です。");
                    break;
            }


            // コンボを加算
            ComboNum += addComboNum;
            comboMidwayBonusCount_ += addComboNum;

            // バウンティーに現在のコンボ数を教える
            bountyManager_.SetNowCombo(comboNum_);

            // コンボが最大到達コンボを超えたかどうか
            if (ComboNum > ComboNumMax) ComboNumMax = ComboNum;
            
            // コンボ時間の更新
            SetComboBonusTimer();

            // UIにコンボ情報を伝える
            comboUI_.SetComboNum();
            
            // アニメーション再生
            animator_.Play(popUpAnimHash);

            // コルーチンでコンボのタイマーを開始
            // すでに動いていた場合特に何も起きない
            StartCoroutine(UpdateComboTimer());

            // コンボボーナスがあればスコアに加算
            ComboMidwayBonus();

        }


        public void FinishGame()
        {
            IsCombo = false;

            // コンボ途切れボーナス加算
            LostComboBonus();

            // コンボ中に倒した敵の数を初期化
            ordinaryPeopleNum_ = 0;
            oldBattleaxeNum_ = 0;
            yakuzaNum_ = 0;

            // コンボ数を初期化
            ComboNum = 0;

            // コンボ残り時間を初期化
            ComboRemainingTime = 0f;

            // 途中コンボボーナスの用の変数を初期化
            comboMidwayBonusTime_ = 0f;
            comboMidwayBonusCount_ = 0;
        }


        /// <summary>
        /// コンボ途切れ
        /// </summary>
        public void LostCombo()
        {
            if (UseComboInsurance()) return;

            IsCombo = false;

            // コンボ途切れボーナス加算
            LostComboBonus();

            // コンボ中に倒した敵の数を初期化
            ordinaryPeopleNum_ = 0;
            oldBattleaxeNum_ = 0;
            yakuzaNum_ = 0;

            // コンボ数を初期化
            ComboNum = 0;

            // コンボ残り時間を初期化
            ComboRemainingTime = 0f;

            // 途中コンボボーナスの用の変数を初期化
            comboMidwayBonusTime_ = 0f;
            comboMidwayBonusCount_ = 0;

            // バウンティーに現在のコンボ数を教える
            bountyManager_.SetNowCombo(comboNum_);
        }


        /// <summary>
        /// コンボ途中ボーナス
        /// </summary>
        /// <returns>コンボ途中ボーナス</returns>
        private void ComboMidwayBonus()
        {
            // コンボ数が0じゃない
            if (ComboNum == 0) return;
            // コンボ制限時間が0じゃない
            if (ComboRemainingTime <= 0) return;
            // コンボ数が十の倍数だったら
            if (comboMidwayBonusCount_ < bonusIntercal_) return;


            // コンボ途中ボーナス獲得！
            // 計算式：(100 ÷ 前回のコンボ途中ボーナスからかかった時間) × コンボ数
            var bonus = (int)(100f / (1f + comboMidwayBonusTime_) * ComboNum);
            // スコア加算
            scoreManager_.AddScore(bonus, Score.ADD_SCORE_TYPE.COMBO_SCORE);


            //コンボの区切り音
            AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_COMBO);


            // カウントから途中コンボボーナス間隔分引く
            comboMidwayBonusCount_ = comboMidwayBonusCount_ - bonusIntercal_;
            // 途中コンボボーナスにかかった時間を初期化
            comboMidwayBonusTime_ = 0f;
        }


        /// <summary>
        /// コンボ途切れボーナス
        /// </summary>
        /// <returns>コンボ途切れボーナス</returns>
        private void LostComboBonus()
        {
            // コンボ数が0なのにどうやってここに来るんだよ
            // 正解は、0コンボで間違った敵を撃った時だよ
            if (ComboNum == 0) return;


            // コンボ途切れボーナススコアの計算
            var lostComboBonus = (50f * ComboNum
                * (1f + ((1f - spawner_.ProbabilityOrdinaryPeople) * ordinaryPeopleNum_))
                * (1f + ((1f - spawner_.ProbabilityOldBattleaxe) * oldBattleaxeNum_))
                * (1f + ((1f - spawner_.ProbabilityYakuza) * yakuzaNum_)));
            // スコア加算
            scoreManager_.AddScore((int)lostComboBonus, Score.ADD_SCORE_TYPE.COMBO_SCORE);
        }


        /// <summary>
        /// コンボボーナス中のタイマー
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateComboTimer()
        {
            // タイマーが止まっているとき
            if (IsCombo == false)
            {
                IsCombo = true;

                // コンボが続かず制限時間がきれたら抜ける
                while (0f < ComboRemainingTime)
                {
                    // ↓↓↓ コンボ中 ↓↓↓

                    yield return null; // 一度終了して次フレームでこの下から実行する

                    // 毎フレームタイマーを減らす
                    ComboRemainingTime -= Time.deltaTime;
                    // コンボ途中ボーナスタイマーを進める
                    comboMidwayBonusTime_ += Time.deltaTime;
                }

                // ↓↓↓ コンボが途切れた時 ↓↓↓

                // コンボ保険が使えたらもう一度コンボを継続
                if (UseComboInsurance())
                {
                    // もう一度
                    SetComboBonusTimer();
                    IsCombo = false;
                    StartCoroutine(UpdateComboTimer());
                }
                // 使えなかったらコンボ終了
                else
                {
                    LostCombo();
                }
            }
        }


        /// <summary>
        /// コンボボーナスの時間を設定
        /// </summary>
        private void SetComboBonusTimer()
        {
            ComboRemainingTime = comboDuration_;
        }


        /// <summary>
        /// コンボ保険を使う
        /// </summary>
        /// <returns>true -> コンボ保険を使用　false -> コンボ保険未使用</returns>
        private bool UseComboInsurance()
        {
            // コンボ保険を持ってなかったら失敗
            if (IsComboInsurance == false) return false;
            // コンボ保険を持っていたら消費して成功を返す
            else
            {
                GameObject.FindGameObjectWithTag(TagName.GAME_CONTROLLER).GetComponent<PickupItemUI>().DeleteItem(ITEM_STATUS.COMBO_INSURANCE);
                IsComboInsurance = false;
                return true;
            }
        }
    }
}
