using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FrontPerson.Bounty;

namespace FrontPerson.UI
{
    public class UI_MissionDraw : MonoBehaviour
    {
        [Header("バウンティーマネージャー")]
        [SerializeField]
        BountyManager bountyManager_ = null;

        /// <summary>
        /// ミッション名
        /// </summary>
        [Header("ミッション1")]
        [SerializeField]
        Text mission1NameText_ = null;
        /// <summary>
        /// 進捗表示用テキスト
        /// </summary>
        [SerializeField]
        Text mission1ProgressText_ = null;
        /// <summary>
        /// 残り時間タイマー
        /// </summary>
        [SerializeField]
        Image mission1Timer_ = null;


        [Header("ミッション2")]
        [SerializeField]
        Text mission2NameText_ = null;
        [SerializeField]
        Text mission2ProgressText_ = null;
        [SerializeField]
        Image mission2Timer_ = null;


        [Header("ミッション3")]
        [SerializeField]
        Text mission3NameText_ = null;
        [SerializeField]
        Text mission3ProgressText_ = null;
        [SerializeField]
        Image mission3Timer_ = null;


        void Start()
        {
            SetText();
        }

        // Update is called once per frame
        void Update()
        {
            SetText();
        }

        float a = 1f;
        void SetText()
        {
            // テスト
            a -= Time.deltaTime;
            mission1Timer_.fillAmount = a / 1f;
            if (a <= 0) a = 1f;


            Bounty.Bounty bounty1 = bountyManager_.GetBountyList[0];
            Bounty.Bounty bounty2 = bountyManager_.GetBountyList[1];
            Bounty.Bounty bounty3 = bountyManager_.GetBountyList[2];

            if (bounty1 != null)
            {
                mission1NameText_.text = bounty1.GetText.text;
                mission1ProgressText_.text = bounty1.GetProgressString;
                mission1Timer_.fillAmount = bounty1.GetNowTime / bounty1.GetLimitTime;
            }

            if (bounty2 != null)
            {
                mission2NameText_.text = bounty2.GetText.text;
                mission2ProgressText_.text = bounty2.GetProgressString;
                mission2Timer_.fillAmount = bounty2.GetNowTime / bounty2.GetLimitTime;
            }

            if (bounty3 != null)
            {
                mission3NameText_.text = bounty3.GetText.text;
                mission3ProgressText_.text = bounty3.GetProgressString;
                mission3Timer_.fillAmount = bounty3.GetNowTime / bounty3.GetLimitTime;
            }
        }
    }
}
