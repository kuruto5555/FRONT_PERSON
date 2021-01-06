using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FrontPerson.Character.Skill;


namespace FrontPerson.UI
{
    public class UI_Skill : MonoBehaviour
    {
        [Header("アイコンのゲージ")]
        [SerializeField]
        Image gauge_ = null;

        [Header("クールタイム表示テキスト")]
        [SerializeField]
        Text coolTime_ = null;


        /// <summary>
        /// サーチエリア
        /// </summary>
        SearchArea searchArea_ = null;


        /// <summary>
        /// 
        /// </summary>
        bool isUpdate = false;

        // Start is called before the first frame update
        void Start()
        {
            //プレイヤーのサーチエリア取得
            searchArea_ = FindObjectOfType<SearchArea>();

            // 初期化
            gauge_.fillAmount = 0f;
            coolTime_.text = "00.00";
            coolTime_.gameObject.SetActive(false);
            isUpdate = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isUpdate) return;

            CoolTimeUpdate();

        }


        public void StartCoolTime()
        {
            gauge_.fillAmount = 1f;
            coolTime_.gameObject.SetActive(true);
            coolTime_.text = searchArea_.SkillIntervalTimeCount_.ToString();
            isUpdate = true;
        }

        void CoolTimeUpdate()
        {
            gauge_.fillAmount = searchArea_.SkillIntervalTimeCount_ / searchArea_.SkillIntervalTime;
            coolTime_.text = searchArea_.SkillIntervalTimeCount_.ToString("0.00");

            //ゲージがなくなったらクールタイムのテキストを消して更新をストップする
            if (gauge_.fillAmount <= 0)
            {
                gauge_.fillAmount = 0f;
                coolTime_.text = "0.00";
                coolTime_.gameObject.SetActive(false);
                isUpdate = false;
            }
        }
    }
}

