using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.UI;
using FrontPerson.Constants;
using UnityEngine.SocialPlatforms.Impl;

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

        [Header("タイムアップ")]
        [SerializeField]
        TimeUp timeUp_ = null;

        [Header("マネージャー")]
        [SerializeField]
        BountyManager bountyManager_ = null;

        /// <summary>
        /// スコアマネージャ―
        /// </summary>
        ScoreManager scoreManager_ = null;
        /// <summary>
        /// アプリケーションマネージャー
        /// </summary>
        ApplicationManager applicationManager_ = null;
        /// <summary>
        /// コンボマネージャー
        /// </summary>
        ComboManager comboManager_ = null;



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
            applicationManager_.SetIsInput(true);
            applicationManager_.SetIsGamePlay(false);

            // スコアマネージャ―取得
            scoreManager_ = ScoreManager.Instance;

            // コンボマネージャー取得
            comboManager_ = ComboManager.Instance;

            // BGM再生
            AudioManager.Instance.PlayBGM(gameObject, BGMPath.GAME_BGM_MAIN);

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
                // 無効にするObject
                tutorial1_.transform.root.gameObject.SetActive(false);

                // 有効にするObject
                tutorial2_.transform.root.gameObject.SetActive(true);

                // ステート切り替え
                state_ = GAME_SCENE_STATE.TUTORIAL2;
            }
        }


        void Tutorial2Update()
        {
            if (tutorial2_.IsFinish)
            {
                // 無効にするObject
                tutorial2_.transform.root.gameObject.SetActive(false);
                applicationManager_.SetIsInput(false);

                // 有効にするObject
                countdown_.transform.root.gameObject.SetActive(true);
                timer_.transform.root.gameObject.SetActive(true);

                // UI登場アニメーション再生
                FindObjectOfType<UI_MissionDraw>().gameObject.GetComponent<Animator>().Play("BountyManagerUI_IN");
                FindObjectOfType<Timer>().gameObject.GetComponent<Animator>().Play("TimerUI_IN");
                FindObjectOfType<ScoreManager>().gameObject.GetComponent<Animator>().Play("ScoreUI_IN");

                
                // ステートをカウントダウンに切り替え
                state_ = GAME_SCENE_STATE.START_COUNT_DOWN;
            }
        }


        void CountDownUpdate()
        {
            if (countdown_.IsCountdownFinish)
            {
                // 無効にするObject
                countdown_.gameObject.SetActive(false);

                // 有効にするObject
                bountyManager_.transform.root.gameObject.SetActive(true);
                applicationManager_.SetIsInput(true);
                applicationManager_.SetIsGamePlay(true);

                // タイマースタート
                timer_.TimerStart();

                // ステート切り替え
                state_ = GAME_SCENE_STATE.PLAY;
            }
        }


        void PlayUpdate()
        {
            //ゲームが終了したとき
            if (timer_.IsTimeOver)
            {
                // タイマーを停止
                timer_.TimerStop();

                // 無効にするObject
                applicationManager_.SetIsInput(false);
                applicationManager_.SetIsGamePlay(false);

                // 有効にするObject
                timeUp_.transform.root.gameObject.SetActive(true);

                // サウンド再生
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_TIME_UP);

                // ステート切り替え
                state_ = GAME_SCENE_STATE.TIME_UP;
            }
        }


        void TimeUpUpdate()
        {
            if (timeUp_.IsFinissh)
            {
                // スコア等を保存
                applicationManager_.ClearMissionNum = bountyManager_._missionCnt;
                applicationManager_.Score = scoreManager_.CurrentScore;
                applicationManager_.ComboNum = comboManager_.ComboNumMax;

                // ステート切り替え
                state_ = GAME_SCENE_STATE.TRANSITION;

                // シーンチェンジ
                SceneManager.Instance.SceneChange(SceneName.RESULT_SCENE, 1.0f, Color.black);
            }
        }


        void TransitionUpdate()
        {
            applicationManager_.SetIsInput(true);
            state_ = GAME_SCENE_STATE.FINISH;
        }
    }
}