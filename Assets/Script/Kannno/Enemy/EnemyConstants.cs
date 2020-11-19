using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Enemy
{
    /// <summary>
    /// 敵の種類を表す定数
    /// </summary>
    public enum EnemyType
    {
        /// <summary>
        /// 一般人
        /// </summary>
        ORDINATY_PEOPLE,

        /// <summary>
        // おばちゃん
        /// </summary>
        OLD_BATTLEAXE,

        /// <summary>
        // ヤクザ
        /// </summary>
        YAKUZA,

        NONE
    };

    /// <summary>
    /// 敵の獲得スコアを表す定数
    /// </summary>
    public enum EnemyScore
    {
        /// <summary>
        /// 一般人
        /// </summary>
        ORDINATY_PEOPLE = 100,

        /// <summary>
        // おばちゃん
        /// </summary>
        OLD_BATTLEAXE = 150,

        /// <summary>
        // ヤクザ
        /// </summary>
        YAKUZA = 200
    };
}