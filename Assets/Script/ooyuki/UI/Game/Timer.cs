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
        Text minute_secondText_ = null;

        [SerializeField]
        Text millisecondText_ = null;

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

        // Start is called before the first frame update
        void Start()
        {
            IsTimeOver = false;
            Second = timeLimit_;

            // 分
            for (; Second > 60f;)
            {
                Second -= 60f;
                Minute++;
            }

            minute_secondText_.text =
                Minute.ToString("00") + ":" +
                Mathf.Floor(Second).ToString("00");
            millisecondText_.text = "\'" + ((Second - (int)Second) * 100).ToString("00");
        }

        // Update is called once per frame
        void Update()
        {
            if (IsTimeOver) return;

            // 秒
            Second -= Time.deltaTime;

            UpdateTimer();
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
            minute_secondText_.text =
                Minute.ToString("00") + ":" +
                Mathf.Floor(Second).ToString("00");
            millisecondText_.text = "\"" +
                ((Second - (int)Second) * 100).ToString("00");
        }
    }
}
