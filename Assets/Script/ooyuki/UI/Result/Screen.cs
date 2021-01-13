using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    public class Screen : MonoBehaviour
    {
        [Header("スクリーン関連")]
        [SerializeField]
        [Tooltip("スクリーンのアニメーター")]
        Animator screenAnimator_ = null;
        [SerializeField]
        [Tooltip("スクリーンのImageコンポーネント")]
        Image screen_ = null;
        [SerializeField]
        [Tooltip("暗い時のスクリーンの色")]
        Color darkColor_;
        [SerializeField]
        [Tooltip("明るい時のスクリーンの色")]
        Color lightColor_;
        public bool IsScreenOpenAnimFinish { get; private set; } = false;



        /// <summary>
        /// スクリーンを出すアニメーションを再生するときに呼ぶやつ
        /// </summary>
        public void ScreenOpen()
        {
            screenAnimator_.Play("ScreenOpen");
        }


        /// <summary>
        /// スクリーンを出すアニメーションが終了したときに呼び出すやつ
        /// </summary>
        public void ScreenOpenFinish()
        {
            IsScreenOpenAnimFinish = true;
        }
    }
}
