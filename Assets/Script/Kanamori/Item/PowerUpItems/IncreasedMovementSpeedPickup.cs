using System.Collections;
using System.Collections.Generic;
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
        }

        private void OnPicked(Player player)
        {
            pickup_.PlayPickupFeedback();

            // 移動速度アップ
            player.PickUpMovementSpeedItem(EFFECT_TIME, MOVEMENT_SPEED_MAGNIFICATION);

            var gameobjs = GameObject.FindGameObjectsWithTag(Constants.TagName.GAME_CONTROLLER);

            foreach (var obj in gameobjs)
            {
                var pickup_itemUI = obj.GetComponent<PickupItemUI>();

                if (pickup_itemUI)
                {
                    pickup_itemUI.AddItem(Constants.ITEM_STATUS.SPEED_UP);

                    break;
                }
            }
        }
    }
}