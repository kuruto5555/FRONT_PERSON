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
            TUTORIAL = 0,
            START_COUNT_DOWN,
            PLAY,
            TIME_UP,
            TRANSITION,

            FINISH
        }
        public GAME_SCENE_STATE state_
        { 
            get; 
            private set; 
        } = GAME_SCENE_STATE.TUTORIAL;

        

        [Header("チュートリアルUI")]
        [SerializeField]
        Tutorial tutorial_ = null;

        [Header("タイマー")]
        [SerializeField]
        Timer timer_ = null;

        [Header("カウントダウン")]
        [SerializeField]
        Countdown countdown_ = null;

        [Header("バウンティマネージャー")]
        [SerializeField]
        BountyManager bountyManager_ = null;

        



        /// <summary>
        /// アプリケーションマネージャー
        /// </summary>
        ApplicationManager applicationManager_;



        // Start is called before the first frame update
        void Start()
        {
            state_ = GAME_SCENE_STATE.TUTORIAL;
            tutorial_.transform.root.gameObject.SetActive(true);
            countdown_.transform.root.gameObject.SetActive(false);
            bountyManager_.transform.root.gameObject.SetActive(false);
            timer_.transform.root.gameObject.SetActive(false);
            timer_.TimerStop();


            applicationManager_ = FindObjectOfType<ApplicationManager>();
            applicationManager_.IsInput = false;
        }

        // Update is called once per frame
        void Update()
        {
            switch (state_)
            {
                case GAME_SCENE_STATE.TUTORIAL:
                    TutorialUpdate();
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

        void TutorialUpdate()
        {
            if (tutorial_.IsFinish)
            {
                tutorial_.transform.root.gameObject.SetActive(false);
                countdown_.transform.gameObject.SetActive(true);
                timer_.transform.root.gameObject.SetActive(true);
                state_ = GAME_SCENE_STATE.START_COUNT_DOWN;
            }
        }

        void CountDownUpdate()
        {
            if (countdown_.IsCountdownFinish)
            {
                state_ = GAME_SCENE_STATE.PLAY;
                applicationManager_.IsInput = true;
                bountyManager_.transform.root.gameObject.SetActive(true);
                timer_.TimerStart();
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
                state_ = GAME_SCENE_STATE.TIME_UP;
                applicationManager_.IsInput = false;
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