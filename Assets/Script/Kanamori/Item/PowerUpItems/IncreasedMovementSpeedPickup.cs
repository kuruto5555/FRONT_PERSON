using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FrontPerson.Manager;
using FrontPerson.Character;
using FrontPerson.UI;

namespace FrontPerson.Item
{
    /// <summary>
    /// 移動速度上昇アイテム
    /// </summary>
    public class IncreasedMovementSpeedPickup : MonoBehaviour
    {
        private PickupItemUI pickupItemUI_ = null;

        /// <summary>
        /// 効果時間
        /// </summary>
        private readonly float EFFECT_TIME = 30f;

        /// <summary>
        /// 移動速度増加倍率
        /// </summary>
        private readonly float MOVEMENT_SPEED_MAGNIFICATION = 1.5f;

        private Pickup pickup_;

        // Start is called before the first frame update
        void Start()
        {
            pickup_ = GetComponent<Pickup>();
            pickup_.onPick += OnPicked;

            pickupItemUI_ = GameObject.FindGameObjectWithTag(Constants.TagName.GAME_CONTROLLER).GetComponent<PickupItemUI>();
        }

        private void OnPicked(Player player)
        {
            pickup_.PlayPickupFeedback();

            // 移動速度アップ
            player.PickUpMovementSpeedItem(EFFECT_TIME, MOVEMENT_SPEED_MAGNIFICATION);

            pickupItemUI_.AddItem(Constants.ITEM_STATUS.SPEED_UP);
        }
    }
}