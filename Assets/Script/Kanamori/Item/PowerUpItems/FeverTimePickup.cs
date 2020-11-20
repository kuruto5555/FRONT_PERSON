using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;

namespace FrontPerson.Item
{
    // フィーバータイムアイテム
    public class FeverTimePickup : MonoBehaviour
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

            // フィーバータイム発動
            ScoreManager.Instance.ActiveFeverTime();
        }
    }
}
