﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.UI;
using UnityEditorInternal;
using FrontPerson.Constants;

namespace FrontPerson.Manager
{
    public class GameSceneController : MonoBehaviour
    {
        enum GAME_SCENE_STATE
        {
            TUTORIAL = 0,
            START_COUNT_DOWN,
            PLAY,
            TIME_UP,
            TRANSITION,

            FINISH
        }
        GAME_SCENE_STATE state_ = GAME_SCENE_STATE.TUTORIAL;

        

        [Header("チュートリアルUI")]
        [SerializeField]
        Tutorial tutorial_ = null;

        [Header("タイマー")]
        [SerializeField]
        Timer timer_ = null;

        ApplicationManager applicationManager_;



        // Start is called before the first frame update
        void Start()
        {
            state_ = GAME_SCENE_STATE.TUTORIAL;
            Time.timeScale = 0f;
            tutorial_.transform.root.gameObject.SetActive(true);
            timer_.transform.root.gameObject.SetActive(false);

            applicationManager_ = FindObjectOfType<ApplicationManager>();
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
                    StasrtCountDownUpdate();
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
                Time.timeScale = 1.0f;
                tutorial_.transform.root.gameObject.SetActive(false);
                timer_.transform.root.gameObject.SetActive(true);
                state_ = GAME_SCENE_STATE.START_COUNT_DOWN;
            }
        }

        void StasrtCountDownUpdate()
        {
            state_ = GAME_SCENE_STATE.PLAY;
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
            }
        }

        void TimeUpUpdate()
        {
            state_ = GAME_SCENE_STATE.TRANSITION;
        }

        void TransitionUpdate()
        {
            state_ = GAME_SCENE_STATE.FINISH;
            SceneManager.Instance.SceneChange(SceneName.RESULT_SCENE, 1.0f, Color.black);

        }
    }
}