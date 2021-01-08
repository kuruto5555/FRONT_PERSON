using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;
using FrontPerson.Weapon;

namespace FrontPerson.Gimmick
{
    public class GarbageCan : NutrientsRecoveryPoint
    {
        [Header("補充できる量")]
        [SerializeField, Range(1, 100)]
        private int recoveryValue_ = 1;


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
            if (!IsCharge) return 0;
            if (value == 0) return 0;

            bountyManager_.NutritionCharge();
            IsCharge = false;
            icon_.SetActive(false);
            return value <= recoveryValue_ ? value : recoveryValue_;
        }


        /// <summary>
        /// 弾の補給
        /// </summary>
        /// <param name="guns">補充する銃</param>
        /// <returns></returns>
        public override void Charge(Gun[] guns)
        {
            // チャージできるフラグがfalseなら補給できない
            if (!IsCharge) return;
            // 補給量が0なら帰る
            var value = guns[0].MaxAmmo_ - guns[0].Ammo;
            if (value <= 0) return;


            bountyManager_.NutritionCharge();
            IsCharge = false;
            icon_.SetActive(false);
            guns[0].Reload(value <= recoveryValue_ ? value : recoveryValue_);
        }
    }
}
