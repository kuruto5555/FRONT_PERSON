using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    public class PickupItemUI : MonoBehaviour
    {
        [SerializeField]
        private List<Image> ItemImages = new List<Image>();

        [Header("アイテムのアイコン")]
        [SerializeField]
        private List<Sprite> ItemSprites = new List<Sprite>();

        private Animator Animator = null;

        /// <summary>
        /// 取得したアイテムの数
        /// </summary>
        private int ItemCnt = 0;

        /// <summary>
        /// 前フレームの取得したアイテムの数
        /// </summary>
        private int Before_ItemCnt = 0;

        /// <summary>
        /// 
        /// </summary>
        private bool flag = false;

        private readonly int PICKUP_ITEM_01 = Animator.StringToHash("PickupItem01");
        private readonly int PICKUP_ITEM_02 = Animator.StringToHash("PickupItem02");
        private readonly int PICKUP_ITEM_03 = Animator.StringToHash("PickupItem03");

        private void OnEnable()
        {
            if(flag)
            {
                Animator?.Play(PICKUP_ITEM_01);

                flag = false;
            }
        }

        void Start()
        {
            flag = true;

            Animator = GetComponent<Animator>();

            PlayAnimation();
        }

        private void Enable()
        {
            flag = true;
        }

        /// <summary>
        /// アニメーションの再生関数
        /// </summary>
        private void PlayAnimation()
        {
            if (Before_ItemCnt < ItemCnt)
            {
                switch (ItemCnt)
                {
                    case 0:
                        break;

                    case 1:
                        Animator.Play(PICKUP_ITEM_01);
                        break;

                    case 2:
                        Animator.Play(PICKUP_ITEM_02);
                        break;

                    case 3:
                        Animator.Play(PICKUP_ITEM_03);
                        break;

                    default:
                        break;
                }
            }
        }

        private void ChangeImage()
        {
            if (ItemCnt < Before_ItemCnt)
            {
                
            }
        }
    }
}