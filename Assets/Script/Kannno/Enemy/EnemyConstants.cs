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

    public class EnemyAnimation
    {
        /// <summary>
        /// 歩きアニメーション
        /// </summary>
        static public readonly int Walk = Animator.StringToHash("Walk");

        /// <summary>
        /// 追いかけてくるアニメーション
        /// </summary>
        static public readonly int Run = Animator.StringToHash("Run");

        /// <summary>
        /// 怒るアニメーション
        /// </summary>
        static public readonly int Angry = Animator.StringToHash("Angry");

        /// <summary>
        /// 攻撃アニメーション
        /// </summary>
        static public readonly int Attack = Animator.StringToHash("Attack");

        /// <summary>
        /// 撃退アニメーション
        /// </summary>
        static public readonly int Repel = Animator.StringToHash("Repel");

        /// <summary>
        /// 元気アニメーション
        /// </summary>
        static public readonly int Fine = Animator.StringToHash("Fine");

        /// <summary>
        /// 逃げるアニメーション
        /// </summary>
        static public readonly int Escape = Animator.StringToHash("Escape");
    }
}