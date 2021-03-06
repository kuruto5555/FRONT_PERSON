﻿using FrontPerson.Character;
using FrontPerson.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;


namespace FrontPerson.Weapon
{
    public class Bullet : MonoBehaviour
    {
        [Header("弾の種類")]
        [SerializeField]
        NUTRIENTS_TYPE bulletType_ = NUTRIENTS_TYPE._ALL;

        [Header("弾速")]
        [SerializeField, Range(1f, 100f)]
        protected float speed_ = 50.0f;

        [Header("弾の威力")]
        [SerializeField, Range(1, 1000)]
        int power_ = 1;

        [Header("ぶしゃーパーティクル")]
        [SerializeField]
        GameObject splashParticle_ = null;

        [Header("有効射程")]
        [SerializeField, Range(50.0f, 100.0f)] float _range = 50.0f;

        protected AudioManager _audioManager = null;

        /// <summary>
        /// 生成された位置
        /// </summary>
        Vector3 initPos_;

        /// <summary>
        /// 前フレームの位置
        /// </summary>
        protected Vector3 prevPos_;

        /// <summary>
        /// 弾の威力
        /// </summary>
        public int Power { get { return power_; } }

        /// <summary>
        /// 弾の種類
        /// </summary>
        public NUTRIENTS_TYPE BulletType { get{return bulletType_;} }

        protected void Start()
        {
            prevPos_ = initPos_ = transform.position;
            _audioManager = AudioManager.Instance;
        }

        protected void Update()
        {
            Move();
        }



        public virtual void Move()
        {
            Vector3 position = prevPos_ = transform.position;

            position += transform.forward * speed_ * Time.deltaTime;

            transform.position = position;

            if((initPos_-position).magnitude > _range)
            {
                Destroy(gameObject);
            }
        }


        private bool HitCheck(out RaycastHit hit)
        {
            Vector3 start = transform.position - transform.forward * 2f;
            Vector3 end = transform.position;
            if (Physics.SphereCast(
                start,
                transform.localScale.x * 0.5f,
                transform.forward,
                out hit,
                (end - start).magnitude,
                1 << LayerNumber.ENEMY | 1 << LayerNumber.FIELD_OBJECT))
            {
                /* デバック */
                Debug.DrawLine(start, end, Color.red, 1000f);
                return true;
            }

            return false;
        }

        void HitUntagged()
        {
            RaycastHit hit;
            if (HitCheck(out hit))
            {
                Instantiate(splashParticle_, hit.point, Quaternion.identity);
                _audioManager.Play3DSE(transform. position, SEPath.GAME_SE_LANDING);
                Destroy(gameObject);
            }
        }

        protected virtual void HitEnemy(Character.Enemy enemy)
        {
            RaycastHit hit;
            if (HitCheck(out hit))
            {
                Instantiate(splashParticle_, hit.point, Quaternion.identity);
                enemy.HitBullet(this);
                _audioManager.Play3DSE(transform.position, SEPath.GAME_SE_LANDING);
                Destroy(gameObject);
            }
        }



        private void OnTriggerEnter(Collider other)
        {

            switch (other.gameObject.tag)
            {
                case TagName.ENEMY:
                HitEnemy(other.GetComponent<Character.Enemy>());
                    break;

                case TagName.UNTAGGED:
                    HitUntagged();
                    break;
            }
        }


        private void OnDrawGizmos()
        {
            Vector3 start = transform.position - transform.forward * 2f;
            Vector3 end = transform.position;
            RaycastHit hit;
            if (Physics.SphereCast(
                start,
                transform.localScale.x * 0.5f,
                transform.forward,
                out hit,
                (end - start).magnitude,
                1 << LayerNumber.ENEMY))
            {
                Gizmos.DrawWireSphere(hit.point, transform.localScale.x * 0.5f);
            }
            Gizmos.DrawLine(start, end);
        }
    }
};

