using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Item;
using System;
using FrontPerson.common;

namespace FrontPerson.Manager
{
    // 管理するアイテムの情報
    public class ManagementItem
    {
        /// <summary>
        /// 管理するアイテム
        /// </summary>
        public FallingItem item_;

        /// <summary>
        /// 消失時間
        /// </summary>
        public float time_to_disappear_;

        public ManagementItem(FallingItem item_, int time_to_disappear_)
        {
            this.item_ = item_;
            this.time_to_disappear_ = time_to_disappear_;
        }
    }

    /// <summary>
    /// 落ちているアイテムの管理
    /// </summary>
    public class FallingItemManager : SingletonMonoBehaviour<FallingItemManager>
    {
        [Header("アイテムの消失時間")]
        [Range(1, 100)]
        [SerializeField]
        private int item_disappearance_time_ = 0;

        private  List<ManagementItem> management_items_ = new List<ManagementItem>();
        
        private void Update()
        {
            ManageTheTimeItTakesToRemoveItem();
        }

        /// <summary>
        /// アイテムを追加
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(FallingItem item)
        {
            // 管理するアイテムと消滅時間を設定
            management_items_.Add(new ManagementItem(item, item_disappearance_time_));
        }

        /// <summary>
        /// アイテムを削除する
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(FallingItem item)
        {
            for (int i = 0; i < management_items_.Count; i++)
            {
                var mi = management_items_[i];

                if (mi.item_ == item)
                {
                    // アイテムを削除する
                    RemoveManagementItem(mi);
                }
            }
        }

        /// <summary>
        /// アイテムを削除するまでの時間を管理する
        /// </summary>
        private void ManageTheTimeItTakesToRemoveItem()
        {
            for (int i = 0; i < management_items_.Count; i++)
            {
                var mi = management_items_[i];

                // 消滅時間になったらアイテムを消す
                if (mi.time_to_disappear_ <= 0)
                {
                    // アイテムを削除する
                    RemoveManagementItem(mi);
                }
                else
                {
                    mi.time_to_disappear_ -= Time.deltaTime;
                }
            }
        }

        /// <summary>
        /// 管理しているアイテムを削除する
        /// </summary>
        /// <param name="management_item"></param>
        private void RemoveManagementItem(ManagementItem management_item)
        {
            management_items_.Remove(management_item);
            management_item.item_.DestroyItem();
        }
    }
}
