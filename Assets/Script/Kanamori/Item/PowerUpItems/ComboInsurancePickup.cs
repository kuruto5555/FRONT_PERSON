using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Character;

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

            Manager.ScoreManager.Instance.ActiveComboInsurance();
        }
    }
}