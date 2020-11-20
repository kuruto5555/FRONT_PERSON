using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Character;

namespace FrontPerson.Item
{
    /// <summary>
    /// 無敵
    /// </summary>
    public class InviciblePickup : MonoBehaviour
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

            // 無敵化
        }
    }
}
