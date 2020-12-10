using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson
{
    public class BillBoard : MonoBehaviour
    {
        /// <summary>
        /// カメラ
        /// </summary>
        private Camera targetCamera = null;

        /// <summary>
        /// 裏表反対になることを直す為の変数
        /// </summary>
        [Header("必要な時にしか設定しないで下さい")]
        [SerializeField]
        private Quaternion rotate = Quaternion.Euler(0.0f, 180.0f, 0.0f);

        /// <summary>
        /// 2フレーム毎に更新させる為の変数
        /// </summary>
        bool enable = true;

#if UNITY_EDITOR
        void Start()
        {
            if(null == targetCamera)
            {
                targetCamera = GameObject.FindGameObjectWithTag(Constants.TagName.MAIN_CAMERA).GetComponent<Camera>();
            }
        }
#endif

        void Update()
        {
            if (enable)
            {
                Vector3 target = targetCamera.transform.position;
                target.y = transform.position.y;
                transform.LookAt(target);

                transform.rotation *= rotate;

                enable = !enable;
            }
            else
            {
                enable = !enable;
            }
        }
    }
}