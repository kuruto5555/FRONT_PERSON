using FrontPerson.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    public class UI_FeverTime : MonoBehaviour
    {
        [SerializeField]
        Text scoreText_ = null;

        [SerializeField]
        RectTransform gauge_ = null;

        ScoreManager scoreManager_ = null;

        // Start is called before the first frame update
        void Start()
        {
            scoreManager_ = ScoreManager.Instance;
            scoreText_.text = "00000000";
            gauge_.anchorMin = new Vector2(1.0f, gauge_.anchorMin.y);

            //gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            //スコアマネージャーからフィーバー中のスコアをもらう
            scoreText_.text = scoreManager_.CurrentScore.ToString("00000000");
            gauge_.anchorMin = new Vector2(1f-(scoreManager_.FeverTimer / scoreManager_.FeverEffectTime), gauge_.anchorMin.y);

            //if (!scoreManager_.IsFever)
            //{
            //    gameObject.SetActive(false);
            //}
        }

        public void FeverStart()
        {
            gameObject.SetActive(true);
            scoreText_.text = scoreManager_.CurrentScore.ToString("00000000");
            gauge_.anchorMin = new Vector2(scoreManager_.FeverTimer / scoreManager_.FeverEffectTime, gauge_.anchorMin.y);
        }
    }
}
