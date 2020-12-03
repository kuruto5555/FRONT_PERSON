using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    public class Gauge
    {
        int   data_ = 0;
        float animSpeed_ = 0;
        float gaugeAnchorMax_X = 0.0f;

        RectTransform gauge_ = null;
        Text dataText_ = null;

        public bool IsAnimation { get; private set; }


        /// <summary>
        /// アニメーション
        /// </summary>
        public void AnimationUpdate()
        {
            if (!IsAnimation) return;

            gauge_.anchorMax += new Vector2(animSpeed_ * Time.deltaTime, 0.0f);
            if (gauge_.anchorMax.x >= gaugeAnchorMax_X)
            {
                dataText_.gameObject.SetActive(true);
                dataText_.text = data_.ToString();
                gauge_.anchorMax = new Vector2(gaugeAnchorMax_X, gauge_.anchorMax.y);
                IsAnimation = false;
            }
        }


        /// <summary>
        /// アニメーションの開始
        /// アニメーションを開始していたら帰る
        /// </summary>
        /// <param name="gauge">ゲージのRectTransform</param>
        /// <param name="dataText">数値を表示するText</param>
        /// <param name="data">データ</param>
        /// <param name="max">グラフの最大値</param>
        /// <param name="animSpeed">何秒でアニメーションさせるか</param>
        public void StartAnimation(RectTransform gauge, Text dataText, int data, int max, float animSpeed)
        {
            if (IsAnimation) return;

            gauge_ = gauge;
            dataText_ = dataText;
            animSpeed_ = 1 / animSpeed;
            data_ = data;

            gaugeAnchorMax_X = data_ / max;

            IsAnimation = true;
        }


        /// <summary>
        /// アニメーションを終了させる
        /// アニメーションが開始していなかったら帰る
        /// </summary>
        public void StopAnimation()
        {
            if (!IsAnimation) return;

            dataText_.gameObject.SetActive(true);
            dataText_.text = data_.ToString();
            gauge_.anchorMax = new Vector2(gaugeAnchorMax_X, gauge_.anchorMax.y);
            IsAnimation = false;
        }

    }
}
