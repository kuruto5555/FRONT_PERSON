using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.UI;
using FrontPerson.Constants;

namespace FrontPerson.Manager
{
    public class GameSceneController : MonoBehaviour
    {
        public enum GAME_SCENE_STATE
        {
            TUTORIAL1 = 0,      // 操作説明
            TUTORIAL2,          // ルール説明
            START_COUNT_DOWN,   // ゲーム開始カウントダウン
            PLAY,               // ゲーム
            TIME_UP,            // タイムアップ
            TRANSITION,         // 遷移

            FINISH              // 終了
        }
        public GAME_SCENE_STATE state_
        { 
            get; 
            private set; 
        } = GAME_SCENE_STATE.TUTORIAL1;

        

        [Header("操作説明UI")]
        [SerializeField]
        Tutorial tutorial1_ = null;

        [Header("ゲームルール説明UI")]
        [SerializeField]
        Tutorial tutorial2_ = null;

        [Header("タイマー")]
        [SerializeField]
        Timer timer_ = null;

        [Header("カウントダウン")]
        [SerializeField]
        Countdown countdown_ = null;

        [Header("バウンティマネージャー")]
        [SerializeField]
        BountyManager bountyManager_ = null;

        //[Header("タイムアップ")]
        //[SerializeField]




        /// <summary>
        /// アプリケーションマネージャー
        /// </summary>
        ApplicationManager applicationManager_;



        // Start is called before the first frame update
        void Start()
        {
            state_ = GAME_SCENE_STATE.TUTORIAL1;
            tutorial1_.transform.root.gameObject.SetActive(true);
            tutorial2_.transform.root.gameObject.SetActive(false);
            countdown_.transform.root.gameObject.SetActive(false);
            bountyManager_.transform.root.gameObject.SetActive(false);
            timer_.transform.root.gameObject.SetActive(false);
            timer_.TimerStop();


            applicationManager_ = FindObjectOfType<ApplicationManager>();
            applicationManager_.IsInput = true;
            applicationManager_.IsGamePlay = false;
        }

        // Update is called once per frame
        void Update()
        {
            switch (state_)
            {
                case GAME_SCENE_STATE.TUTORIAL1:
                    Tutorial1Update();
                    break;

                case GAME_SCENE_STATE.TUTORIAL2:
                    Tutorial2Update();
                    break;

                case GAME_SCENE_STATE.START_COUNT_DOWN:
                    CountDownUpdate();
                    break;

                case GAME_SCENE_STATE.PLAY:
                    PlayUpdate();
                    break;

                case GAME_SCENE_STATE.TIME_UP:
                    TimeUpUpdate();
                    break;

                case GAME_SCENE_STATE.TRANSITION:
                    TransitionUpdate();
                    break;
            }
        }

        void Tutorial1Update()
        {
            if (tutorial1_.IsFinish)
            {
                tutorial1_.transform.root.gameObject.SetActive(false);
                tutorial2_.transform.root.gameObject.SetActive(true);
                state_ = GAME_SCENE_STATE.TUTORIAL2;
            }
        }

        void Tutorial2Update()
        {
            if (tutorial2_.IsFinish)
            {
                tutorial2_.transform.root.gameObject.SetActive(false);
                countdown_.transform.root.gameObject.SetActive(true);
                timer_.transform.root.gameObject.SetActive(true);
                applicationManager_.IsInput = false;

                state_ = GAME_SCENE_STATE.START_COUNT_DOWN;
            }
        }

        void CountDownUpdate()
        {
            if (countdown_.IsCountdownFinish)
            {
                bountyManager_.transform.root.gameObject.SetActive(true);
                timer_.TimerStart();
                applicationManager_.IsInput = true;
                applicationManager_.IsGamePlay = true;

                state_ = GAME_SCENE_STATE.PLAY;
            }
        }

        void PlayUpdate()
        {
            if (timer_.IsTimeOver)
            {
                Time.timeScale = 0;
                applicationManager_.Score = 10000;
                applicationManager_.ComboNum = 10;
                applicationManager_.ClearMissionNum = 0;
                applicationManager_.IsInput = false;
                applicationManager_.IsGamePlay = false;

                state_ = GAME_SCENE_STATE.TIME_UP;
            }
        }

        void TimeUpUpdate()
        {
            state_ = GAME_SCENE_STATE.TRANSITION;

            //applicationManager_.Score = ScoreManager.Instance.;
            //applicationManager_.ComboNum = ScoreManager.Instance.;
            applicationManager_.ClearMissionNum = bountyManager_._missionCnt;
            SceneManager.Instance.SceneChange(SceneName.RESULT_SCENE, 1.0f, Color.black);
        }

        void TransitionUpdate()
        {
            applicationManager_.IsInput = true;
            state_ = GAME_SCENE_STATE.FINISH;
        }
    }
}