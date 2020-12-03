using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FrontPerson.Character;

namespace FrontPerson.Item
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Pickup : MonoBehaviour
    {
        [Header("回転速度（秒間）")]
        [SerializeField]
        private float rotation_speed_ = 360f;

        [Header("ふわふわ浮く移動量")]
        [SerializeField]
        private float fluffy_movement_ = 0.5f;

        [Header("浮き沈みの速度")]
        [SerializeField]
        private float fluffy_speed_ = 1f;

        [Header("取得時のエフェクト")]
        [SerializeField]
        private ParticleSystem particle_ = null;

        /// <summary>
        /// 拾った際のイベント
        /// </summary>
        public UnityAction<Player> onPick;

        private Transform transform_;

        // Start is called before the first frame update
        void Start()
        {
            transform_ = transform;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().isTrigger = true;
        }

        // Update is called once per frame
        void Update()
        {
            // ふわふわ浮かせる
//          transform_.position = new Vector3(transform_.position.x, Mathf.PingPong(Time.time / fluffy_speed_, fluffy_movement_), transform_.position.z);
            //transform_.position = Vector3.up * Mathf.PingPong(Time.time * fluffy_speed_, fluffy_movement_);

            // 回転
            transform_.Rotate(Vector3.up, rotation_speed_ * Time.deltaTime, Space.Self);
        }

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                if(onPick != null)
                {
                    onPick.Invoke(player);

                    Destroyed();
                }
            }
        }

        public void PlayPickupFeedback()
        {
            if (particle_)
            {
                Instantiate(particle_, transform_.position, Quaternion.identity);
            }
        }

        public void Destroyed()
        {
            Destroy(this.gameObject);
        }
    }
}
