using FrontPerson.Constants;
using FrontPerson.Manager;
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

        Rank rank_ = Rank.C;

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
            rank_ = totalScore;
            totalScore_.text = rankText[rank_];
            totalScore_.color = fontColor_[(int)totalScore];
            GetComponent<Animator>().Play("TotalScoreAnimation");
            totalScore_.gameObject.SetActive(true);
        }

        public void AnimFinish()
        {
            IsAnimFinish_ = true;
            AudioManager.Instance.UnPauseBGM();
        }


        public void PlaySE_ScoreShow1()
        {
            AudioManager.Instance.PauseBGM();
            AudioManager.Instance.Play2DSE(gameObject, SEPath.RESULT_SE_SCORE_SHOW1);
        }



        public void PlaySE_ScoreShow2()
        {
            AudioManager.Instance.Play2DSE(gameObject, SEPath.RESULT_SE_SCORE_SHOW2);
        }


        public void PlaySE_ScoreRank()
        {
            switch (rank_)
            {
                case Rank.C:
                    AudioManager.Instance.Play2DSE(gameObject, SEPath.RESULT_SE_SCORE_C);
                    break;

                case Rank.B:
                    AudioManager.Instance.Play2DSE(gameObject, SEPath.RESULT_SE_SCORE_B);
                    break;

                case Rank.A:
                    AudioManager.Instance.Play2DSE(gameObject, SEPath.RESULT_SE_SCORE_A);
                    break;

                case Rank.S:
                    AudioManager.Instance.Play2DSE(gameObject, SEPath.RESULT_SE_SCORE_S);
                    break;
            }
        }
    }
}
