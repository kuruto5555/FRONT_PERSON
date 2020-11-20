using FrontPerson.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FrontPerson.Weapon
{

    public class Bullet : MonoBehaviour
    {
        [Header("弾の種類")]
        [SerializeField]
        NUTRIENTS_TYPE bulletType_ = NUTRIENTS_TYPE._ALL;

        [Header("弾速")]
        [SerializeField, Range(30f, 100f)]
        float speed_ = 50.0f;

        [Header("弾の威力")]
        [SerializeField, Range(1, 1000)]
        int power_ = 1;

        [Header("ぶしゃーパーティクル")]
        [SerializeField]
        GameObject splashParticle_ = null;

        /// <summary>
        /// 生成された位置
        /// </summary>
        Vector3 initPos_;

        /// <summary>
        /// 前フレームの位置
        /// </summary>
        Vector3 prevPos_;

        /// <summary>
        /// 弾の威力
        /// </summary>
        public int Power { get { return power_; } }

        /// <summary>
        /// 弾の種類
        /// </summary>
        public NUTRIENTS_TYPE BulletType { get{return bulletType_;} }

        void Start()
        {
            prevPos_ = initPos_ = transform.position;
        }

        void Update()
        {
            Move();
        }



        protected void Move()
        {
            Vector3 position = prevPos_ = transform.position;

            position += transform.forward * speed_ * Time.deltaTime;

            transform.position = position;

            if((initPos_-position).magnitude > 50f)
            {
                Destroy(gameObject);
            }
        }


        protected virtual void Hit()
        {
            // 破裂エフェクト生成
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
                Instantiate(splashParticle_, hit.point, Quaternion.identity);
                Destroy(gameObject);
                /* デバック */
                Debug.DrawLine(start, end, Color.red, 1000f);
            }


        }



        private void OnTriggerEnter(Collider other)
        {
            Hit();

            switch (other.gameObject.tag)
            {
                case TagName.ENEMY:
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

