using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Manager
{
    public class GameSceneController : MonoBehaviour
    {
        enum GAME_SCENE_STATE
        {
            TUTORIAL = 0,
            PLAY,
            TIME_UP,
            TRANSITION
        }
        GAME_SCENE_STATE state_ = GAME_SCENE_STATE.TUTORIAL;





        // Start is called before the first frame update
        void Start()
        {
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