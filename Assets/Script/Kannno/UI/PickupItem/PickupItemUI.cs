using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using FrontPerson.Constants;

namespace FrontPerson.UI
{
    public class PickupItemUI : MonoBehaviour
    {
        [SerializeField]
        private List<Image> ItemImages = new List<Image>();

        [Tooltip("透明化、速度上昇、コンボ保険 の順番で設定して下さい")]
        [Header("アイテムのアイコン")]
        [SerializeField]
        private List<Sprite> ItemSprites = new List<Sprite>();

        /// <summary>
        /// 所持しているアイテム一覧
        /// </summary>
        private List<ITEM_STATUS> HaveItems = new List<ITEM_STATUS>() { ITEM_STATUS.NORMAL, ITEM_STATUS.NORMAL, ITEM_STATUS.NORMAL };

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
        /// アニメーション中かを表すフラグ(true = 再生中)
        /// </summary>
        private bool PlayAnime = false;

        private readonly int PICKUP_ITEM_01 = Animator.StringToHash("PickupItem01");
        private readonly int PICKUP_ITEM_02 = Animator.StringToHash("PickupItem02");
        private readonly int PICKUP_ITEM_03 = Animator.StringToHash("PickupItem03");

        void Start()
        {
            Animator = GetComponent<Animator>();

            foreach(var item in ItemImages)
            {
                item.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            //foreach (var item in ItemImages)
            //{
            //    Debug.Log(item.name + " " + item.color.ToString());
            //}
        }

        private void LateUpdate()
        {
            PlayAnimation();
        }


        /// <summary>
        /// アニメーションの再生関数
        /// </summary>
        private void PlayAnimation()
        {
            if (Before_ItemCnt != ItemCnt)
            {
                if (false == PlayAnime)
                {
                    switch (ItemCnt)
                    {
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

                    Before_ItemCnt = ItemCnt;
                    PlayAnime = true;

                    for (int i = 0; i < ItemCnt; i++)
                    {
                        ItemImages[i].gameObject.SetActive(true);
                    }
                }

                switch (ItemCnt)
                {
                    case 2:
                        Animator.SetBool("Item_02", true);
                        Animator.SetBool("Item_03", false);
                        break;

                    case 3:
                        Animator.SetBool("Item_02", false);
                        Animator.SetBool("Item_03", true);
                        break;

                    default:
                        break;
                }
            }
            else
            {
                if (1f <= Animator.GetCurrentAnimatorStateInfo(0).normalizedTime && PlayAnime)
                {
                    Animator.SetBool("Item_02", false);
                    Animator.SetBool("Item_03", false);

                    PlayAnime = false;
                }
            }
        }

        /// <summary>
        /// アイテムの追加
        /// </summary>
        /// <param name="item_status"></param>
        public void AddItem(ITEM_STATUS item_status)
        {
            if (HaveItems.Contains(item_status)) return;

            //if (0 == ItemCnt) ItemImages[0].gameObject.SetActive(true);

            for (int i = 0; i < HaveItems.Count(); i++)
            {
                if (HaveItems[i] == ITEM_STATUS.NORMAL)
                {
                    switch (item_status)
                    {
                        case ITEM_STATUS.INVICIBLE:
                            ItemImages[i].sprite = GetSprite(ITEM_STATUS.INVICIBLE);
                            HaveItems[i] = ITEM_STATUS.INVICIBLE;
                            break;

                        case ITEM_STATUS.SPEED_UP:
                            ItemImages[i].sprite = GetSprite(ITEM_STATUS.SPEED_UP);
                            HaveItems[i] = ITEM_STATUS.SPEED_UP;
                            break;

                        case ITEM_STATUS.COMBO_INSURANCE:
                            ItemImages[i].sprite = GetSprite(ITEM_STATUS.COMBO_INSURANCE);
                            HaveItems[i] = ITEM_STATUS.COMBO_INSURANCE;
                            break;

                        default:
                            return;
                    }

                    ItemImages[i].gameObject.SetActive(true);

                    // アイテムの取得数の加算
                    ItemCnt++;
                    break;
                }
            }
        }

        /// <summary>
        /// アイテムの削除
        /// </summary>
        /// <param name="item_status"></param>
        public void DeleteItem(ITEM_STATUS item_status)
        {
             if (!HaveItems.Contains(item_status)) return;

            for (int i = 0; i < HaveItems.Count(); i++)
            {
                if (HaveItems[i] == item_status)
                {
                    HaveItems[i] = ITEM_STATUS.NORMAL;
                    ItemImages[i].gameObject.SetActive(false);

                    // アイテムの取得数の減算
                    ItemCnt--;
                    Before_ItemCnt = ItemCnt;

                    switch (ItemCnt)
                    {
                        case 1:
                            if(0 == i)
                            {
                                ItemImages[i].sprite = GetSprite(HaveItems[i + 1]);
                                ItemImages[i + 1].gameObject.SetActive(false);

                                ItemImages[i].gameObject.SetActive(true);

                                HaveItems[i] = HaveItems[i + 1];
                                HaveItems[i + 1] = ITEM_STATUS.NORMAL;
                            }
                            break;

                        case 2:
                            if (0 == i)
                            {
                                ItemImages[i].sprite = GetSprite(HaveItems[i + 1]);
                                ItemImages[i+ 1].sprite = GetSprite(HaveItems[i + 2]);
                                ItemImages[i + 2].gameObject.SetActive(false);

                                ItemImages[i].gameObject.SetActive(true);
                                ItemImages[i + 1].gameObject.SetActive(true);

                                HaveItems[i] = HaveItems[i + 1];
                                HaveItems[i + 1] = HaveItems[i + 2];
                                HaveItems[i + 2] = ITEM_STATUS.NORMAL;
                            }
                            if (1 == i)
                            {
                                ItemImages[i].sprite = GetSprite(HaveItems[i + 1]);
                                ItemImages[i + 1].gameObject.SetActive(false);

                                ItemImages[i - 1].gameObject.SetActive(true);
                                ItemImages[i].gameObject.SetActive(true);

                                HaveItems[i] = HaveItems[i + 1];
                                HaveItems[i + 1] = ITEM_STATUS.NORMAL;
                            }
                            break;

                        default:
                            return;
                    }

                    break;
                }
            }

            if (0 == ItemCnt)
            {
                foreach (var item in ItemImages)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }

        private Sprite GetSprite(ITEM_STATUS item_status)
        {
            switch (item_status)
            {
                case ITEM_STATUS.INVICIBLE:
                    return ItemSprites[0];

                case ITEM_STATUS.SPEED_UP:
                    return ItemSprites[1];

                case ITEM_STATUS.COMBO_INSURANCE:
                    return ItemSprites[2];

                default:
                    return null;
            }
        }
    }
}