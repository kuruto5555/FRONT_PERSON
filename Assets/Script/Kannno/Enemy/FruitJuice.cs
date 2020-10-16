using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson
{
    /// <summary>
    /// 果汁オブジェクトのインターフェースクラス
    /// </summary>
    /// <typeparam name="T">継承先クラス</typeparam>
    public abstract class FruitJuice<T> : MonoBehaviour
    {
        // 飛ぶ向き
        protected Vector3 _move_vec = new Vector3();
        private void SetMoveVec()
        {
            float x = Random.Range(-1.0f, 1.0f);
            float y = Random.Range(-0.3f, 1.0f);
            float z = Random.Range(-1.0f, 1.0f);

            Vector3 vec = new Vector3(x, y, z);
            vec = Vector3.Normalize(vec);

            _move_vec = vec;
        }

        protected float move_scale = 0.0f;

        private void SetMoveScale()
        {
            move_scale = Random.Range(40.0f, 40.0f);
        }

        protected Rigidbody _rigidbody = null;

        // Start is called before the first frame update
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            SetMoveVec();
            SetMoveScale();

            _rigidbody.AddForce(_move_vec * move_scale);

            OnStart();
        }

        // Update is called once per frame
        private void Update()
        {
            OnUpdate();
        }

        /// <summary>
        /// 抽象関数
        /// 継承者は宣言しないとエラー吐きますよ。
        /// </summary>
        protected abstract void OnStart();
        protected abstract void OnUpdate();     // Update必要かな？

        /// <summary>
        /// 仮想関数
        /// 物理的接触が発生した瞬間
        /// </summary>
        protected virtual void OnCollisionEnter(Collision collision) { }
        /// <summary>
        /// 仮想関数
        /// 物理的接触が発生している間(１フレームごと)
        /// </summary>
        protected virtual void OnCollisionStay(Collision collision) { }
        /// <summary>
        /// 仮想関数
        /// 物体が離れた瞬間
        /// </summary>
        protected virtual void OnCollisionExit(Collision collision) { }

        /// <summary>
        /// 仮想関数
        /// トリガーに接触した瞬間
        /// </summary>
        protected virtual void OnTriggerEnter(Collider collider) { }
        /// <summary>
        /// 仮想関数
        /// トリガーに接触している間(１フレームごと)
        /// </summary>
        protected virtual void OnTriggerStay(Collider collider) { }
        /// <summary>
        /// 仮想関数
        /// トリガーから離れた瞬間
        /// </summary>
        protected virtual void OnTriggerExit(Collider collider) { }
    }
}