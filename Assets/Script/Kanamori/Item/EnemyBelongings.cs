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
        [Header("出現させるアイテムのプレハブ")]
        [SerializeField]
        private GameObject item_prefab_ = null;

        [Header("アイテムを落とす確率")]
        [Range(1, 100)]
        [SerializeField]
        private int probability_of_dropping_item_ = 1;

        /// <summary>
        /// アイテムを落とす
        /// </summary>
        /// <returns>アイテムのプレハブ</returns>
        public void DropItem()
        {
            // ランダムでアイテムを落とす
            if(UnityEngine.Random.Range(1, 100) <= probability_of_dropping_item_)
            {
                Instantiate(item_prefab_, transform.position, transform.rotation);
            }
        }
    }
}
