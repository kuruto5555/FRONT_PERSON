using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Constants;
using FrontPerson.Manager;


namespace FrontPerson
{
    public class SoundPlayTest : MonoBehaviour
    {
       

        // Start is called before the first frame update
        void Start()
        {
          
        }

        // Update is called once per frame
        void Update()
        {
            //b.Play3DSound(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
            //b.Play3DSound(gameObject, SEPath.COMMON_SE_BACK);



            if (Input.GetKeyDown(KeyCode.W))
            {
                AudioManager.Instance.Play2DSound(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSound(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSound(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSound(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSound(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);

            }

            if (Input.GetKeyDown(KeyCode.A))
            {

                AudioManager.Instance.Play2DSound(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);




            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                AudioManager.Instance.Play2DSound(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);



            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                AudioManager.Instance.Play2DSound(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);



            }
        }
    }
}