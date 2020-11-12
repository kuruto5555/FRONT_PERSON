using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Item
{
    /// <summary>
    /// アイテムの基底クラス
    /// </summary>
    public abstract class Item : MonoBehaviour
    {
        /// <summary>
        /// アイテムを拾った
        /// </summary>
        public abstract void TakenItem();

        /// <summary>
        /// アイテムを削除する
        /// </summary>
        public abstract void DestroyItem();

        /// <summary>
        /// 出現中のアイテムに挙動をもたせる。
        /// </summary>
        public void MovementItem()
        {
            
        }
    }
}
