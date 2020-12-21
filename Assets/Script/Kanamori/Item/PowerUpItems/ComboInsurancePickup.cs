using System.Collections;
using System.Collections.Generic;
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

            Manager.ComboManager.Instance.ActiveComboInsurance();

            var gameobjs = GameObject.FindGameObjectsWithTag(Constants.TagName.GAME_CONTROLLER);

            foreach(var obj in gameobjs)
            {
                var pickup_itemUI = obj.GetComponent<PickupItemUI>();

                if (pickup_itemUI)
                {
                    pickup_itemUI.AddItem(Constants.ITEM_STATUS.COMBO_INSURANCE);

                    break;
                }
            }
        }
    }
}