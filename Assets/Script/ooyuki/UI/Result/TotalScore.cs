using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    public enum Rank
    {
        S,
        A,
        B,
        C
    }

    public class TotalScore : MonoBehaviour
    {
        Dictionary<Rank, string> rankText = new Dictionary<Rank, string>()
        {
            { Rank.S, "S" },
            { Rank.A, "A" },
            { Rank.B, "B" },
            { Rank.C, "C" },
        };

        [Header("総合評価(S～C)を出すテキスト")]
        [SerializeField]
        Text totalScore_ = null;

        [Header("文字の色S～C")]
        [SerializeField]
        List<Color> fontColor_ = null;

        /// <summary>
        /// 総合評価のアニメーションが終わったかどうか
        /// </summary>
        public bool IsAnimFinish_ { get; private set; } = false;

        // Start is called before the first frame update
        void Start()
        {
            totalScore_.gameObject.SetActive(false);
            totalScore_.text = rankText[Rank.C];
        }


        public void StartAnimation(Rank totalScore)
        {
            totalScore_.text = rankText[totalScore];
            totalScore_.color = fontColor_[(int)totalScore];
            GetComponent<Animator>().Play("TotalScoreAnimation");
            totalScore_.gameObject.SetActive(true);
        }

        public void AnimFinish()
        {
            IsAnimFinish_ = true;
        }
    }
}
