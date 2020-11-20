using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
        private void OnPicked(PlayerInventory inventory)
        {
            pickup_.PlayPickupFeedback();

            Manager.ScoreManager.Instance.is_combo_insurance_ = true;
        }
    }
}