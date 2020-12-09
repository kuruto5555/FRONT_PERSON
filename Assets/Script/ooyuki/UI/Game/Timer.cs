using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    public class Timer : MonoBehaviour
    {
        [Header("タイマー表示UIテキスト")]
        [SerializeField]
        Text minute10_ = null;
        [SerializeField]
        Text minute1_ = null;
        [SerializeField]
        Text second10_ = null;
        [SerializeField]
        Text second1_ = null;


        [Header("制限時間(s)")]
        [SerializeField]
        float timeLimit_ = 1.0f;

        /// <summary>
        /// 時間制限に達したかどうか
        /// </summary>
        public bool IsTimeOver { get; private set; } = false;

        /// <summary>
        /// 分
        /// </summary>
        public float Minute { get; private set; } = 0f;

        /// <summary>
        /// 秒
        /// </summary>
        public float Second { get; private set; } = 0f;

        /// <summary>
        /// タイマーを進めていいかどうか
        /// </summary>
        bool IsCount_ = false;


        // Start is called before the first frame update
        void Start()
        {
            IsCount_ = false;
            IsTimeOver = false;
            Second = timeLimit_;

            // 分
            for (; Second >= 60f;)
            {
                Second -= 60f;
                Minute++;
            }

            SetTimeText();
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsCount_) return;
            if (IsTimeOver) return;

            // 秒
            Second -= Time.deltaTime;
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.T))
            {
                Second -= Time.deltaTime * 60f;
            }
#endif

            UpdateTimer();
        }


        /// <summary>
        /// タイマーを動かす
        /// </summary>
        public void TimerStart()
        {
            IsCount_ = true;
        }


        /// <summary>
        /// タイマーを止める
        /// </summary>
        public void TimerStop()
        {
            IsCount_ = false;
        }


        /// <summary>
        /// タイマーをリセット
        /// </summary>
        public void TimreLiset()
        {
            Start();
        }

        void UpdateTimer()
        {
            // 分
            if (Second < 0f && Minute > 0f)
            {
                Second += 60f;
                Minute--;
            }

            // 時間切れになったかどうか
            if (Second <= 0f && Minute <= 0f)
            {
                Second = Minute = 0f;
                IsTimeOver = true;
            }

            // テキストの更新
            SetTimeText();
        }


        void SetTimeText()
        {
            minute10_.text = ((int)(Minute * 0.1f)).ToString("0");
            minute1_.text =  ((int)(Minute % 10f)).ToString("0");
            second10_.text = ((int)(Second * 0.1f)).ToString("0");
            second1_.text =  ((int)(Second % 10f)).ToString("0");
        }
    }
}
