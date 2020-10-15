using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FrontPerson.Gun
{

    public class Bullet : MonoBehaviour
    {
        [Header("弾速")]
        [SerializeField, Range(30f, 100f)]
        float speed_ = 50.0f;

        [Header("弾の威力")]
        [SerializeField, Range(1, 1000)]
        int power_ = 10;

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



            Destroy(gameObject);
        }
    }


};

