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
        private bool flag = false;

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
            flag = true;

            animator = GetComponent<Animator>();

            //animator?.Play(PICKUP_ITEM);
        }

        private void Enable()
        {
            flag = true;
        }
    }
}