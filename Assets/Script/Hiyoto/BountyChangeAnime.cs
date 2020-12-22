using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;

namespace FrontPerson.Anim
{
    public class BountyChangeAnime : MonoBehaviour
    {

        private BountyManager bountyManager = null;

        [Header("バウンティの番号")]
        [SerializeField]
        int number = 0;

        /// <summary>
        /// アニメーター
        /// </summary>
        Animator animator_ = null;

        /// <summary>
        /// ポップアップアニメーションのハッシュ
        /// </summary>
        readonly int changeBounty_FailedAnimHash = Animator.StringToHash("BountyChange_Failed");
        readonly int changeBounty_CLearAnimHash = Animator.StringToHash("BountyChange_Clear");

        // Start is called before the first frame update
        void Start()
        {
            bountyManager = BountyManager._instance;

            //アニメーターの取得
            animator_ = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (bountyManager.GetBountyList[number].IsFinish)
            {
                if (bountyManager.GetBountyList[number].IsCrear)
                {
                    //クリア時のアニメーション
                    animator_.Play(changeBounty_CLearAnimHash);
                }
                else
                {
                    //失敗時のアニメーション
                    animator_.Play(changeBounty_FailedAnimHash);
                }

            }
        }
    }
}

