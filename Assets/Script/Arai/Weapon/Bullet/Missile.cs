using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Constants;

namespace FrontPerson.Weapon
{
    public class Missile : Bullet
    {
        [Header("爆発範囲")]
        [SerializeField, Range(0.1f, 10.0f)] float Radius_ = 1.0f;

        [Header("爆発プレハブ")]
        [SerializeField] GameObject Blast_ = null;

        [Header("旋回速度")]
        [SerializeField, Range(0.01f, 1.0f)] float TurnSpeed_ = 0.06f;

        [Header("追尾開始する高度")]
        [SerializeField, Range(10.0f, 100.0f)] float StartHeight_ = 50.0f;


        [SerializeField] ParticleSystem Particle_ = null;

        [Header("爆発エフェクトプレハブ")]
        [SerializeField] GameObject _BlastEffect = null;

        //発射されてからの時間
        private float _nowTime = 0.0f;

        private Transform _target = null;
        private Vector3 _targetPos;

        private LockOnUI _cursor = null;

        private bool _isHoming = false;

        Vector3 newPos;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            _nowTime = 0.0f;
            _isHoming = false;
            _targetPos = _target.position;

            newPos = transform.position + transform.forward * 25.0f;
            newPos.y = StartHeight_;

        }

        // Update is called once per frame
        void Update()
        {
            base.Update();

            _nowTime += Time.deltaTime;

            TargetUpdate();
        }

        private void TargetUpdate()
        {
            //ターゲットがいなかったらターゲットの座標を更新しない
            if (_target == null) return;

            _targetPos = _target.position;
        }

        public override void Move()
        {
            prevPos_ = transform.position;
            Rise();
            Homing();
          
        }

        /// <summary>
        /// 上昇
        /// </summary>
        private void Rise()
        {
            //if (_nowTime < StarterSpeed_) return;
            if (_isHoming) return;

            //Vector3 newPos = new Vector3(_targetPos.x, StartHeight_, _targetPos.z);
            

            //ターゲットの方向ベクトル
            Vector3 vec = (newPos - transform.position).normalized;

            //方向を、回転情報に変換
            Quaternion rot = Quaternion.LookRotation(vec);

            transform.rotation = Quaternion.Slerp(transform.rotation, rot, TurnSpeed_);

            transform.position += transform.forward * speed_ * Time.deltaTime;

            if (transform.position.y > StartHeight_) _isHoming = true;

        }

        private void Homing()
        {
            if (!_isHoming)  return;

            //ターゲットの方向ベクトル
            Vector3 vec = (_targetPos - transform.position).normalized;

            //方向を、回転情報に変換
            Quaternion rot = Quaternion.LookRotation(vec);

            transform.rotation = Quaternion.Slerp(transform.rotation, rot, TurnSpeed_);

            transform.position += transform.forward * speed_ * Time.deltaTime;
        }

        public void SetData(Transform data, LockOnUI cursor)
        {
            _target = data;
            _cursor = cursor;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((other.gameObject.tag == TagName.ENEMY) || other.gameObject.layer == LayerNumber.FIELD_OBJECT)
            {
                var blast = Instantiate(Blast_, transform.position, Quaternion.identity);
                blast.GetComponent<Blast>().SetData(Radius_, this);
                _cursor.SetDead();
                Particle_.transform.parent = null;
                Particle_.loop = false;
                GameObject obj = Instantiate(_BlastEffect, transform.position, Quaternion.identity);
                //obj.GetComponent<ParticleSystemRenderer>().material = _BlastEffect.GetComponent<ParticleSystemRenderer>().sharedMaterial;
                _audioManager.Play3DSE(transform.position, SEPath.GAME_SE_LANDING_MISSILE);
                Destroy(gameObject);
            }
        }
    }
}