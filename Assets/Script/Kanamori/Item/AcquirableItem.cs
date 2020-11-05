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
        MOVEMENT_SPEED,     // 移動速度
        COMBO_INSURANCE,    // コンボ保険
        COMBO_ADDITION,     // コンボ加算
        NO_DAMAGE,          // 攻撃されない
        MAX
    }

    public enum SpecialItemType
    {

    }
    
    /// <summary>
    /// 獲得できるアイテム
    /// </summary>
    public class AcquirableItem : MonoBehaviour
    {
        public delegate void TakenItem();
    }
}
