using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;

namespace FrontPerson.Weapon
{
    public class Gun : MonoBehaviour
    {
        [Header("弾のプレハブ")]
        [SerializeField]
        GameObject bullet_ = null;

        [Header("銃口")]
        [SerializeField]
        GameObject hole_ = null;

        [Header("発射間隔s")]
        [Range(0.1f, 2.0f)]
        public float rate_ = 1f;

        [Header("マガジンの最大弾数")]
        [Range(1, 200)]
        public int MaxAmmo_ = 10;

        /// <summary>
        /// ヒエラルキーのBountyManagerを入れておく
        /// </summary>
        private BountyManager _bountyManager = null;


        /// <summary>
        /// 一発撃ってからの時間
        /// </summary>
        float shotTime_ = 0.0f;

        /// <summary>
        /// 現在の弾数
        /// </summary>
        int ammo_ = 0;
        public int Ammo { get { return ammo_; } }



        // Start is called before the first frame update
        void Start()
        {
            ammo_ = MaxAmmo_;
            shotTime_ = 0.0f;
            _bountyManager = GameObject.FindGameObjectWithTag("BountyManager").GetComponent<BountyManager>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateShotTime();


        }

        void UpdateShotTime()
        {
            if (shotTime_ > 0.0f) shotTime_ -= Time.deltaTime;
        }


        /// <summary>
        /// 撃つ
        /// </summary>
        public void Shot()
        {
            if (shotTime_ > 0.0f) return;
            if (ammo_ < 1) return;

            Instantiate(bullet_, hole_.transform.position, hole_.transform.rotation, null);
            shotTime_ = rate_;
            ammo_--;
            _bountyManager.FireCount();
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

            if(ammo_ > MaxAmmo_)
                ammo_ = MaxAmmo_;
        }
    }

};
