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
            //AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);

            if (Input.GetKey(KeyCode.A))
            {
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
            }


            if (Input.GetKeyDown(KeyCode.W))
            {
                AudioManager.Instance.Play3DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
            }


        }
    }
}