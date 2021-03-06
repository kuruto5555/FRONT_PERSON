﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FrontPerson.Manager;

namespace FrontPerson.UI
{
    public class UI_MissionDraw : MonoBehaviour
    {
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

        /// <summary>
        /// バウンティマネージャー
        /// </summary>
        BountyManager bountyManager_ = null;



        void Start()
        {
            bountyManager_ = BountyManager._instance;
            SetText();
        }

        // Update is called once per frame
        void Update()
        {
            SetText();
        }


        void SetText()
        {
            // テスト
            //a -= Time.deltaTime;
            //mission1Timer_.fillAmount = a / 1f;
            //if (a <= 0) a = 1f;


            Bounty.Bounty bounty1 = bountyManager_.MissionInfoList[0].mission;
            Bounty.Bounty bounty2 = bountyManager_.MissionInfoList[1].mission;
            Bounty.Bounty bounty3 = bountyManager_.MissionInfoList[2].mission;

            if (bounty1 != null)
            {
                mission1NameText_.text = bounty1.GetMissionName;
                mission1ProgressText_.text = bounty1.GetProgressString;
                mission1Timer_.fillAmount = bounty1.GetNowTime / bounty1.GetLimitTime;
            }

            if (bounty2 != null)
            {
                mission2NameText_.text = bounty2.GetMissionName;
                mission2ProgressText_.text = bounty2.GetProgressString;
                mission2Timer_.fillAmount = bounty2.GetNowTime / bounty2.GetLimitTime;
            }

            if (bounty3 != null)
            {
                mission3NameText_.text = bounty3.GetMissionName;
                mission3ProgressText_.text = bounty3.GetProgressString;
                mission3Timer_.fillAmount = bounty3.GetNowTime / bounty3.GetLimitTime;
            }
        }
    }
}
