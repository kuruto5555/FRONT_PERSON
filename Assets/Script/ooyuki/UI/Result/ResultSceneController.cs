using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.UI;
using FrontPerson.Constants;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        Button backButton_ = null;

        [Header("ランキングへボタン")]
        [SerializeField]
        Button rabkingButton_ = null;


        [Header("トータルスコアの目安")]
        [Tooltip("上から、B、A、S、\nBより小さければC")]
        [SerializeField]
        List<int> totalScoreGuideline_ = null;

        /// <summary>
        /// アプリケーションマネージャー
        /// </summary>
        ApplicationManager appManager_ = null;

        int youScore = 95000000;
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

            backButton_.gameObject.SetActive(false);
            rabkingButton_.gameObject.SetActive(false);


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
                averageComboNum += appManager_.save_data_.RankingComboNum[i];
                averageMissionClearNum += appManager_.save_data_.RankingClearMissionNum[i];
            }
            averageScore /= appManager_.save_data_.RankingScore.Count;
            averageComboNum /= appManager_.save_data_.RankingScore.Count;
            averageMissionClearNum /= appManager_.save_data_.RankingScore.Count;

            // 一位のデータ
            numberOneScore = appManager_.save_data_.RankingScore[0];
            numberOneCmnboNum = appManager_.save_data_.RankingComboNum[0];
            numberOneMissionClearNum = appManager_.save_data_.RankingClearMissionNum[0];

            //ここで総合評価を求める
            rank_ = GetRank(youScore);

            // ランキング更新
            UpdateRanking(youScore, youComboNum, youMissionClearNum);

            //いちようカーソル有効か
            CursorManager.CursorUnlock();

            // BGM再生して一回ポーズ
            AudioManager.Instance.PlayBGM(gameObject, BGMPath.RESULT_BGM_MAIN, 2.0f);
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
                //scoreGraph_.StartAnimation(youScore, 5000, 20000);
                scoreGraph_.StartAnimation(youScore, averageScore, numberOneScore);
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
                //comboNumGraph_.StartAnimation(youComboNum, 50, 150);
                comboNumGraph_.StartAnimation(youComboNum, averageComboNum, numberOneCmnboNum);
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
                //missionClearNumGraph_.StartAnimation(youMissionClearNum, 10, 50);
                missionClearNumGraph_.StartAnimation(youMissionClearNum, averageMissionClearNum, numberOneMissionClearNum);
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
                backButton_.gameObject.SetActive(true);
                rabkingButton_.gameObject.SetActive(true);
                FirstTouchSelectable.Select(EventSystem.current, backButton_);

                state_ = RESULT_SCENE_STATE.PLAYER_INPUT;
            }
        }


        /// <summary>
        /// ランキングデータ更新用
        /// </summary>
        public struct RankingData
        {
            public RankingData(int score, int comboNum, int missionClearNum) { Score = score; ComboNum = comboNum; MissionClearNum = missionClearNum;  }
            public int Score;
            public int ComboNum;
            public int MissionClearNum;
        }
        /// <summary>
        /// ランキングデータ更新
        /// </summary>
        /// <param name="nowScore">今回のスコア</param>
        /// <param name="nowComboNum">今回のコンボ数</param>
        /// <param name="nowMissionClearNum">今回のミッションクリア回数</param>
        void UpdateRanking(int nowScore, int nowComboNum, int nowMissionClearNum)
        {
            // セーブデータ取得
            var saveData = appManager_.save_data_;

            // ソートするためにデータを格納
            var sortScoreData = new List<RankingData>()
            {
                { new RankingData(saveData.RankingScore[0], saveData.RankingComboNum[0], saveData.RankingClearMissionNum[0]) },
                { new RankingData(saveData.RankingScore[1], saveData.RankingComboNum[1], saveData.RankingClearMissionNum[1]) },
                { new RankingData(saveData.RankingScore[2], saveData.RankingComboNum[2], saveData.RankingClearMissionNum[2]) },
                { new RankingData(saveData.RankingScore[3], saveData.RankingComboNum[3], saveData.RankingClearMissionNum[3]) },
                { new RankingData(saveData.RankingScore[4], saveData.RankingComboNum[4], saveData.RankingClearMissionNum[4]) },
                { new RankingData(nowScore                , nowComboNum                , nowMissionClearNum                ) }
            };

            // ソート
            sortScoreData.Sort((a, b) => b.Score - a.Score);

            // 値を入れる
            int i = 0;
            foreach(var scoreData in sortScoreData)
            {
                saveData.RankingScore[i] = scoreData.Score;
                saveData.RankingComboNum[i] = scoreData.ComboNum;
                saveData.RankingClearMissionNum[i] = scoreData.MissionClearNum;
                i++;
                if (i >= 5) break;
            }
        }


        Rank GetRank(int nowScore)
        {
            if (totalScoreGuideline_[0] > nowScore) return Rank.C;
            if (totalScoreGuideline_[1] > nowScore) return Rank.B;
            if (totalScoreGuideline_[2] > nowScore) return Rank.A;
            return Rank.S;
        }
    }
}
