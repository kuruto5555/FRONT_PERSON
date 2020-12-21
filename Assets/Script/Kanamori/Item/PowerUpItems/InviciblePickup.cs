using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FrontPerson.Character;
using FrontPerson.UI;

namespace FrontPerson.Item
{
    /// <summary>
    /// 無敵
    /// </summary>
    public class InviciblePickup : MonoBehaviour
    {
        private PickupItemUI pickupItemUI_ = null;

        [SerializeField]
        private float invicible_time_ = 15f;

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

            // 無敵化
            player.SetInvincible(invicible_time_);

            pickupItemUI_.AddItem(Constants.ITEM_STATUS.INVICIBLE);
        }
    }
}
