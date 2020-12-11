using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Constants;
using FrontPerson.Manager;

namespace FrontPerson.Weapon
{
    public abstract class Gun : MonoBehaviour
    {
        [Header("弾のプレハブ")]
        [SerializeField]
        protected GameObject bullet_ = null;

        [Header("銃口")]
        [SerializeField]
        protected GameObject Muzzle = null;

        [Header("発射間隔(秒間何発)")]
        [Range(1f, 60.0f)]
        public float rate_ = 1f;

        [Header("マガジンの最大弾数")]
        [Range(1, 200)]
        public int MaxAmmo_ = 10;

        [Header("レティクルPrefab")]
        [SerializeField] GameObject Reticle_ = null;

        /// <summary>
        /// ゲームUIキャンバス参照
        /// </summary>
        protected GameObject _canvas = null;

        [Header("マズルスプラッシュ")]
        [SerializeField] protected GameObject MuzzleFlash = null;

        /// <summary>
        /// ヒエラルキーのBountyManagerを入れておく
        /// </summary>
        protected BountyManager _bountyManager = null;

        protected WEAPON_TYPE _type;

        protected GameObject _reticle = null;

        public WEAPON_TYPE GetWeaponType { get { return _type; } }

        /// <summary>
        /// 一発撃ってからの時間
        /// </summary>
        protected float shotTime_ = 0.0f;

        /// <summary>
        /// 現在の弾数
        /// </summary>
        protected int ammo_ = 0;

        /// <summary>
        /// 今の残弾数
        /// </summary>
        public int Ammo {  get { return ammo_; }}

        protected void Awake()
        {
            _bountyManager = BountyManager._instance;
            _canvas = GameObject.Find("WeaponCanvas");
        }

        // Start is called before the first frame update
        protected void Start()
        {
            ammo_ = MaxAmmo_;
            shotTime_ = 0.0f;

            _bountyManager = BountyManager._instance;
        }

        // Update is called once per frame
        protected void Update()
        {
            UpdateShotTime();


        }

        private void UpdateShotTime()
        {
            if (shotTime_ > 0.0f) shotTime_ -= Time.deltaTime;
        }


        /// <summary>
        /// 撃つ
        /// </summary>
        public virtual void Shot()
        {
            if (shotTime_ > 0.0f) return;
            if (ammo_ < 1) return;
           
            Instantiate(bullet_, Muzzle.transform.position, Muzzle.transform.rotation, null);
            shotTime_ = 1.0f / rate_;
            ammo_--;
            _bountyManager.FireCount();
            Instantiate(MuzzleFlash,  Muzzle.transform.position, Quaternion.identity, transform);
        }

        /// <summary>
        /// リロード
        /// </summary>
        public void Reload()
        {
            if (ammo_ >= MaxAmmo_) return;

            ammo_ = MaxAmmo_;
        }

        /// <summary>
        /// リロード
        /// </summary>
        /// <param name="value">補給量</param>
        public void Reload(int value)
        {
            ammo_ += value;

            if (ammo_ > MaxAmmo_) return;

            ammo_ = MaxAmmo_;
        }

        private void OnDestroy()
        {
            if (_reticle == null) return;

            Destroy(_reticle);
        }

        private void OnDisable()
        {
            if (_reticle == null) return;

            Destroy(_reticle);
        }

        private void OnEnable()
        {
            if (Reticle_ != null)
            {
                _reticle = Instantiate(Reticle_, _canvas.transform);
            }
        }
    }

};
