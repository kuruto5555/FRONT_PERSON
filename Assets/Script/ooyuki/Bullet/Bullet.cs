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
        Constants.VITAMIN_TYPE bulletType_ = Constants.VITAMIN_TYPE.VITAMIN_B;

        [Header("弾速")]
        [SerializeField, Range(30f, 100f)]
        float speed_ = 50.0f;

        [Header("弾の威力")]
        [SerializeField, Range(1, 1000)]
        int power_ = 1;

        /// <summary>
        /// 生成された位置
        /// </summary>
        Vector3 initPos_;

        /// <summary>
        /// 弾の威力
        /// </summary>
        public int Power { get { return power_; } }

        void Start()
        {
            initPos_ = transform.position;
        }

        void Update()
        {
            Move();
        }



        protected void Move()
        {
            Vector3 position = transform.position;

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


            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Hit();

            switch (collision.gameObject.tag)
            {
                case TagName.ENEMY:
                    break;
            }
        }
    }


};

