using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;

namespace FrontPerson.Item
{
    // コンボ加算アイテム
    public class ComboAdditionPcikup : MonoBehaviour
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

            // コンボ追加
            ScoreManager.Instance.AddComboBonus(5);
        }
    }
}
