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
            //AudioManager.Instance.PauseBGM();
        }

        // Update is called once per frame
        void Update()
        {
            //AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);

#if UNITY_EDITOR

            if (Input.GetKey(KeyCode.H))
            {
                //AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.GAME_SE_ENEMY_HEALTH_2);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_BACK);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_BACK);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_BACK);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_BACK);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_BACK);
                AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_BACK);

            }


            if (Input.GetKeyDown(KeyCode.Y))
            {
                AudioManager.Instance.Play3DSE(gameObject.transform.position, SEPath.GAME_SE_ENEMY_HEALTH_2);
            }
#endif

        }
    }
}