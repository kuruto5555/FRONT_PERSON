using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson
{
    /// <summary>
    /// 敵オブジェクトのインターフェースクラス
    /// </summary>
    /// <typeparam name="T">継承先クラス</typeparam>
    public abstract class Enemy<T> : MonoBehaviour
    {
        // 足りないビタミンの定数
        public enum Vitamin
        {
            Vitamin_A = 0,
            Vitamin_B,
            Vitamin_C,
            Vitamin_D,
            Max,
        }

        static List<string> VitaminStrings = new List<string>() {
            { "ビタミンA" },
            { "ビタミンB" },
            { "ビタミンC" },
            { "ビタミンD" },
        };

        // 足りないビタミン
        protected Vitamin lack_vitamins = Vitamin.Max;

        [Header("不足しているビタミンの文字列")]
        [SerializeField]
        protected TextMesh tetxMesh;

        [Header("不足しているビタミンの量")]
        [SerializeField]
        protected int insufficiency = 1;

        protected bool isDead { private set; get; } = false;

        private void Start()
        {
            Set_LackVitamin();
            Set_LackVitamin_Text();

            OnStart();
        }

        private void Update()
        {
            OnUpdate();

            Dead();
        }

        protected void Set_LackVitamin()
        {
            int cnt = Random.Range(0, (int)Vitamin.Max);

            List<Vitamin> vitamins = new List<Vitamin>() { Vitamin.Vitamin_A, Vitamin.Vitamin_B, Vitamin.Vitamin_C, Vitamin.Vitamin_D };

            lack_vitamins = vitamins[cnt];
        }

        protected void Set_LackVitamin_Text()
        {
            tetxMesh.text = VitaminStrings[(int)lack_vitamins];
        }

        public void AddVitamins(int cnt)
        {
            insufficiency -= cnt;
        }

        protected void SetDestroy()
        {
            isDead = true;
        }

        private void Dead()
        {
            if(true == isDead)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 抽象関数
        /// 継承者は宣言しないとエラー吐きますよ。
        /// </summary>
        protected abstract void OnStart();
        protected abstract void OnUpdate();

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