using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Constants;
using FrontPerson.Weapon;

namespace FrontPerson.Gimmick
{
    public abstract class NutrientsRecoveryPoint : MonoBehaviour
    {
        [Header("栄養素の種類")]
        [SerializeField]
        private NUTRIENTS_TYPE nutrientsType_ = NUTRIENTS_TYPE._ALL;

        [Header("ビルボードアイコン")]
        [SerializeField]
        protected GameObject icon_ = null;

        /// <summary>
        /// 使えるかどうか
        /// true  -> チャージできる
        /// false -> チャージできない
        /// </summary>
        public bool IsCharge { get; protected set; } = true;

        /// <summary>
        /// 取り扱っているビタミンの種類
        /// </summary>
        public NUTRIENTS_TYPE VitaminType { get { return nutrientsType_; } }


        /// <summary>
        /// 弾の補給
        /// </summary>
        /// <param name="value">欲しい弾の量</param>
        /// <returns>もらえる弾の量</returns>
        public abstract int Charge(int value);

        /// <summary>
        /// 弾の補給
        /// </summary>
        /// <param name="guns">補充する銃</param>
        /// <returns></returns>
        public abstract void Charge(Gun[] guns);
    }
}
