using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;

namespace FrontPerson.Gimmick
{
    public class GarbageCan : NutrientsRecoveryPoint
    {
        [Header("補充できる量")]
        [SerializeField, Range(1, 100)]
        private int recoveryValue_ = 1;

        /// <summary>
        /// 一度使っているかどうか
        /// </summary>
        public bool IsUsed { get; private set; } = true;


        /// <summary>
        /// バウンティマネージャー
        /// </summary>
        BountyManager bountyManager_ = null;


        private void Start()
        {
            bountyManager_ = BountyManager._instance;
        }


        /// <summary>
        /// 弾の補給
        /// </summary>
        /// <param name="value">欲しい弾の量</param>
        /// <returns>もらえる弾の量</returns>
        public override int Charge(int value)
        {
            if (IsUsed) return 0;

            bountyManager_.NutritionCharge();
            IsUsed = true;
            icon_.SetActive(false);
            return value <= recoveryValue_ ? value : recoveryValue_;
        }
    }
}
