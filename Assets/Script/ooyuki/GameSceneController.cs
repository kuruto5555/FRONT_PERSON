using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.UI;

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
            TRANSITION
        }
        GAME_SCENE_STATE state_ = GAME_SCENE_STATE.TUTORIAL;


        [Header("チュートリアルUI")]
        [SerializeField]
        Tutorial tutorial_ = null;

        [Header("タイマー")]
        [SerializeField]
        Timer timer_ = null;



        // Start is called before the first frame update
        void Start()
        {
            state_ = GAME_SCENE_STATE.TUTORIAL;
            Time.timeScale = 0f;
            tutorial_.transform.root.gameObject.SetActive(true);
            timer_.transform.root.gameObject.SetActive(false);
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
            }
        }

        void StasrtCountDownUpdate()
        {

        }

        void PlayUpdate()
        {

        }

        void TimeUpUpdate()
        {

        }

        void TransitionUpdate()
        {

        }
    }
}