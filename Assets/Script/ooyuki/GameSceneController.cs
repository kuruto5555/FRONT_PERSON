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

        [Header("タイムアップ")]
        [SerializeField]
        TimeUp timeUp_ = null;



        /// <summary>
        /// アプリケーションマネージャー
        /// </summary>
        ApplicationManager applicationManager_;



        // Start is called before the first frame update
        void Start()
        {
            // 最初は操作説明のみ出す
            tutorial1_.transform.root.gameObject.SetActive(true);

            // 他はいったん非表示
            tutorial2_.transform.root.gameObject.SetActive(false);
            countdown_.transform.root.gameObject.SetActive(false);
            timeUp_.transform.root.gameObject.SetActive(false);
            bountyManager_.transform.root.gameObject.SetActive(false);
            timer_.transform.root.gameObject.SetActive(false);
            
            // タイマーを止めていく
            timer_.TimerStop();

            // アプリケーションマネージャーに現在の状態を保存
            applicationManager_ = FindObjectOfType<ApplicationManager>();
            applicationManager_.IsInput = true;
            applicationManager_.IsGamePlay = false;

            // ステートを操作説明にする
            state_ = GAME_SCENE_STATE.TUTORIAL1;
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
                timer_.TimerStop();
                timeUp_.transform.root.gameObject.SetActive(true);
                applicationManager_.IsInput = false;
                applicationManager_.IsGamePlay = false;
                state_ = GAME_SCENE_STATE.TIME_UP;
            }
        }

        void TimeUpUpdate()
        {
            if (timeUp_.IsFinissh)
            {
                // スコア等を保存
                applicationManager_.ClearMissionNum = bountyManager_._missionCnt;
                applicationManager_.Score = 10000;
                applicationManager_.ComboNum = 10;
                state_ = GAME_SCENE_STATE.TRANSITION;
                SceneManager.Instance.SceneChange(SceneName.RESULT_SCENE, 1.0f, Color.black);
            }
        }

        void TransitionUpdate()
        {
            applicationManager_.IsInput = true;
            state_ = GAME_SCENE_STATE.FINISH;
        }
    }
}