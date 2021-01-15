using FrontPerson.Constants;
using System.Runtime.InteropServices;
using UnityEngine;
using FrontPerson.Gimmick;
using FrontPerson.Manager;
using FrontPerson.Weapon;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Linq;

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
        [Tooltip("上下の視点感度")]
        static int verticalRotationSpeed_ = 5;
        [SerializeField, Range(1, 14)]
        [Tooltip("左右の視点感度")]
        static int horizontalRotationSpeed_ = 5;


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

        [Header("スタンエフェクト")]
        [SerializeField]
        GameObject StunEffect_ = null;

        [Header("スタン音鳴らす間隔")]
        [SerializeField, Range(0.0f, 1.0f)]
        float StunSoundRate = 0.5f;

        [Header("追いかけられているエフェクト")]
        [SerializeField]
        GameObject AlartEffect_ = null;

        private GameObject AlartObj = null;


        /// <summary>
        /// 所持してる武器のList
        /// 1左拳銃,2右拳銃,3持ってれば特殊武器
        /// </summary>
        public List<Gun> WeaponList { get; private set; }

        /// <summary>
        /// スペシャル武器
        /// </summary>
        SpecialWeapon Weapon = null;

        /// <summary>
        /// スペシャル武器のUI
        /// </summary>
        UI.UI_SP_Weapon spWeaponUI_ = null;



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
        bool IsStop = false;

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
        /// スタン音を鳴らす間隔
        /// </summary>
        float _nowStunSoundRate = 0.0f;

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

        List<Animator> _weaponAnims = null;

        /// <summary>
        /// 現在触れている補給ポイント
        /// </summary>
        NutrientsRecoveryPoint nutrientsRecoveryPoint_ = null;

        /// <summary>
        /// アプリケーションマネージャー参照
        /// </summary>
        private ApplicationManager _appManager = null;


        /// <summary>
        /// 入手した武器の番号
        /// </summary>
        private int _weaponType = (int)WEAPON_TYPE.HANDGUN;

        /// <summary>
        /// 生成したスタン
        /// </summary>
        private GameObject _stunEffect = null;

        /// <summary>
        /// おばちゃん、ヤクザに追いかけられている数
        /// </summary>
        private int AlartCnt = 0;

        /// <summary>
        /// 追いかけられているかを表すフラグ(true = 追いかけられている)
        /// </summary>
        public bool isAlart { get; private set; } = false;

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
        /// 空中にいるかどうか
        /// </summary>
        public bool IsAir { get { return position_.y > _nowGrandHeigh; } }

        /// <summary>
        /// ぴよってるかどうか
        /// </summary>
        public bool IsStun { get; private set; } = false;

        /// <summary>
        /// 無敵かどうか
        /// </summary>
        public bool IsInvincible { get; private set; } = false;

        /// <summary>
        /// 透明化どうか
        /// </summary>
        public bool IsTransparent { get; private set; } = false;

        /// <summary>
        /// スペシャル武器を持っているかどうか
        /// </summary>
        public bool IsSpecialWeapon { get { return Weapon != null; } }

        /// <summary>
        /// 左右の視点感度
        /// </summary>
        static public int HorizontalRotetaSpeed { get { return horizontalRotationSpeed_; } set { horizontalRotationSpeed_ = Mathf.Clamp(value, 1, 14); } }


        /// <summary>
        /// 上下の視点感度
        /// </summary>
        static public int VerticalRotetaSpeed { get { return verticalRotationSpeed_; } set { verticalRotationSpeed_ = Mathf.Clamp(value, 1, 14); } }





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

            //_viewRotetaSpeed = RotationSpeed_;

            _weaponAnims = new List<Animator>();
            WeaponList = new List<Gun> { gunL_, gunR_, null };

            _appManager = GameObject.FindGameObjectWithTag(TagName.MANAGER).GetComponent<ApplicationManager>();
            if (_appManager == null) Debug.Log("GameSceneController");

            _audioManager = AudioManager.Instance;

            _canvas = GameObject.Find("GameUI_Canvas").transform;

            spWeaponUI_ = FindObjectOfType<UI.UI_SP_Weapon>();

            _comboManager = ComboManager.Instance;

            _weaponType = (int)WEAPON_TYPE.HANDGUN;
        }

        // Update is called once per frame
        void Update()
        {
            // ゲーム中でないなら更新しない
            if (!_appManager.IsGamePlay) return;

            // フラグ初期化
            _isFireLHand = _isFireRHand = false;

            // 空中にいるときの処理もあるので先に呼ぶ
            position_ = transform.position;
            Jump();

            // スタン中だったら他の処理をせずにスタンの時間を更新して終わり
            if (IsStun)
            {
                StunStatus();
                transform.position = position_;
                return;
            }

            
            DebugUpdeta();
            ItemStatusUpdate();
            InvincibleStatus();
            ViewPointMove();
            Search();
            Dash();
            Move();
            WeaponChange();
            Shot();
            WeaponStatus();

            Reload(nutrientsRecoveryPoint_);

            transform.position = position_;
        }

        private void LateUpdate()
        {
            Alart();
        }

        /// <summary>
        /// 視点移動
        /// </summary>
        private void ViewPointMove()
        {
            float Y_Rotation = Input.GetAxis(Constants.InputName.VERTICAL2)   * VerticalRotetaSpeed * 20 * Time.deltaTime;
            float X_Rotation = Input.GetAxis(Constants.InputName.HORIZONTAL2) * HorizontalRotetaSpeed * 30 * Time.deltaTime;
            
            transform.Rotate(0, X_Rotation, 0);

            var x = _xAxiz.x - Y_Rotation;

            //角度検証
            if (x >= -LimitVerticalAngle_ && x <= LimitVerticalAngle_)
            {
                //問題無ければ反映
                _xAxiz.x = x;
                cameraTransform_.localEulerAngles = _xAxiz;
            }

            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
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
                IsStop = true;
            }
            else
            {
                IsStop = false;
            }
        }

        /// <summary>
        /// 左シフトが押されていたら移動速度を走る速度に設定
        /// 押されていなかったら歩く速度に設定
        /// </summary>
        void Dash()
        {
            if (_isJump) return;
            if (searchArea.IsSearch) return;
            if (IsStop)
            {
                if (IsDash)
                {
                    // 武器にダッシュアニメーションを止めさせる
                    foreach (var weapon in WeaponList)
                    {
                        if (weapon == null) continue;
                        if (!weapon.gameObject.activeSelf) continue;
                        weapon.GetComponent<Animator>()?.SetBool("Dash", false);
                    }
                }
                
                moveSpeed_ = walkSpeed_;

                return;
            }

            if (Input.GetButton(Constants.InputName.DASH))
            {
                if (IsWalk)
                {
                    // 武器にダッシュアニメーションを止めさせる
                    foreach (var weapon in WeaponList)
                    {
                        if (weapon == null) continue;
                        if (!weapon.gameObject.activeSelf) continue;
                        weapon.GetComponent<Animator>()?.SetBool("Dash", true);
                    }
                }

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
                if (IsDash)
                {
                    // 武器にダッシュアニメーションを止めさせる
                    foreach (var weapon in WeaponList)
                    {
                        if (weapon == null) continue;
                        if (!weapon.gameObject.activeSelf) continue;
                        weapon.GetComponent<Animator>()?.SetBool("Dash", false);
                    }
                }

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
                if(!_isJump && !IsStun) //ジャンプが始まる瞬間
                {
                    if (IsDash)
                    {
                        // 武器にダッシュアニメーションを止めさせる
                        foreach (var weapon in WeaponList)
                        {
                            if (weapon == null) continue;
                            if (!weapon.gameObject.activeSelf) continue;
                            weapon.GetComponent<Animator>()?.SetBool("Dash", false);
                        }
                    }

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
                if (IsDash)
                {
                    // 武器にダッシュアニメーションを止めさせる
                    foreach (var weapon in WeaponList)
                    {
                        if (weapon == null) continue;
                        if (!weapon.gameObject.activeSelf) continue;
                        weapon.GetComponent<Animator>()?.SetBool("Dash", true);
                    }
                }

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
            if (searchArea.IsSearch) return;
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
        /// 弾の補充（野生のフルーツ）
        /// </summary>
        /// <param name="vitaminType">補充するビタミンの種類</param>
        /// <param name="value">補給量</param>
        void Reload(NutrientsRecoveryPoint vrp)
        {
            if (!Input.GetButtonDown(Constants.InputName.RELOAD)) return;
            if (IsDash) return;
            if (searchArea.IsSearch) return;
            if (IsSpecialWeapon) return;
            if (!nutrientsRecoveryPoint_) return;

            switch (vrp.VitaminType)
            {
                case NUTRIENTS_TYPE._A:
                    {
                        if (gunL_.Ammo == gunL_.MaxAmmo_) return;
                        //gunL_.Reload(vrp.Charge(GunAmmoMAX_L - GunAmmoL));
                        Gun[] guns = new Gun[] { gunL_ };
                        vrp.Charge(guns);
                        _audioManager.Play3DSE(position_, SEPath.GAME_SE_SUPPLY);
                    }
                    break;

                case NUTRIENTS_TYPE._B:
                    {
                        if (gunR_.Ammo == gunR_.MaxAmmo_) return;
                        //gunR_.Reload(vrp.Charge(GunAmmoMAX_R - GunAmmoR));
                        Gun[] guns = new Gun[] { gunR_ };
                        vrp.Charge(guns);
                        _audioManager.Play3DSE(position_, SEPath.GAME_SE_SUPPLY);
                    }
                    break;

                case NUTRIENTS_TYPE._ALL:
                    {
                        if (gunL_.Ammo == gunL_.MaxAmmo_ && gunR_.Ammo == gunR_.MaxAmmo_) return;
                        //gunL_.Reload(vrp.Charge(GunAmmoMAX_L - GunAmmoL));
                        //gunR_.Reload(vrp.Charge(GunAmmoMAX_R - GunAmmoR));
                        Gun[] guns = new Gun[] { gunL_, gunR_ };
                        vrp.Charge(guns);
                        _audioManager.Play3DSE(position_, SEPath.GAME_SE_SUPPLY);
                    }
                    break;
            }

            nutrientsRecoveryPoint_ = null;
        }


        /// <summary>
        /// 敵の不足ビタミンを知るエリアを展開
        /// </summary>
        void Search()
        {
            if (_isJump) return;
            if (IsDash) return;

            if (!Input.GetButtonDown(Constants.InputName.SCAN)) return;

            if (!searchArea.IsSearch)
            {
                searchArea.Search();
            }
            else
            {
                searchArea.Stop();
            }
        }


        /// <summary>
        /// スタン中の処理
        /// </summary>
        void StunStatus()
        {
            if (!IsStun) return;

            //スタン中だったら
            if(stunTime > _nowStunTime)
            {
                _nowStunTime += Time.deltaTime;
                _nowStunSoundRate += Time.deltaTime;

                if (_nowStunSoundRate > StunSoundRate)
                {
                    _audioManager.Play3DSE(position_, SEPath.GAME_SE_STUN);
                    _nowStunSoundRate = 0;
                }
            }
            else
            {
                _nowStunTime = 0.0f;

                IsStun = false;

                //無敵開始
                IsInvincible = true;

                // アニメーション途中の武器があったら再生する
                foreach (var weaponAnim in _weaponAnims)
                {
                    weaponAnim.speed = 1f;
                }

                Destroy(_stunEffect);
            }
            
        }


        /// <summary>
        /// スタンさせる
        /// </summary>
        public void Stun()
        {
            if (_itemStatusFlag.HasFlag(ITEM_STATUS.INVICIBLE)) return;
            IsStun = true;
            _bountyManager.PlayerDamage();
            _audioManager.Play3DSE(Position, SEPath.GAME_SE_STUN);
            _audioManager.Play3DSE(Position, SEPath.GAME_SE_DAMEGE);
            _comboManager.LostCombo();

            _nowStunSoundRate = StunSoundRate;

            // アニメーション途中の武器があったら一時停止する
            foreach (var weaponAnim in _weaponAnims)
            {
                weaponAnim.speed = 0f;
            }

            if (_stunEffect == null)
            {
                _stunEffect = Instantiate(StunEffect_, transform);
            }

            ResetAlart();
        }


        /// <summary>
        /// 無敵中の処理
        /// </summary>
        void InvincibleStatus()
        {
            //無敵ではなかったら終わり
            if (!IsInvincible) return;

            //無敵タイム中
            if(_nowInvincibleTime < invincibleTime)
            {
                _nowInvincibleTime += Time.deltaTime;
            }
            else //無敵終了
            {
                _nowInvincibleTime = 0.0f;
                IsInvincible = false;
                 
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
                Weapon.ChangeAnimationStart("Put");
                _weaponAnims.Add(Weapon.gameObject.GetComponent<Animator>());
                StartCoroutine(WeaponChangeUpdate());
            }
            
        }

        

        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case TagName.RECOVERY_POINT:
                    // 補給所がnullじゃないなら処理する必要がない
                    if (nutrientsRecoveryPoint_ != null) break;
                    
                    // 触れた補給所が使えなかったら処理する必要がない
                    var nutrientsRecoveryPoint = other.GetComponent<NutrientsRecoveryPoint>();
                    if (!nutrientsRecoveryPoint.IsCharge) break;

                    // 補給所獲得
                    nutrientsRecoveryPoint_ = nutrientsRecoveryPoint;
                    break;
            }
        }


        private void OnTriggerStay(Collider other)
        {
            switch (other.tag)
            {
                case TagName.RECOVERY_POINT:
                    // 補給所がnullじゃないなら処理する必要がない
                    if (nutrientsRecoveryPoint_ != null) break;

                    // 触れた補給所が使えなかったら処理する必要がない
                    var nutrientsRecoveryPoint = other.GetComponent<NutrientsRecoveryPoint>();
                    if (!nutrientsRecoveryPoint.IsCharge) break;

                    // 補給所獲得
                    nutrientsRecoveryPoint_ = nutrientsRecoveryPoint;
                    break;
            }
        }


        private void OnTriggerExit(Collider other)
        {
            switch (other.tag)
            {
                case TagName.RECOVERY_POINT:
                    nutrientsRecoveryPoint_ = null;
                    break;
            }
        }


        /// <summary>
        /// 下の関数だけしか使わない
        /// </summary>
        private bool isOne = false;

        private int animCnt = 0;

        private bool _isNewWeapon = false;

        private IEnumerator WeaponChangeUpdate()
        {
            if (_weaponAnims.Count != 0 && _isWeaponChangeAnimation == false)
            {
                WeaponChangeAnimationStart();

                // 1フレーム待つ
                if (!isOne)
                {
                    animCnt = _weaponAnims.Count;
                    _isNewWeapon = false;
                    isOne = true;
                    yield return null;
                }

                //if(_weaponAnims[0] == null)
                //{
                //    int a = 0;
                //}

                // アニメーション再生待ち
                while (_weaponAnims[0].GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
                {
                    if(animCnt != _weaponAnims.Count)
                    {
                        _isNewWeapon = true;
                    }

                    yield return null;
                }
                // 再生が終わったらリストをいったん空にする
                _weaponAnims.Clear();
                isOne = false;


                // ハンドガンからスペシャルウェポンに切り替わるとき
                if (Weapon == null)
                {
                    SetWeapon();
                    _weaponAnims.Add(Weapon.gameObject.GetComponent<Animator>());
                    // 武器タイプをハンドガンにして何回も武器チェンできるのを防ぐ
                    _weaponType = (int)WEAPON_TYPE.HANDGUN;
                }
                // スペシャルウェポンから切り替わるとき
                else
                {
                    //弾切れでハンドガンに戻るとき
                    if (Weapon.Ammo <= 0 && !_isNewWeapon)
                    {
                        gunL_.gameObject.SetActive(true);
                        gunR_.gameObject.SetActive(true);
                        _weaponAnims.Add(gunL_.gameObject.GetComponent<Animator>());
                        _weaponAnims.Add(gunR_.gameObject.GetComponent<Animator>());
                    }
                    // 新しいスペシャルウェポンの時
                    else
                    {
                        SetWeapon();
                        _weaponAnims.Add(Weapon.gameObject.GetComponent<Animator>());
                    }
                }

                //// 1フレーム待つ
                //if (!isOne)
                //{
                //    isOne = true;
                //    yield return null;
                //}
                //
                //
                //// アニメーション再生待ち
                //while (_weaponAnims[0].GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f) {
                //    yield return null;
                //}
                

                // 武器の切り替え完了
                isOne = false;
                _weaponAnims.Clear();
                WeaponChangeAnimationFinish();
            }
        }


        /// <summary>
        /// 武器を入手する時に呼ぶ
        /// WeaponManagerの引数番目を生成して持たせる
        /// </summary>
        /// <param name="type">乱数</param>
        public void WeaponUpgrade(int type)
        {
            //if (_isWeaponChangeAnimation) return;
            _weaponType = type;


        }


        /// <summary>
        /// 武器切り替え
        /// </summary>
        void WeaponChange()
        {
            if (IsDash) return;
            if (_weaponType == (int)WEAPON_TYPE.HANDGUN || _weaponType == (int)WEAPON_TYPE.NONE) return;

            if (Input.GetButtonDown(InputName.WEAPON_CHANGE))
            {
                // UI_SP_Weaponに武器チェンしたことを教える
                if (spWeaponUI_ == null) spWeaponUI_ = FindObjectOfType<UI.UI_SP_Weapon>();
                spWeaponUI_.WeaponChange();

                gunL_.Reload();
                gunR_.Reload();


                if (IsSpecialWeapon)
                {
                    //武器チェンジアニメーションスタート
                    Weapon.ChangeAnimationStart("Put");
                    _weaponAnims.Add(Weapon.GetComponent<Animator>());
                }
                else
                {
                    //ハンドガンを下に下げるアニメーションを呼ぶ
                    gunL_.ChangeAnimationStart("Put");
                    gunR_.ChangeAnimationStart("Put");
                    _weaponAnims.Add(gunL_.GetComponent<Animator>());
                    _weaponAnims.Add(gunR_.GetComponent<Animator>());
                }

                StartCoroutine(WeaponChangeUpdate());
            }
        }


        /// <summary>
        /// 武器チェンジアニメーションが終わった時に呼ぶ
        /// </summary>
        public void SetWeapon()
        {
            if (_weaponType == (int)WEAPON_TYPE.HANDGUN || _weaponType == (int)WEAPON_TYPE.NONE) return;

            Weapon = Instantiate(_weponManager.WeaponPrefabList[_weaponType], cameraTransform_).GetComponent<Weapon.SpecialWeapon>();
            WeaponList[2] = Weapon;
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
                var obj = GameObject.FindGameObjectWithTag(TagName.GAME_CONTROLLER).GetComponent<UI.PickupItemUI>();
                obj.DeleteItem(ITEM_STATUS.SPEED_UP);
            }
        }

        private void TransparentItemUpdate()
        {
            if (!_itemStatusFlag.HasFlag(ITEM_STATUS.INVICIBLE)) return;

            _transparentItemTime -= Time.deltaTime;

            if(_transparentItemTime < 0)
            {
                _itemStatusFlag &= ~ITEM_STATUS.INVICIBLE; //解除
                IsTransparent = false;
                Destroy(_invisibleObject);
                var obj = GameObject.FindGameObjectWithTag(TagName.GAME_CONTROLLER).GetComponent<UI.PickupItemUI>();
                obj.DeleteItem(ITEM_STATUS.INVICIBLE);
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

            IsTransparent = true;

            _itemStatusFlag |= ITEM_STATUS.INVICIBLE;

            if (_invisibleObject == null)
            {
                _invisibleObject = Instantiate(InvisibleEffect_, _canvas.transform);
                _invisibleObject.transform.SetAsFirstSibling();
            }

            ResetAlart();
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

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                WeaponUpgrade(0);
                if (spWeaponUI_ == null) spWeaponUI_ = FindObjectOfType<UI.UI_SP_Weapon>();
                spWeaponUI_.GetSPWeapon(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                WeaponUpgrade(1);
                if (spWeaponUI_ == null) spWeaponUI_ = FindObjectOfType<UI.UI_SP_Weapon>();
                spWeaponUI_.GetSPWeapon(1);

            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                WeaponUpgrade(2);
                if (spWeaponUI_ == null) spWeaponUI_ = FindObjectOfType<UI.UI_SP_Weapon>();
                spWeaponUI_.GetSPWeapon(2);
            }

#endif
        }

        /// <summary>
        /// 追いかけられている数を増減
        /// </summary>
        /// <param name="enable"></param>
        public void Alart(bool enable)
        {
            if (enable)
            {
                AlartCnt++;
            }
            else
            {
                AlartCnt--;

                AlartCnt = Math.Max(AlartCnt, 0);
            }
        }

        /// <summary>
        /// アラートエフェクトの生成と破棄
        /// </summary>
        private void Alart()
        {
            if (0 < AlartCnt && !isAlart)
            {
                AlartObj = Instantiate(AlartEffect_, _canvas.transform);
                AlartObj.transform.SetAsFirstSibling();

                isAlart = true;
            }

            if (0 == AlartCnt && (null != AlartObj))
            {
                Destroy(AlartObj);

                isAlart = false;
            }
        }

        /// <summary>
        /// 追いかけられている数をリセットする
        /// </summary>
        private void ResetAlart()
        {
            AlartCnt = 0;
        }
    }

};
