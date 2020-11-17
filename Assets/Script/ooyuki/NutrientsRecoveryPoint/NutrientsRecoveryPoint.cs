using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Constants;

namespace FrontPerson.Gimmick
{
    public abstract class NutrientsRecoveryPoint : MonoBehaviour
    {
        [Header("栄養素の種類")]
        [SerializeField]
        private NUTRIENTS_TYPE nutrientsType_ = NUTRIENTS_TYPE._ALL;

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
    }
}
