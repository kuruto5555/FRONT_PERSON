using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using FrontPerson.Manager;

namespace FrontPerson.Gimmick
{
    public class SupplyStation : NutrientsRecoveryPoint
    {
        [Header("一度補給してから次補給できるまでの時間")]
        [SerializeField, Range(0f, 60f)]
        private float interval_ = 0f;

        // ハンドガンは二個あるからそのカウント補給
        int chargeNum_ = 2;


        /// <summary>
        /// インターバル計測用
        /// </summary>
        float time_ = 0f;

        /// <summary>
        /// バウンティーマネージャー
        /// </summary>
        BountyManager bountyManager_ = null;


        private void Start()
        {
            bountyManager_ = BountyManager._instance;
        }

        private void Update()
        {
            if (!IsCharge)
            {
                time_ -= Time.deltaTime;
                if (time_ <= 0f)
                {
                    chargeNum_ = 2; // ハンドガンが二丁あるから
                    icon_.SetActive(true);
                    IsCharge = true;
                    time_ = 0f;
                }
            }
        }


        /// <summary>
        /// 弾の補給
        /// </summary>
        /// <param name="value">欲しい弾の量</param>
        /// <returns>もらえる弾の量</returns>
        public override int Charge(int value)
        {
            if (!IsCharge) return 0;
            if (value == 0) return 0;

            // インターバル時間が設定されていた場合
            if (interval_ > 0f)
            { 
                // ハンドガン二丁分の計測
                chargeNum_--;

                // 二丁分補給したら使えなくする
                if (chargeNum_ <= 0)
                {
                    time_ = interval_;
                    IsCharge = false;
                    icon_.SetActive(false);
                }
            }

            // 補充量を返す
            return value;
        }

    }

}
