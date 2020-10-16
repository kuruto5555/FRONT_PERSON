using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace FrontPerson.Vitamin
{
    public enum VITAMIN_TYPE
    {
        VITAMIN_B,
        VITAMIN_C,
        VITAMIN_D,
    }



    public class VitaminRecoveryPoint : MonoBehaviour
    {
        [Header("ビタミンの種類")]
        public VITAMIN_TYPE vitaminType = VITAMIN_TYPE.VITAMIN_B;

        [Header("価格")]
        [Range(1, 200)]
        public int price = 50;

        [Header("在庫")]
        [Range(1,1000)]
        public int stock_MAX = 100;


    }

}
