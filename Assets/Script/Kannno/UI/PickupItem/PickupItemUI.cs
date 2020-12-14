using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.UI
{
    public class PickupItemUI : MonoBehaviour
    {
        static readonly int PICKUP_ITEM = Animator.StringToHash("PickupItem");

        private Animator animator = null;

        /// <summary>
        /// 
        /// </summary>
        private bool flag = true;

        private void OnEnable()
        {
            if(flag)
            {
                animator?.Play(PICKUP_ITEM);

                flag = false;
            }
        }

        void Start()
        {
            animator = GetComponent<Animator>();

            if (flag)
            {
                animator?.Play(PICKUP_ITEM);

                flag = false;
            }
        }

        private void Enable()
        {
            flag = true;
        }
    }
}