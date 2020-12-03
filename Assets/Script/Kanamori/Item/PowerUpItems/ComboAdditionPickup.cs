using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;
using FrontPerson.Character;

namespace FrontPerson.Item
{
    // コンボ加算アイテム
    public class ComboAdditionPickup : MonoBehaviour
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

            // コンボ追加(５追加)
            ScoreManager.Instance.AddComboBonus(5);
            // コンボボーナス開始
            ScoreManager.Instance.StartComboBonus();
        }
    }
}
