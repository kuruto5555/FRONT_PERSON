using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.UI;
using FrontPerson.Constants;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.EventSystems;

namespace FrontPerson.Manager
{
    public class GameSceneController : MonoBehaviour
    {
        public enum GAME_SCENE_STATE
        {
            TUTORIAL = 0,       // ゲーム説明
            START_COUNT_DOWN,   // ゲーム開始カウントダウン
            PLAY,               // ゲーム
            OPEN_GAME_MENU,     // メニュー開いてる
            TIME_UP,            // タイムアップ
            TRANSITION,         // 遷移

            FINISH              // 終了
        }
        public GAME_SCENE_STATE state_
        { 
            get; 
            private set; 
        } = GAME_SCENE_STATE.TUTORIAL;

        

        [Header("操作説明UI")]
        [SerializeField]
        Tutorial tutorial_ = null;

        [Header("タイマーUI")]
        [SerializeField]
        Timer timer_ = null;

        [Header("カウントダウンUI")]
        [SerializeField]
        Countdown countdown_ = null;

        [Header("ミッションリストUI")]
        [SerializeField]
        UI_MissionDraw missionDrawUI_ = null;

        [Header("タイムアップUI")]
        [SerializeField]
        TimeUp timeUp_ = null;

        [Header("残弾ゲージUI")]
        [SerializeField]
        RemainingBulletGauge remainingBulletGauge_ = null;

        [Header("スキルUI")]
        [SerializeField]
        UI_Skill skillUI_ = null;

        [Header("オプションメニュー")]
        [SerializeField]
        InGameOptionMenu optionMenu_ = null;


        /// <summary>
        /// アプリケーションマネージャー
        /// </summary>
        ApplicationManager applicationManager_ = null;
        /// <summary>
        /// バウンティマネージャー
        /// </summary>
        BountyManager bountyManager_ = null;
        /// <summary>
        /// スコアマネージャ―
        /// </summary>
        ScoreManager scoreManager_ = null;
        /// <summary>
        /// コンボマネージャー
        /// </summary>
        ComboManager comboManager_ = null;



        // Start is called before the first frame update
        void Start()
        {
            // スコアマネージャ―取得
            scoreManager_ = ScoreManager.Instance;
            // コンボマネージャー取得
            comboManager_ = ComboManager.Instance;
            // バウンティマネージャー取得
            bountyManager_ = BountyManager._instance;

            // アプリケーションマネージャーに現在の状態を保存
            applicationManager_ = FindObjectOfType<ApplicationManager>();
            applicationManager_.SetIsInput(true);
            applicationManager_.SetIsGamePlay(false);

            // 最初は操作説明のみ出す
            tutorial_.gameObject.SetActive(true);

            // 他はいったん非表示
            timer_.gameObject.SetActive(false);
            timeUp_.gameObject.SetActive(false);
            countdown_.gameObject.SetActive(false);
            scoreManager_.gameObject.SetActive(false);
            missionDrawUI_.gameObject.SetActive(false);
            bountyManager_.gameObject.SetActive(false);
            remainingBulletGauge_.gameObject.SetActive(false);
            optionMenu_.gameObject.SetActive(false);
            skillUI_.gameObject.SetActive(false);

            // タイマーを止めておく
            timer_.TimerStop();

            // マウスカーソルをだす
            CursorManager.CursorUnlock();

            // BGM再生
            AudioManager.Instance.PlayBGM(gameObject, BGMPath.GAME_BGM_MAIN, 3.0f);

            // ステートを操作説明にする
            state_ = GAME_SCENE_STATE.TUTORIAL;
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

                case GAME_SCENE_STATE.OPEN_GAME_MENU:
                    OpenMenuUpdate();
                    break;

                case GAME_SCENE_STATE.TIME_UP:
                    TimeUpUpdate();
                    break;

                case GAME_SCENE_STATE.TRANSITION:
                    TransitionUpdate();
                    break;

                case GAME_SCENE_STATE.FINISH:
                    break;
            }
        }


        void TutorialUpdate()
        {
            if (tutorial_.IsFinish)
            {
                // 無効にするObject
                tutorial_.gameObject.SetActive(false);

                // カウントダウン中は入力フラグを無効にする
                applicationManager_.SetIsInput(false);

                // 有効にするObject
                countdown_.gameObject.SetActive(true);
                timer_.gameObject.SetActive(true);
                scoreManager_.gameObject.SetActive(true);
                missionDrawUI_.gameObject.SetActive(true);
                remainingBulletGauge_.gameObject.SetActive(true);
                skillUI_.gameObject.SetActive(true);

                // UI登場アニメーション再生
                timer_.gameObject.GetComponent<Animator>().Play("TimerUI_IN");
                scoreManager_.gameObject.GetComponent<Animator>().Play("ScoreUI_IN");
                missionDrawUI_.gameObject.GetComponent<Animator>().Play("BountyManagerUI_IN");
                skillUI_.gameObject.GetComponent<Animator>().Play("SkillUI_IN");
                remainingBulletGauge_.transform.GetChild(0).gameObject.GetComponent<Animator>().Play("HandGunGageStart");

                // カーソルを消して画面中央にロック
                CursorManager.CursorLock();

                // カウントダウンSE再生
                countdown_.PlaySe();

                // ステート切り替え
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
                bountyManager_.gameObject.SetActive(true);
                applicationManager_.SetIsInput(true);
                applicationManager_.SetIsGamePlay(true);
                optionMenu_.gameObject.SetActive(true);

                // タイマースタート
                timer_.TimerStart();

                // ステート切り替え
                state_ = GAME_SCENE_STATE.PLAY;
            }
        }


        void PlayUpdate()
        {
            // メニューが開いたら
            if (optionMenu_.IsOpen)
            {
                // ゲーム中をフォルス
                applicationManager_.SetIsGamePlay(false);
                // カーソルをアンロック
                CursorManager.CursorUnlock();
                // ステートをメニューにする
                state_ = GAME_SCENE_STATE.OPEN_GAME_MENU;
            }

            //ゲームが終了したとき
            if (timer_.IsTimeOver)
            {
                // タイマーを停止
                timer_.TimerStop();

                // 無効にするObject
                applicationManager_.SetIsInput(false);
                applicationManager_.SetIsGamePlay(false);
                optionMenu_.gameObject.SetActive(false);

                // 有効にするObject
                timeUp_.gameObject.SetActive(true);

                // タイムアップのSE再生サウンド再生
                timeUp_.PlaySe();

                // ステート切り替え
                state_ = GAME_SCENE_STATE.TIME_UP;
            }
        }


        void OpenMenuUpdate()
        {
            if (optionMenu_.IsGoToTitle)
            {
                // ゲーム中をトゥルー
                applicationManager_.SetIsGamePlay(false);
                applicationManager_.SetIsInput(false);
                // カーソルをロック
                CursorManager.CursorUnlock();
                // ステートをプレイにする
                state_ = GAME_SCENE_STATE.TRANSITION;
            }

            // メニューが閉じたら
            else if (!optionMenu_.IsOpen)
            {
                // ゲーム中をトゥルー
                applicationManager_.SetIsGamePlay(true);
                // カーソルをロック
                CursorManager.CursorLock();
                // ステートをプレイにする
                state_ = GAME_SCENE_STATE.PLAY;
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

                // UI登場アニメーション再生
                timer_.gameObject.GetComponent<Animator>().Play("TimerUI_OUT");
                scoreManager_.gameObject.GetComponent<Animator>().Play("ScoreUI_OUT");
                missionDrawUI_.gameObject.GetComponent<Animator>().Play("BountyManagerUI_OUT");
                skillUI_.gameObject.GetComponent<Animator>().Play("SkillUI_OUT");
                
                // BGM停止
                AudioManager.Instance.StopBGM(3f);

                // シーンチェンジ
                SceneManager.Instance.SceneChange(SceneName.RESULT_SCENE, 3f, Color.black);
            }
        }


        void TransitionUpdate()
        {
            applicationManager_.SetIsInput(false);
            state_ = GAME_SCENE_STATE.FINISH;
        }
    }
}