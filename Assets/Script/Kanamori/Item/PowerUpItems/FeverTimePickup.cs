﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;
using FrontPerson.Character;

namespace FrontPerson.Item
{
    // フィーバータイムアイテム
    public class FeverTimePickup : MonoBehaviour
    {
        [Header("フィーバータイム継続時間")]
        [SerializeField, Range(1f, 30f)]
        float feverTimeduration_ = 15f;

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

            // フィーバータイム発動
            ScoreManager.Instance.ActiveFeverTime(feverTimeduration_);
        }
    }
}
