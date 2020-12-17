using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    public class Timer : MonoBehaviour
    {
        [Header("タイマー表示UIテキスト")]
        [Tooltip("分の十の位")]
        [SerializeField]
        Text minute10_ = null;
        [Tooltip("分の一の位")]
        [SerializeField]
        Text minute1_ = null;
        [Tooltip("秒の十の位")]
        [SerializeField]
        Text second10_ = null;
        [Tooltip("秒の一の位")]
        [SerializeField]
        Text second1_ = null;
        [Tooltip("分と秒の間の点々")]
        [SerializeField]
        Text colon_ = null;

        [Header("文字の色")]
        [Tooltip("残り時間で変わる色\n上から順に変わっていく")]
        [SerializeField]
        List<Color> textColorList_ = null;

        [Header("文字の色の変わるタイミング(s)")]
        [Tooltip("色が変わる残り時間\n上から順にみる")]
        [SerializeField]
        List<float> textColorChangeTimeList_ = null;

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

        /// <summary>
        /// タイマーの色のインデックス
        /// </summary>
        ushort textColorIndex_ = 0;

        /// <summary>
        /// アニメーター
        /// </summary>
        Animator animator_;


        // Start is called before the first frame update
        void Start()
        {
            animator_ = GetComponent<Animator>();

            IsCount_ = false;
            IsTimeOver = false;
            Second = timeLimit_;

            // 分
            for (; Second >= 60f;)
            {
                Second -= 60f;
                Minute++;
            }

            UpdateTextColor();
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
            // タイマー加速
            if (Input.GetKey(KeyCode.T))
            {
                Second -= Time.deltaTime * 59f;
            }
#endif

            UpdateTextColor();
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


        /// <summary>
        /// テキストの色を更新
        /// </summary>
        void UpdateTextColor()
        {
            if (textColorIndex_ >= textColorList_.Count) return;


            float remainingSecond = Second + Minute * 60f;
            if(textColorChangeTimeList_[textColorIndex_] >= remainingSecond)
            {
                // 色変更
                second1_.color = textColorList_[textColorIndex_];
                second10_.color = textColorList_[textColorIndex_];
                minute1_.color = textColorList_[textColorIndex_];
                minute10_.color = textColorList_[textColorIndex_];
                colon_.color = textColorList_[textColorIndex_];

                // 黄と赤の時はアニメーション再生
                switch (textColorIndex_)
                {
                    case 1:
                        animator_.Play("TimerYellowBlinking");
                        break;

                    case 2:
                        animator_.Play("TimerRedBlinking");
                        break;
                }
                textColorIndex_++;
            }
        }


        /// <summary>
        /// タイマーの更新
        /// </summary>
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


        /// <summary>
        /// テキストに時間をセット
        /// </summary>
        void SetTimeText()
        {
            minute10_.text = ((int)(Minute * 0.1f)).ToString("0");
            minute1_.text =  ((int)(Minute % 10f)).ToString("0");
            second10_.text = ((int)(Second * 0.1f)).ToString("0");
            second1_.text =  ((int)(Second % 10f)).ToString("0");
        }
    }
}
