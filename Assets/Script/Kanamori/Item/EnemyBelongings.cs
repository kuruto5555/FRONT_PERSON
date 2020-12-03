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
        [Header("アイテムを落とす確率")]
        [Range(1, 100)]
        [SerializeField]
        private int probability_of_dropping_item_ = 40;

        [Header("出現させるアイテムのプレハブ")]
        [SerializeField]
        private List<GameObject> item_prefabs_ = new List<GameObject>();

        /// <summary>
        /// アイテムを落とす
        /// </summary>
        /// <returns>アイテムのプレハブ</returns>
        public void DropItem()
        {
            // アイテムを確率で落とす
            if (UnityEngine.Random.Range(1, 100) <= probability_of_dropping_item_)
            {
                // ランダムでアイテムを落とす
                int pickup_item_num = UnityEngine.Random.Range(0, item_prefabs_.Count);
                var item = Instantiate(item_prefabs_[pickup_item_num], transform.position, transform.rotation);
                item.AddComponent<PickupDeadline>();
            }
        }

    }
}
