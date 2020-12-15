using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Constants;
using FrontPerson.Character;
using FrontPerson.Manager;

namespace FrontPerson.Weapon {

    public class Blast : MonoBehaviour
    {
        [SerializeField] Bullet _bullet;

        private float _radius = 0;

        private int _nowLife = 0;

        private AudioManager _audioManager;

        //private Bullet _bullet = null;
        // Start is called before the first frame update
        void Start()
        {
            _nowLife = 0;
            _audioManager = AudioManager.Instance;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if(1 < _nowLife)
            {
                Destroy(gameObject);
            }

            _nowLife ++;
            
        }

        public void SetData(float radius, Bullet bullet)
        {
            _radius = radius;
            GetComponent<SphereCollider>().radius = _radius;
            //_bullet = bullet;
        }

        /// <summary>
        /// 爆風壁貫通対策
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        private bool HitCheckEnemy(Collider other)
        {
            Vector3 pos = transform.position;

            Vector3 vec = (other.transform.position - pos).normalized;

            RaycastHit hit;

            int layerMask = 1 << LayerNumber.ENEMY | 1 << LayerNumber.FIELD_OBJECT; //enemyとFildeObjectだけぶつける

            //hitにnullが返ってくる謎現象がある
            if(Physics.Raycast(pos, vec, out hit, _radius, layerMask))
            {
                if (hit.transform.tag == TagName.ENEMY)
                {
                    return true;
                }
            }
            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerNumber.ENEMY) return;

            other.GetComponent<Character.Enemy>().HitBullet(_bullet);

            //_audioManager.Play3DSE(transform.position, SEPath.GAME_SE_)

            //if (HitCheckEnemy(other))
            //{
            //    other.GetComponent<Character.Enemy>().HitBullet(_bullet);
            //}
           
        }
    }
}