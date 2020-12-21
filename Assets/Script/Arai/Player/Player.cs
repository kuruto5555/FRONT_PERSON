using FrontPerson.Constants;
using System.Runtime.InteropServices;
using UnityEngine;
using FrontPerson.Gimmick;
using FrontPerson.Manager;
using FrontPerson.Weapon;
using System.Collections.Generic;
using System;


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
        int RotationSpeed_ = 7;

        [Header("視点角度制限")]
        [SerializeField, Range(30.0f, 90.0f)]
        float LimitVerticalAngle_ = 89.0f;

        [Header("銃")]
        [SerializeField]
        Weapon.Gun gunR_ = null;
        [SerializeField]
        Weapon.Gun gunL_ = null;

        [Header("サーチエリア")]
        [SerializeField]
        Skill.SearchArea searchArea = null;

        [Header("足音鳴らす間隔")]
        [SerializeField, Range(0.1f, 1.0f)]
        float DashSoundRate = 1.0f;

        [Header("透明中画面エフェクト")]
        [SerializeField] 
        GameObject InvisibleEffect_ = null;

        /// <summary>
        /// スペシャル武器
        /// </summary>
        Weapon.SpecialWeapon Weapon = null;

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
        /// 透明化どうか
        /// </summary>
        bool _isTransparent = false;

        /// <summary>
        /// サーチ中かどうか
        /// </summary>
        bool isSearch_ = false;

        /// <summary>
        /// ジャンプしてるかどうか
        /// </summary>
        bool _isJump = false;

        /// <summary>
        /// 右指トリガー
        /// </summary>
        bool _isFireRHand = false;

        /// <summary>
        /// 左指トリガー
        /// </summary>
        bool _isFireLHand = false;

        /// <summary>
        /// 止まっているかどうか
        /// </summary>
        bool _isStop = false;

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

        /// <summary>
        /// ウェポンマネージャー
        /// </summary>
        private SpecialWeaponManager _weponManager = null;

        /// <summary>
        /// コンボマネージャー参照
        /// </summary>
        private ComboManager _comboManager = null;

        /// <summary>
        /// 視点感度変数
        /// </summary>
        int _viewRotetaSpeed = 0;

        /// <summary>
        /// 武器切り替えのアニメーションが再生されてるかどうか
        /// </summary>
        bool _isWeaponChangeAnimation = false;

        /// <summary>
        /// アイテムフラグ
        /// </summary>
        ITEM_STATUS _itemStatusFlag = ITEM_STATUS.NORMAL;

        /// <summary>
        /// スピードアップアイテムの倍率
        /// </summary>
        float _addSpeed = 1.0f;

        /// <summary>
        /// スピードアップアイテムの時間
        /// </summary>
        float _movementSpeedUpTime = 0.0f;

        /// <summary>
        /// 透明化アイテムの時間
        /// </summary>
        float _transparentItemTime = 0.0f;

        /// <summary>
        /// 足音を鳴らす間隔
        /// </summary>
        float _nowDashSoundRate = 0.0f;

        /// <summary>
        /// オーディオマネージャー参照
        /// </summary>
        AudioManager _audioManager = null;

        /// <summary>
        /// 透明化中のエフェクトの参照
        /// </summary>
        GameObject _invisibleObject = null;

        /// <summary>
        /// UI用キャンバス参照
        /// </summary>
        Transform _canvas = null;

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
        public bool IsStop { get { return _isStop; } }

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

        public bool IsTransparent { get { return _isTransparent; } }

        /// <summary>
        /// スペシャル武器を持っているかどうか
        /// </summary>
        public bool IsSpecialWeapon { get { return Weapon != null; } }

        public bool IsLeftTrigger { get { return _isFireLHand; } }

        public bool IsRightTrigger { get { return _isFireRHand; } }

        /// <summary>
        /// アプリケーションマネージャー参照
        /// </summary>
        private ApplicationManager _appManager = null;

        /// <summary>
        /// 所持してる武器一覧
        /// </summary>
        private List<Gun> _weaponList;

        /// <summary>
        /// 入手した武器の番号
        /// </summary>
        private int _weaponType = 0;

        /// <summary>
        /// 所持してる武器のList
        /// 1左拳銃,2右拳銃,3持ってれば特殊武器
        /// </summary>
        public List<Gun> GetWeaponList { get { return _weaponList; } }

        /// <summary>
        /// 視点感度取得関数
        /// </summary>
        /// <returns></returns>
        public int GetViewRotateSpeed()
        {
            return _viewRotetaSpeed;
        }

        /// <summary>
        /// 視点感度代入関数
        /// </summary>
        /// <param name="speed"></param>
        public void SetViewRotateSpeed(int speed)
        {
            _viewRotetaSpeed = speed;
        }

        // Start is called before the first frame update
        void Start()
        {
            cameraTransform_ = Camera.main.transform;
            //gunL_ = GetComponentInChildren<Gun>();
            //gunR_ = GetComponentInChildren<Gun>();

            position_ = transform.position;

            moveSpeed_ = walkSpeed_;

            //初期角度を取得して置く
            _xAxiz = cameraTransform_.localEulerAngles;

            _bountyManager = BountyManager._instance;

            _weponManager = SpecialWeaponManager._instance;

            _viewRotetaSpeed = RotationSpeed_;

            _weaponList = new List<Gun>();

            _weaponList.Add(gunL_);
            _weaponList.Add(gunR_);
            _weaponList.Add(null);

            _appManager = GameObject.FindGameObjectWithTag(TagName.MANAGER).GetComponent<ApplicationManager>();
            if (_appManager == null) Debug.Log("GameSceneController");

            _nowDashSoundRate = 1.0f;

            _audioManager = AudioManager.Instance;

            _canvas = GameObject.Find("GameUI_Canvas").transform;

            _comboManager = ComboManager.Instance;
        }

        // Update is called once per frame
        void Update()
        {
            if (!_appManager.IsGamePlay) return;

            _isFireLHand = _isFireRHand = false;

            if (_isStun)
            {
                StunStatus();

                return;
            }

            position_ = transform.position;
            DebugUpdeta();
            ItemStatusUpdate();
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
            float Y_Rotation = Input.GetAxis(Constants.InputName.VERTICAL2) * RotationSpeed_ * 30 * Time.deltaTime;
            float X_Rotation = Input.GetAxis(Constants.InputName.HORIZONTAL2) * RotationSpeed_ * 30 * Time.deltaTime;
            
            transform.Rotate(0, X_Rotation, 0);

            var x = _xAxiz.x - Y_Rotation;

            //角度検証
            if (x >= -LimitVerticalAngle_ && x <= LimitVerticalAngle_)
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

            direction += Input.GetAxisRaw(Constants.InputName.HORIZONTAL) * transform.right;
            direction += Input.GetAxisRaw(Constants.InputName.VERTICAL) * transform.forward;

            position_ += direction.normalized * moveSpeed_* _addSpeed * Time.deltaTime;

            //移動中だけ音を鳴らす
            if(direction == Vector3.zero)
            {
                _isStop = true;
            }
            else
            {
                _isStop = false;
            }
        }

        /// <summary>
        /// 左シフトが押されていたら移動速度を走る速度に設定
        /// 押されていなかったら歩く速度に設定
        /// </summary>
        void Dash()
        {
            if (IsJump) return;
            if (IsStop)
            {
                moveSpeed_ = walkSpeed_;
                return;
            }


            if (Input.GetButton(Constants.InputName.DASH))
            {
                moveSpeed_ = runSpeed_;

                _nowDashSoundRate += Time.deltaTime;

                if (_nowDashSoundRate > DashSoundRate)
                {
                    _audioManager.Play3DSE(position_, SEPath.GAME_SE_DASH);
                    _nowDashSoundRate = 0;
                }
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

            if (Input.GetButtonDown(Constants.InputName.JUMP))
            {
                if(!IsJump) //ジャンプが始まる瞬間
                {
                    _isJump = true;
                    _jumpForce = jumpPower;
                    position_ += transform.up * _jumpForce * Time.deltaTime;

                    _audioManager.Play3DSE(Position, SEPath.GAME_SE_JUMP);
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
            if (_isWeaponChangeAnimation) return;

            //左クリック
            if (Input.GetButton(Constants.InputName.FIRE1)
                || 0 < Input.GetAxis(Constants.InputName.FIRE1))
            {
                if (IsSpecialWeapon)
                {
                    _isFireLHand = true;
                }
                else
                {
                    gunL_.Shot();
                    
                }
            }

            //右クリック
            if (Input.GetButton(Constants.InputName.FIRE2)
                || 0 < Input.GetAxis(Constants.InputName.FIRE2))
            {
                if (IsSpecialWeapon)
                {
                    _isFireRHand = true;
                }
                else
                {
                    gunR_.Shot();
                    
                }            
            }

            //二重でshotを呼ばない為にフラグを使う
            if (_isFireLHand || _isFireRHand)
            {
                Weapon.Shot();
            }
        }

        /// <summary>
        /// 弾の補充（補給所）
        /// </summary>
        void Reload()
        {
            if (IsDash) return;
            if (isSearch_) return;

            if (!Input.GetButton(Constants.InputName.RELOAD)) return;

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
            if (!Input.GetButtonDown(Constants.InputName.RELOAD)) return;
            if (IsDash) return;
            if (isSearch_) return;
            if (IsSpecialWeapon) return;

            switch (vrp.VitaminType)
            {
                case NUTRIENTS_TYPE._A:
                    gunL_.Reload(vrp.Charge(GunAmmoMAX_L - GunAmmoL));
                    break;

                case NUTRIENTS_TYPE._B:
                    gunR_.Reload(vrp.Charge(GunAmmoMAX_R - GunAmmoR));
                    break;

                case NUTRIENTS_TYPE._ALL:
                    gunL_.Reload(vrp.Charge(GunAmmoMAX_L - GunAmmoL));
                    gunR_.Reload(vrp.Charge(GunAmmoMAX_R - GunAmmoR));
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

            if (!Input.GetButtonDown(Constants.InputName.SCAN)) return;
                
            if(isSearch_ == false)
            {
                isSearch_ = true;
                searchArea.Search();
                _audioManager.Play3DSE(Position, SEPath.GAME_SE_SCAN);
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

            //スタン中だったら
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

                //ぴよぴよ音止める処理欲しい
            }
            
        }

        /// <summary>
        /// スタンさせる
        /// </summary>
        public void Stun()
        {
            if (_itemStatusFlag.HasFlag(ITEM_STATUS.INVICIBLE)) return;
            _isStun = true;
            _bountyManager.PlayerDamage();
            _audioManager.Play3DSE(Position, SEPath.GAME_SE_STUN);
            _audioManager.Play3DSE(Position, SEPath.GAME_SE_DAMEGE);
            _comboManager.LostCombo();

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
        /// 毎フレームスペシャル武器の状態をチェックする
        /// </summary>
        private void WeaponStatus()
        {
            if (Weapon == null) return;
            if (Weapon.IsAnimation) return;

            if (Weapon.Ammo <= 0 )
            {
                //武器のアニメーションスタート
                //Weapon.
            }
            
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

        /// <summary>
        /// 武器を入手する時に呼ぶ
        /// WeaponManagerの引数番目を生成して持たせる
        /// </summary>
        /// <param name="type">乱数</param>
        public void WeaponUpgrade(int type)
        {
            gunL_.Reload();
            gunR_.Reload();

            if (IsSpecialWeapon)
            {
                Weapon.WeaponForcedChange();
                //武器チェンジアニメーションスタート
            }
            else
            {
                //ハンドガンを下に下げるアニメーションを呼ぶ
                //WeaponChangeAnimationStart();
            }

            _weaponType = type;

            //下２行アニメーションが出来次第消す
            Weapon = Instantiate(_weponManager.WeaponPrefabList[type], cameraTransform_).GetComponent<Weapon.SpecialWeapon>();
            _weaponList[2] = Weapon;
        }

        /// <summary>
        /// 武器チェンジアニメーションが終わった時に呼ぶ
        /// </summary>
        public void SetWeapon()
        {
            Weapon = Instantiate(_weponManager.WeaponPrefabList[_weaponType], cameraTransform_).GetComponent<Weapon.SpecialWeapon>();
            _weaponList[2] = Weapon;
        }

        private void ItemStatusUpdate()
        {
            if (_itemStatusFlag == ITEM_STATUS.NORMAL)
            {
                return;
            }
            else
            {
                AddMovementSpeedItemUpdate();
                TransparentItemUpdate();
            }

            
        }

        private void AddMovementSpeedItemUpdate()
        {
            if (!_itemStatusFlag.HasFlag(ITEM_STATUS.SPEED_UP)) return;

            _movementSpeedUpTime -= Time.deltaTime;

            if (_movementSpeedUpTime < 0)
            {
                _itemStatusFlag &= ~ITEM_STATUS.SPEED_UP; //解除
                _addSpeed = 1.0f; //等倍に戻す
            }
        }

        private void TransparentItemUpdate()
        {
            if (!_itemStatusFlag.HasFlag(ITEM_STATUS.INVICIBLE)) return;

            _transparentItemTime -= Time.deltaTime;

            if(_transparentItemTime < 0)
            {
                _itemStatusFlag &= ~ITEM_STATUS.INVICIBLE; //解除
                _isTransparent = false;
                Destroy(_invisibleObject);
            }
        }

        /// <summary>
        /// 武器のアニメーションが始まったとき呼ぶ関数
        /// </summary>
        public void WeaponChangeAnimationStart()
        {
            _isWeaponChangeAnimation = true;
        }

        /// <summary>
        /// 武器のアニメーションが終わったとき呼ぶ関数
        /// </summary>
        public void WeaponChangeAnimationFinish()
        {
            _isWeaponChangeAnimation = false;
        }

        public void PickUpItem(ITEM_STATUS type, int time, float value)
        {
            switch (type)
            {
                case ITEM_STATUS.INVICIBLE:
        
                    break;
        
                case ITEM_STATUS.FEVER:
                    break;
        
                case ITEM_STATUS.SPEED_UP:
                    break;
            }
        }

        /// <summary>
        /// 速度上昇アイテムを取得した時呼ぶ
        /// </summary>
        /// <param name="time">効果時間</param>
        /// <param name="value">効果倍率</param>
        public void PickUpMovementSpeedItem(float time, float value)
        {
            _movementSpeedUpTime = time;
            _addSpeed = value;

            //flagを立てる
            _itemStatusFlag |= ITEM_STATUS.SPEED_UP;
        }

        /// <summary>
        /// 透明化アイテムを取得した時呼ぶ
        /// </summary>
        public void PickUpTransparent(float time)
        {
            _transparentItemTime = time;

            _isTransparent = true;

            _itemStatusFlag |= ITEM_STATUS.INVICIBLE;

            if (_invisibleObject == null)
            {
                _invisibleObject = Instantiate(InvisibleEffect_, _canvas.transform);
                _invisibleObject.transform.SetAsFirstSibling();
            }
        }

        private void DebugUpdeta()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.J)) Stun();
            if (Input.GetKeyDown(KeyCode.L)) 
            {
                PickUpMovementSpeedItem(10.0f, 10.5f);
                
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                PickUpTransparent(10.0f);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) WeaponUpgrade(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) WeaponUpgrade(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) WeaponUpgrade(2);

#endif
        }

    }

};
