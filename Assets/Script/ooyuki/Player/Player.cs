
using FrontPerson.Vitamin;
using System.Runtime.InteropServices;
using UnityEngine;


namespace FrontPerson.Character
{
    public class Player : MonoBehaviour
    {
        [Header("歩き速度")]
        [SerializeField, Range(0.1f, 10.0f)]
        float walkSpeed_ = 5.0f;

        [Header("走り速度")]
        [SerializeField, Range(0.1f, 20.0f)]
        float runSpeed_ = 8.0f;

        [Header("ジャンプ高度")]
        [SerializeField, Range(0.1f, 5.0f)]
        float jumpPower = 2.0f;

        [Header("視点感度")]
        [SerializeField, Range(1, 14)]
        int rotationSpeed_ = 7;

        [Header("銃")]
        [SerializeField]
        Weapon.Gun gunR_ = null;
        [SerializeField]
        Weapon.Gun gunL_ = null;

        [Header("サーチエリア")]
        [SerializeField]
        Skill.SearchArea searchArea = null;



        /// <summary>
        /// カメラのトランスフォーム
        /// </summary>
        Transform cameraTransform_ = null;

        /// <summary>
        /// 移動速度
        /// </summary>
        float moveSpeed_ = 1.0f;

        /// <summary>
        /// 座標更新用
        /// </summary>
        Vector3 position_;

        /// <summary>
        /// ぴよってるかどうか
        /// </summary>
        bool isPiyori_ = false;

        /// <summary>
        /// サーチ中かどうか
        /// </summary>
        bool isSearch_ = false;



        /*---- プロパティ ----*/
        /// <summary>
        /// プレイヤーのポジション
        /// </summary>
        public Vector3 Position { get { return position_; } }

        /// <summary>
        /// 左の銃の残弾数
        /// </summary>
        public int GunAmmoL { get { return gunL_.Ammo; } }

        /// <summary>
        /// 右の銃の残弾数
        /// </summary>
        public int GunAmmoR { get { return gunR_.Ammo; } }

        /// <summary>
        /// 走っているかどうか
        /// </summary>
        public bool IsDash { get { return moveSpeed_ == runSpeed_; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsJump { get { return position_.y > 1f; } }


        // Start is called before the first frame update
        void Start()
        {
            cameraTransform_ = Camera.main.transform;
            //gunL_ = GetComponentInChildren<Gun>();
            //gunR_ = GetComponentInChildren<Gun>();

            position_ = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            Search();
            Dash();
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

        /// <summary>
        /// 左シフトが押されていたら移動速度を走る速度に設定
        /// 押されていなかったら歩く速度に設定
        /// </summary>
        void Dash()
        {
            if (IsJump) return;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed_ = runSpeed_;
            }
            else
            {
                moveSpeed_ = walkSpeed_;
            }
        }

        /// <summary>
        /// ジャンプ
        /// </summary>
        void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

            }
        }

        /// <summary>
        /// 攻撃
        /// </summary>
        void Shot()
        {
            if (IsDash) return;
            if (isSearch_) return;

            if (Input.GetKey(KeyCode.Mouse0))
            {
                gunL_.Shot();
            }

            if (Input.GetKey(KeyCode.Mouse1))
            {
                gunR_.Shot();
            }
        }

        /// <summary>
        /// 弾の補充（補給所）
        /// </summary>
        void Reload()
        {
            if (!Input.GetKeyDown(KeyCode.R)) return;

            gunL_.Reload();
            gunR_.Reload();
        }


        /// <summary>
        /// 弾の補充（野生のフルーツ）
        /// </summary>
        /// <param name="vitaminType">補充するビタミンの種類</param>
        /// <param name="value">補給量</param>
        void Reload(VITAMIN_TYPE vitaminType, int value)
        {
            if (!Input.GetKeyDown(KeyCode.R)) return;

            if(vitaminType == VITAMIN_TYPE.VITAMIN_C)
            {
                gunL_.Reload(value);
            }
            else if(vitaminType == VITAMIN_TYPE.VITAMIN_D)
            {
                gunR_.Reload(value);
            }
        }


        /// <summary>
        /// 敵の不足ビタミンを知るエリアを展開
        /// </summary>
        void Search()
        {
            if (!Input.GetKeyDown(KeyCode.E)) return;
                
            if(isSearch_ == false)
            {
                isSearch_ = true;
                searchArea.Search();
            }
            else
            {
                isSearch_ = false;
                searchArea.Stop();
            }


        }



        private void OnTriggerStay(Collider other)
        {
            if (other.tag != Constants.TagName.RECOVERY_POINT) return;

            Reload();
        }
    }

};
