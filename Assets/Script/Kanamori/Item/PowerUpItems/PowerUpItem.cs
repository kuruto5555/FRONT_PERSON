using FrontPerson.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FrontPerson.Item
{
    public class PowerUpItem : MonoBehaviour
    {
        [SerializeField]
        private PowerUpItemType item_type_;

        // Start is called before the first frame update
        private void Start()
        {
        }

        /// <summary>
        /// アイテムを取った
        /// </summary>
        public void TakenItem()
        {
            switch (item_type_)
            {
                case PowerUpItemType.MOVEMENT_SPEED:
                    {
                        break;
                    }
                case PowerUpItemType.COMBO_INSURANCE:
                    {
                        break;
                    }
                case PowerUpItemType.COMBO_ADDITION:
                    {
                        break;
                    }
                case PowerUpItemType.NO_DAMAGE:
                    {
                        break;
                    }
            }

            GameObject.Destroy(this.gameObject);
        }
    }
}
