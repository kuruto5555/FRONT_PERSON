using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Item
{
    /// <summary>
    /// プレイヤーの持ち物
    /// </summary>
    public class PlayerInventory : MonoBehaviour
    {
        /// <summary>
        /// 取得したアイテムの総数
        /// </summary>
        private int[] acquired_item_total_count_ = new int[(int)PowerUpItemType.Max];

        /// <summary>
        /// 取ることが可能なアイテム
        /// </summary>
        private GameObject item_take_possible_;

        // Start is called before the first frame update
        private void Start()
        {
        }
        
        /// <summary>
        /// アイテムを持ち物に加える
        /// </summary>
        public void AddItemToInventory()
        {

        }
    }
}
