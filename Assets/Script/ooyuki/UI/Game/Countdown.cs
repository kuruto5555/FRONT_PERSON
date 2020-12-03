using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FrontPerson.UI
{
    public class Countdown : MonoBehaviour
    {
        [Header("カウントダウンアニメーター")]
        [SerializeField]
        Animator animator_;

        [Header("カウントダウンのテキスト")]
        [SerializeField]
        Text text_ = null;

        [Header("表示する文字（上から順に出る）")]
        [SerializeField]
        List<string> texts_ = null;



        /// <summary>
        /// カウントダウンが終了したかどうか
        /// </summary>
        [System.NonSerialized]
        public bool IsCountdownFinish = false;



        /// <summary>
        /// 表示する文字のインデックス
        /// </summary>
        int index_ = 0;


        void Start()
        {
            index_ = 0;
        }

        public void ChangeText()
        {
            if (index_ >= texts_.Count)
            {
                IsCountdownFinish = true;
                animator_.SetBool("IsFinish", true);
                return;
            }

            text_.text = texts_[index_];
            index_++;
        }
    }
}
