using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Item
{
    /// <summary>
    /// エネミーの持ち物
    /// </summary>
    public class EnemyBelongings : MonoBehaviour
    {
        [Serializable]
        public class DroppedItemInfo
        {
            [Header("出現させるアイテムのプレハブ")]
            public GameObject item_prefab_ = null;

            [Header("アイテムを落とす確率")]
            [Range(1, 100)]
            public int probability_of_dropping_item_ = 1;
        }

        [SerializeField]
        private List<DroppedItemInfo> items_ = new List<DroppedItemInfo>();

        /// <summary>
        /// アイテムを落とす
        /// </summary>
        /// <returns>アイテムのプレハブ</returns>
        public void DropItem()
        {
            // ランダムでアイテムを落とす
            foreach (var item in items_)
            {
                if(UnityEngine.Random.Range(1, 100) <= item.probability_of_dropping_item_)
                {
                    Instantiate(item.item_prefab_, transform.position, transform.rotation);
                }
            }
        }
    }
}
