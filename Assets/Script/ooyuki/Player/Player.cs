
using FrontPerson.Vitamin;
using System.Runtime.InteropServices;
using UnityEngine;


namespace FrontPerson.Character
{
    public class Player : MonoBehaviour
    {
        [Header("移動速度")]
        [SerializeField, Range(0.1f, 10.0f)]
        float moveSpeed_ = 5.0f;

        [Header("視点感度")]
        [SerializeField, Range(1, 14)]
        int rotationSpeed_ = 7;

        [Header("体力")]
        [SerializeField, Range(1, 100)]
        int hp_ = 10;

        [Header("所持金")]
        [SerializeField, Range(0, 10000)]
        int money_ = 1000;

        /// <summary>
        /// カメラのトランスフォーム
        /// </summary>
        Transform cameraTransform_ = null;

        /// <summary>
        /// 銃
        /// </summary>
        Gun.Gun gun_ = null;

        /// <summary>
        /// ビタミンBの残弾数
        /// </summary>
        public int vitaminB_ = 0;

        /// <summary>
        /// ビタミンCの残弾数
        /// </summary>
        public int vitaminC_ = 0;

        /// <summary>
        /// ビタミンDの残弾数
        /// </summary>
        public int vitaminD_ = 0;


        /// <summary>
        /// 座標更新用
        /// </summary>
        Vector3 position_;


        /// <summary>
        /// プレイヤーのポジション
        /// </summary>
        Vector3 Position { get { return position_; } }



        // Start is called before the first frame update
        void Start()
        {
            cameraTransform_ = Camera.main.transform;
            gun_ = GetComponentInChildren<FrontPerson.Gun.Gun>();

            position_ = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            Move();
            Shot();

            transform.position = position_;
        }

        private void Move()
        {
            Vector3 direction = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) direction += transform.forward;
            if (Input.GetKey(KeyCode.S)) direction -= transform.forward;
            if (Input.GetKey(KeyCode.A)) direction -= transform.right;
            if (Input.GetKey(KeyCode.D)) direction += transform.right;

            float X_Rotation = Input.GetAxis("Mouse X") * rotationSpeed_ * 30 * Time.deltaTime;
            float Y_Rotation = Input.GetAxis("Mouse Y") * rotationSpeed_ * 30 * Time.deltaTime;

            transform.Rotate(0, X_Rotation, 0);
            cameraTransform_.Rotate(-Y_Rotation, 0, 0);

            position_ += direction * moveSpeed_ * Time.deltaTime;
        }


        void Shot()
        {
            if (!Input.GetKey(KeyCode.Mouse0)) return;

            gun_.Shot();
        }

        void Reload(VITAMIN_TYPE vitaminType)
        {
            if (!Input.GetKey(KeyCode.R)) return;

            switch (vitaminType)
            {
                case VITAMIN_TYPE.VITAMIN_B:
                    vitaminB_++;
                    break;

                case VITAMIN_TYPE.VITAMIN_C:
                    vitaminC_++;
                    break;

                case VITAMIN_TYPE.VITAMIN_D:
                    vitaminD_++;
                    break;
            }
        }


        private void OnTriggerStay(Collider other)
        {
            if (other.tag != Constants.TagName.RECOVERY_POINT) return;

            Reload(other.GetComponent<VitaminRecoveryPoint>().vitaminType);
        }
    }

};
