using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.UI;
using UnityEngine.UI;
using FrontPerson.Constants;

namespace FrontPerson.Manager
{
    public class ResultSceneController : MonoBehaviour
    {
        enum RESULT_SCENE_STATE
        {
            FADE_IN = 0,
            DRAW_SCORE,
            DRAW_COMBO_NUM,
            DRAW_MISSION_CLEAR_NUM,
            DRAW_TOTAL_SUCORE,
            PLAYER_INPUT,
            TRANSITION,

            FINISH
        }
        RESULT_SCENE_STATE state_ = RESULT_SCENE_STATE.FADE_IN;



        [Header("グラフ")]
        [SerializeField]
        Graph scoreGraph_ = null;
        [SerializeField]
        Graph comboNumGraph_ = null;
        [SerializeField]
        Graph missionClearNumGraph_ = null;

        [Header("総合評価")]
        [SerializeField]
        TotalScore totalScore_ = null;

        [Header("戻るボタン")]
        [SerializeField]
        GameObject backButton_ = null;

        /// <summary>
        /// アプリケーションマネージャー
        /// </summary>
        ApplicationManager appManager_ = null;

        int youScore = 0;
        int averageScore = 0;
        int numberOneScore = 0;

        int youComboNum = 0;
        int averageComboNum = 0;
        int numberOneCmnboNum = 0;

        int youMissionClearNum = 0;
        int averageMissionClearNum = 0;
        int numberOneMissionClearNum = 0;

        /// <summary>
        /// 総合評価
        /// </summary>
        Rank rank_ = Rank.C;


        // Start is called before the first frame update
        void Start()
        {
            state_ = RESULT_SCENE_STATE.FADE_IN;

            backButton_.SetActive(false);

            appManager_ = FindObjectOfType<ApplicationManager>();
            appManager_.SetIsGamePlay(false);
            appManager_.SetIsInput(false);

            // 今回の戦績
            youScore = appManager_.Score;
            youComboNum = appManager_.ComboNum;
            youMissionClearNum = appManager_.ClearMissionNum;

            // みんなの平均
            for (int i = 0; i < appManager_.save_data_.RankingScore.Count; i++)
            {
                averageScore += appManager_.save_data_.RankingScore[i];
                //averageComboNum += appManager_.save_data_.RankingComboNum[i]
                //averageMissionClearNum += appManager_.save_data_.RankingMissionClearNum[i]
            }

            // 一位のデータ
            numberOneScore = appManager_.save_data_.RankingScore[0];
            //numberOneCmnboNum = appManager_.save_data_.RankingConboNum[0];
            //numberOneMissionClearNum = appManager_.save_data_.RankingMissionClearNum[0];

            //ここで総合評価を求める
            //rank_ =


            AudioManager.Instance.PlayBGM(gameObject, BGMPath.RESULT_BGM_MAIN);
            AudioManager.Instance.PauseBGM();
        }

        // Update is called once per frame
        void Update()
        {
            switch (state_)
            {
                case RESULT_SCENE_STATE.FADE_IN:
                    FadeInUpdate();
                    break;

                case RESULT_SCENE_STATE.DRAW_SCORE:
                    DrawScoewUpdate();
                    break;

                case RESULT_SCENE_STATE.DRAW_COMBO_NUM:
                    DrawComboNum();
                    break;

                case RESULT_SCENE_STATE.DRAW_MISSION_CLEAR_NUM:
                    DrawMissionClearNum();
                    break;

                case RESULT_SCENE_STATE.DRAW_TOTAL_SUCORE:
                    DrawTotalScore();
                    break;

                case RESULT_SCENE_STATE.PLAYER_INPUT:

                    break;

                case RESULT_SCENE_STATE.TRANSITION:
                    break;

                case RESULT_SCENE_STATE.FINISH:
                    break;
            }
        }


        /// <summary>
        /// フェード中の更新
        /// </summary>
        void FadeInUpdate()
        {
            if(!FadeManager.Instance.IsFade)
            {
                scoreGraph_.StartAnimation(youScore, 5000, 20000);
                //scoreGraph_.StartAnimation(youScore, averageScore, numberOneScore);
                appManager_.SetIsInput(true);
                state_ = RESULT_SCENE_STATE.DRAW_SCORE;
            }
        }


        /// <summary>
        /// スコアの表示中の更新
        /// </summary>
        void DrawScoewUpdate()
        {
            if (scoreGraph_.IsFinish)
            {
                comboNumGraph_.StartAnimation(youComboNum, 50, 150);
                //comboNumGraph_.StartAnimation(youComboNum, averageComboNum, numberOneCmnboNum);
                state_ = RESULT_SCENE_STATE.DRAW_COMBO_NUM;
            }
        }


        /// <summary>
        /// コンボ数の表示中の更新
        /// </summary>
        void DrawComboNum()
        {
            if (comboNumGraph_.IsFinish)
            {
                missionClearNumGraph_.StartAnimation(youMissionClearNum, 10, 50);
                //missionClearNumGraph_.StartAnimation(youMissionClearNum, averageMissionClearNum, numberOneMissionClearNum);
                state_ = RESULT_SCENE_STATE.DRAW_MISSION_CLEAR_NUM;
            }
        }


        /// <summary>
        /// ミッションクリア数の表示中の更新
        /// </summary>
        void DrawMissionClearNum()
        {
            if (missionClearNumGraph_.IsFinish)
            {
                totalScore_.StartAnimation(rank_);
                state_ = RESULT_SCENE_STATE.DRAW_TOTAL_SUCORE;
            }
        }


        /// <summary>
        /// 総合評価表示中の更新
        /// </summary>
        void DrawTotalScore()
        {
            if (totalScore_.IsAnimFinish_)
            {
                backButton_.SetActive(true);
                state_ = RESULT_SCENE_STATE.PLAYER_INPUT;
            }
        }
    }
}
