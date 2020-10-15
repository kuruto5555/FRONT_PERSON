using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FrontPerson.Gun
{

    public enum BULLET_TYPE
    {
        HAND_GUN = 0,
        MACHINE_GUN,
        SHOTGUN,
        ROCKET_LAUNCHER,

        BULLET_TYPE_MAX,
    }


    public class Gun : MonoBehaviour
    {
        [Header("弾のプレハブ")]
        [SerializeField]
        List<GameObject> bullets_ = null;

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
        /// 現在の銃のタイプ
        /// </summary>
        [HideInInspector]
        public BULLET_TYPE bulletType_ = BULLET_TYPE.HAND_GUN;

        /// <summary>
        /// 一発撃ってからの時間
        /// </summary>
        float shotTime_ = 0.0f;

        /// <summary>
        /// 現在の弾数
        /// </summary>
        int ammo_ = 0;




        // Start is called before the first frame update
        void Start()
        {
            ammo_ = MaxAmmo_;
            shotTime_ = 0.0f;
            bulletType_ = BULLET_TYPE.HAND_GUN;
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

            Instantiate(bullets_[(int)bulletType_], hole_.transform.position, hole_.transform.rotation, null);
            shotTime_ = rate_;
            ammo_--;
        }

        /// <summary>
        /// 吸い込む
        /// </summary>
        public void Inhole()
        {
        }

        /// <summary>
        /// リロード（仕様次第で消すかも）
        /// </summary>
        public void Reload()
        {
            if (ammo_ >= MaxAmmo_) return;

            ammo_ = MaxAmmo_;
        }

        /// <summary>
        /// 弾の切り替え
        /// </summary>
        /// <param name="newBulletType">切り替えたい弾の種類</param>
        public void ChangeBullet(BULLET_TYPE newBulletType)
        {
            if (bulletType_ == newBulletType) return;

            bulletType_ = newBulletType;


        }
    }

};
