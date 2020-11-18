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

        void SetText()
        {
            mission1NameText_.text = bountyManager_.GetBountyList[0].GetText.text;
            //mission1ProgressText_.text = bountyManager_.;
            mission1Timer_.fillAmount = bountyManager_.GetBountyList[0].GetNowTime / 30;// bountyManager_.GetBountyList[0].;

            //mission2NameText_.text = bountyManager_.GetBountyList[1].GetText.text;
            //mission2ProgressText_.text = bountyManager_.;
            //mission2Timer_. = bountyManager_.;

            //mission3NameText_.text = bountyManager_.GetBountyList[2].GetText.text;
            //mission3ProgressText_.text = bountyManager_.;
            //mission3Timer_. = bountyManager_.;
        }
    }
}
