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
        /// 弾の威力
        /// </summary>
        public int Power { get { return power_; } }

        // Start is called before the first frame update
        void Update()
        {
            Move();
        }



        protected void Move()
        {
            Vector3 position = transform.position;

            position += transform.forward * speed_ * Time.deltaTime;

            transform.position = position;
        }


        protected virtual void Hit(Collider other)
        {
            // 破裂エフェクト生成


            Destroy(gameObject);
        }


    }


};

