using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Item;
using System;

namespace FrontPerson.Manager
{
    // 管理するアイテムの情報
    public class ManagementItem
    {
        /// <summary>
        /// 管理するアイテム
        /// </summary>
        public Item.Item item_;

        /// <summary>
        /// 消失時間
        /// </summary>
        public float time_to_disappear_;

        public ManagementItem(Item.Item item_, int time_to_disappear_)
        {
            this.item_ = item_;
            this.time_to_disappear_ = time_to_disappear_;
        }
    }

    /// <summary>
    /// アイテム管理
    /// </summary>
    public class ItemManager : MonoBehaviour
    {
        [Header("アイテムの消失時間")]
        [Range(1, 100)]
        private int item_disappearance_time_ = 0;

        private  List<ManagementItem> management_items_;
        
        /// <summary>
        /// アイテムを追加
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(Item.Item item)
        {
            management_items_.Add(new ManagementItem(item, item_disappearance_time_));
        }

        /// <summary>
        /// アイテムを管理から外す
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(Item.Item item)
        {
            foreach (var i in management_items_)
            {
                if (i.item_ == item)
                {
                    // アイテムを削除する
                    management_items_.Remove(i);
                    item.DestroyItem();
                }
            }
        }
        private void Update()
        {
            foreach (var i in management_items_)
            {
                // 消滅時間になったらアイテムを消す
                if(i.time_to_disappear_ < 0)
                {
                    management_items_.Remove(i);
                    i.item_.DestroyItem();
                }
                
                i.time_to_disappear_ -= Time.deltaTime;
            }
        }
    }
}
