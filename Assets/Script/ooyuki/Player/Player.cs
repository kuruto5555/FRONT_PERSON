using FrontPerson.Constants;
using System.Runtime.InteropServices;
using UnityEngine;
using FrontPerson.Gimmick;
using FrontPerson.Manager;


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
        [SerializeField, Range(0.1f, 10.0f)]
        float jumpPower = 2.0f;

        [Header("ジャンプでかかる重力")]
        [SerializeField, Range(1.0f, 50.0f)]
        float jumpGravity = 5.0f;

        [Header("スタンの時間")]
        [SerializeField, Range(0.5f, 5.0f)]
        float stunTime = 1.0f;

        [Header("無敵時間")]
        [SerializeField, Range(0.5f, 5.0f)]
        float invincibleTime = 1.0f;

        [Header("視点感度")]
        [SerializeField, Range(1, 14)]
        int rotationSpeed_ = 7;

        [Header("視点角度制限")]
        [SerializeField, Range(30.0f, 90.0f)]
        float limitVerticalAngle = 89.0f;

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
        bool _isStun = false;

        /// <summary>
        /// 無敵かどうか
        /// </summary>
        bool _isInvincible = false;

        /// <summary>
        /// サーチ中かどうか
        /// </summary>
        bool isSearch_ = false;

        /// <summary>
        /// ジャンプしてるかどうか
        /// </summary>
        bool _isJump = false;

        /// <summary>
        /// スペシャルウェポンを持っているか
        /// </summary>
        bool _isSpecialWeapon = false;

        /// <summary>
        /// ジャンプの余力
        /// </summary>
        float _jumpForce = 0.0f;

        /// <summary>
        /// プレイヤーの着地する床の高さ
        /// </summary>
        float _nowGrandHeigh = 1.0f;

        /// <summary>
        /// 首の縦の動きを反映させるためのvector3
        /// </summary>
        Vector3 _xAxiz;

        /// <summary>
        /// 下のY軸限界値
        /// </summary>
        float _limitUnderHeight = -256.0f;

        /// <summary>
        /// 今のスタンタイム
        /// </summary>
        float _nowStunTime = 0.0f;

        /// <summary>
        /// 今の無敵時間
        /// </summary>
        float _nowInvincibleTime = 0.0f;

        /// <summary>
        /// バウンティマネージャー
        /// </summary>
        private BountyManager _bountyManager = null;


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
        /// 左の銃の最大弾薬数
        /// </summary>
        public int GunAmmoMAX_L { get { return gunL_.MaxAmmo_; } }

        /// <summary>
        /// 右の銃の残弾数
        /// </summary>
        public int GunAmmoR { get { return gunR_.Ammo; } }

        /// <summary>
        /// 右の銃の最大弾薬数
        /// </summary>
        public int GunAmmoMAX_R { get { return gunR_.MaxAmmo_; } }

        /// <summary>
        /// 走っているかどうか
        /// </summary>
        public bool IsDash { get { return moveSpeed_ == runSpeed_; } }

        /// <summary>
        /// 歩いているかどうか
        /// </summary>
        public bool IsWalk { get { return moveSpeed_ == walkSpeed_; } }

        /// <summary>
        /// 止まっているかどうか
        /// </summary>
        public bool IsStop { get { return moveSpeed_ == 0f; } }

        /// <summary>
        /// ジャンプしているかどうか
        /// </summary>
        public bool IsJump { get { return _isJump; } }

        /// <summary>
        /// 空中にいるかどうか
        /// </summary>
        public bool IsAir { get { return position_.y > _nowGrandHeigh; } }

        /// <summary>
        /// 気絶してるかどうか
        /// </summary>
        public bool IsStun { get { return _isStun; } }

        /// <summary>
        /// 無敵かどうか
        /// </summary>
        public bool IsInvincible { get { return _isInvincible; } }

        public bool IsSpecialWeapon { get { return _isSpecialWeapon; } }


        // Start is called before the first frame update
        void Start()
        {
            cameraTransform_ = Camera.main.transform;
            //gunL_ = GetComponentInChildren<Gun>();
            //gunR_ = GetComponentInChildren<Gun>();

            position_ = transform.position;

            //初期角度を取得して置く
            _xAxiz = cameraTransform_.localEulerAngles;

            _bountyManager = GameObject.FindGameObjectWithTag("BountyManager").GetComponent<BountyManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_isStun)
            {
                StunStatus();

                return;
            }

            if (Input.GetKeyDown(KeyCode.J)) Stun();

            position_ = transform.position;

            InvincibleStatus();
            ViewPointMove();
            Search();
            Dash();
            Move();
            Shot();
            Jump();

            transform.position = position_;
        }

        /// <summary>
        /// 視点移動
        /// </summary>
        private void ViewPointMove()
        {
            float X_Rotation = Input.GetAxis("Mouse X") * rotationSpeed_ * 30 * Time.deltaTime;
            float Y_Rotation = Input.GetAxis("Mouse Y") * rotationSpeed_ * 30 * Time.deltaTime;

            transform.Rotate(0, X_Rotation, 0);

            var x = _xAxiz.x - Y_Rotation;

            //角度検証
            if (x >= -limitVerticalAngle && x <= limitVerticalAngle)
            {
                //問題無ければ反映
                _xAxiz.x = x;
                cameraTransform_.localEulerAngles = _xAxiz;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Move()
        {
            Vector3 direction = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) direction += transform.forward;
            if (Input.GetKey(KeyCode.S)) direction -= transform.forward;
            if (Input.GetKey(KeyCode.A)) direction -= transform.right;
            if (Input.GetKey(KeyCode.D)) direction += transform.right;

            

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
             _nowGrandHeigh = LandingHeight(position_, 12);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(!IsJump) //ジャンプが始まる瞬間
                {
                    _isJump = true;
                    _jumpForce = jumpPower;
                    position_ += transform.up * _jumpForce * Time.deltaTime;
                }
            }

            //空中にいるとき
            if (IsAir)
            {
                position_ += transform.up * _jumpForce * Time.deltaTime;
                _jumpForce -= jumpGravity * Time.deltaTime;
                _isJump = true;
            }

            //地面にいるとき
            else
            {
                //着地後力が残ってしまうので初期化
                _jumpForce = 0.0f;

                //ジャンプしてない時地面にくっつける
                position_ = new Vector3(position_.x, _nowGrandHeigh, position_.z);

                _isJump = false;
            }
        }
 
        /// <summary>
        /// 地面の高さを確認する
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="Hitlayer">取得したいobjectLayer</param>
        /// <returns>posの真下objectの高さを返す</returns>
        private float LandingHeight(Vector3 pos, int Hitlayer)
        {
            RaycastHit hit;
            Vector3 startPos;
            Vector3 endPos;

            startPos = pos - transform.up * 0.5f;
            endPos = pos + transform.up * 0.5f;
            int layerMask = 1 << 12;
            if (Physics.Raycast(pos, Vector3.down, out hit, 10.0f, layerMask))
            {
                if (LayerNumber.ENEMY != hit.collider.gameObject.layer)
                    return hit.point.y + 1.0f;
            }
            else
            {
                //もし下にすり抜けて落ちたら上空に沸く
                if (pos.y < _limitUnderHeight + 2.0f) return 100.0f;
            }

            return _limitUnderHeight + 1.5f;
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
            if (IsDash) return;
            if (isSearch_) return;

            if (!Input.GetKeyDown(KeyCode.R)) return;

            gunL_.Reload();
            gunR_.Reload();
        }


        /// <summary>
        /// 弾の補充（野生のフルーツ）
        /// </summary>
        /// <param name="vitaminType">補充するビタミンの種類</param>
        /// <param name="value">補給量</param>
        void Reload(NutrientsRecoveryPoint vrp)
        {
            if (!Input.GetKeyDown(KeyCode.R)) return;

            switch (vrp.VitaminType)
            {
                case NUTRIENTS_TYPE._A:
                    gunL_.Reload(vrp.Charge(GunAmmoMAX_L - GunAmmoL));
                    break;

                case NUTRIENTS_TYPE._B:
                    gunR_.Reload(vrp.Charge(GunAmmoMAX_R - GunAmmoR));
                    break;

                case NUTRIENTS_TYPE._ALL:
                    gunL_.Reload();
                    gunR_.Reload();
                    break;
            }
        }


        /// <summary>
        /// 敵の不足ビタミンを知るエリアを展開
        /// </summary>
        void Search()
        {
            if (IsJump) return;
            if (IsDash) return;

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

        /// <summary>
        /// スタン中の処理
        /// </summary>
        void StunStatus()
        {
            if (!_isStun) return;

            if(stunTime > _nowStunTime)
            {
                _nowStunTime += Time.deltaTime;
            }
            else
            {
                _nowStunTime = 0.0f;

                _isStun = false;

                //無敵開始
                _isInvincible = true;
            }
            
        }

        /// <summary>
        /// スタンさせる
        /// </summary>
        public void Stun()
        {
            _isStun = true;
            _bountyManager.PlayerDamage();
        }

        /// <summary>
        /// 無敵中の処理
        /// </summary>
        void InvincibleStatus()
        {
            //無敵ではなかったら終わり
            if (!_isInvincible) return;

            //無敵タイム中
            if(_nowInvincibleTime < invincibleTime)
            {
                _nowInvincibleTime += Time.deltaTime;
            }
            else //無敵終了
            {
                _nowInvincibleTime = 0.0f;
                _isInvincible = false;
            }
        }

        /// <summary>
        /// 無敵にする時呼ぶ
        /// </summary>
        /// <param name="time"></param>
        public void SetInvincible(float time)
        {
            _isInvincible = true;
        }

        private void OnTriggerStay(Collider other)
        {
            switch (other.tag)
            {
                case TagName.RECOVERY_POINT:
                    Reload(other.GetComponent<NutrientsRecoveryPoint>());
                    break;

                case TagName.ENEMY:

                    break;
            }
        }
    }

};
