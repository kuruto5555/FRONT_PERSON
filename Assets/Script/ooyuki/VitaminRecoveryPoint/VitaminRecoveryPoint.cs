﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace FrontPerson.Vitamin
{
    public class VitaminRecoveryPoint : MonoBehaviour
    {
        [Header("ビタミンの種類")]
        [SerializeField]
        private VITAMIN_TYPE vitaminType = VITAMIN_TYPE.VITAMIN_B;

        [Header("在庫　-1：無限")]
        [SerializeField, Range(-1, 1000)]
        private int stock_MAX = 100;


        private int stock_ = 0;


        /// <summary>
        /// 取り扱っているビタミンの種類
        /// </summary>
        public VITAMIN_TYPE ViteminType { get { return vitaminType; } }



        private void Start()
        {
            stock_ = stock_MAX;
        }


        public int Charge(int value)
        {
            int chargeValue = 0;

            if (stock_ == 0)
            {
                return chargeValue;
            }
            else if (stock_ < value)
            {
                chargeValue = stock_;
                stock_ = 0;
            }
            else if (stock_ >= value)
            {
                chargeValue = value;
                stock_ -= value;
            }

            return chargeValue;
        }

    }

}