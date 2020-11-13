using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;

namespace FrontPerson.Item
{
    /// <summary>
    /// フィールド上に落ちているアイテムの基底クラス
    /// </summary>
    public abstract class FallingItem : MonoBehaviour
    {
        /// <summary>
        /// 回転速度
        /// </summary>
        private readonly float ROTATIONSPEED = 1f;
        /// <summary>
        /// ふわふわ浮く移動量
        /// </summary>
        private readonly float FLUFFY_MOOVEMENT = 0.5f;
        /// <summary>
        /// ふわふわの浮き沈みの速度
        /// </summary>
        private readonly float FLUFFY_SPEED = 2f;

        protected Transform transform_;

        private void Start()
        {
            // アイテムマネージャーにアイテムを管理させる。
            FallingItemManager.Instance.AddItem(this);

            transform_ = transform;

            OnStart();
        }

        private void Update()
        {
            // フィールド上に落ちているアイテムの挙動
            MovementItem();

            OnUpdate();
        }

        /// <summary>
        /// アイテムを拾った
        /// </summary>
        public void TakenItem(PlayerInventory inventory)
        {
            OnTakenItem(inventory);

            // マネージャーから消去
            FallingItemManager.Instance.RemoveItem(this);
        }

        /// <summary>
        /// 落ちているアイテムを削除する
        /// </summary>
        public void DestroyItem()
        {
            OnDestroyItem();

            GameObject.Destroy(this.gameObject);
        }

        /// <summary>
        /// 出現中のアイテムに挙動をもたせる。
        /// </summary>
        public void MovementItem()
        {
            // 回転
            transform_.Rotate(new Vector3(0f, ROTATIONSPEED, 0f));

            // ふわふわ浮かせる
            transform_.position = new Vector3(transform_.position.x, Mathf.PingPong(Time.time / FLUFFY_SPEED, FLUFFY_MOOVEMENT), transform_.position.z);
        }

        /* ---- 抽象関数 ----*/
        /// <summary>
        /// アイテムを拾った
        /// </summary>
        protected abstract void OnTakenItem(PlayerInventory inventory);
        /// <summary>
        /// 落ちているアイテムを削除する
        /// </summary>
        protected abstract void OnDestroyItem();
        /// <summary>
        /// 開始
        /// </summary>
        protected abstract void OnStart();
        /// <summary>
        /// 更新
        /// </summary>
        protected abstract void OnUpdate();
    }
}
