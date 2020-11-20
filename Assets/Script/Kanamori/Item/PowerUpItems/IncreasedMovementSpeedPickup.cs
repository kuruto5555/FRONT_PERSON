using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;
namespace FrontPerson.Item
{
    /// <summary>
    /// 移動速度上昇アイテム
    /// </summary>
    public class IncreasedMovementSpeedPickup : MonoBehaviour
    {
        private Pickup pickup_;

        // Start is called before the first frame update
        void Start()
        {
            pickup_ = GetComponent<Pickup>();
            pickup_.onPick += OnPicked;
        }

        private void OnPicked(PlayerInventory inventory)
        {
            pickup_.PlayPickupFeedback();

            // 移動速度アップ
        }
    }
}