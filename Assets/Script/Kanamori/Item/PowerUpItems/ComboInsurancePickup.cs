﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FrontPerson.Character;
using FrontPerson.UI;

namespace FrontPerson.Item
{
    /// <summary>
    /// コンボ保険
    /// </summary>
    public class ComboInsurancePickup : MonoBehaviour
    {
        private PickupItemUI pickupItemUI_ = null;

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

            Manager.ComboManager.Instance.ActiveComboInsurance();

            pickupItemUI_.AddItem(Constants.ITEM_STATUS.COMBO_INSURANCE);
        }
    }
}