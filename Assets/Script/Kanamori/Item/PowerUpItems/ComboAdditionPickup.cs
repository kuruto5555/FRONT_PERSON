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
        [Header("加算するコンボ数")]
        [SerializeField, Range(1, 9)]
        int addComboNum_ = 5;

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
            ComboManager.Instance.AddCombo(addComboNum_, ADD_COMBO_TYPE.ITEM);
        }
    }
}
