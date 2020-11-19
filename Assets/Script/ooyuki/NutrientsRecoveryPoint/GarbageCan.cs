using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Constants;

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
        public bool IsUse { get; private set; }


        /// <summary>
        /// 弾の補給
        /// </summary>
        /// <param name="value">欲しい弾の量</param>
        /// <returns>もらえる弾の量</returns>
        public override int Charge(int value)
        {
            if (IsUse) return 0;

            IsUse = true;
            return value <= recoveryValue_ ? value : recoveryValue_;
        }
    }
}
