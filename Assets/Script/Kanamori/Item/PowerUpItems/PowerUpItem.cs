using FrontPerson.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FrontPerson.Item
{
    /// <summary>
    /// パワーアップアイテムの種類
    /// </summary>
    public enum PowerUpItemType
    {
        // 無敵
        // Aランクアイテム。ヤクザ、敵対したおばちゃんから一定時間内に攻撃をされなくなる。(透明化)
        Invicible,

        // コンボ保険
        // Bランクアイテム。コンボが途切れるのを一回だけ防げる。効果を発動するまでは所持し続ける。重複所持は不可。
        ComboInsurance,

        // フィーバータイム
        // Bランクアイテム。アイテムを獲得してから15秒間、スコアを1.5倍にする。
        FeverTime,

        // コンボ加算
        // Cランクアイテム。現在のコンボに＋5コンボ加える
        ComboAddition,

        // 移動速度アップ
        // Cランクアイテム。プレイヤーの移動速度が上がる。(とりあえずx1.5。)
        MoovementSpeedUp,

        Max
    }

    public class PowerUpItem : FallingItem
    {
        [Header("アイテムを獲得した際に出るエフェクト")]
        [SerializeField]
        private ParticleSystem effect_obtained_items_ = null;

        /// <summary>
        /// アイテムの種類
        /// </summary>
        private PowerUpItemType item_type_ = PowerUpItemType.Max;

        protected override void OnStart()
        {
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnTakenItem(PlayerInventory inventory)
        {
            // エフェクトを出す
            if (effect_obtained_items_)
            {
                Instantiate(effect_obtained_items_, transform_.position, transform_.rotation);
            }
        }

        protected override void OnDestroyItem()
        {
        }
    }
}
