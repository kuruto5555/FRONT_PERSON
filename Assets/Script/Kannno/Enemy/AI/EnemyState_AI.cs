using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Enemy.AI
{
    /// <summary>
    /// 敵AIオブジェクトのインターフェースクラス
    /// </summary>
    public abstract class  EnemyState_AI : MonoBehaviour
    {
        protected Enemy Owner = null;

        // Start is called before the first frame update
        public void Start()
        {
            OnStart();
        }

        // Update is called once per frame
        public void Update()
        {
            OnUpdate();
        }

        /// <summary>
        /// 抽象関数
        /// 継承者は宣言しないとエラー吐きますよ。
        /// </summary>
        protected abstract void OnStart();
        protected abstract void OnUpdate();
        protected abstract void OnChangeState();

        public void SetOwner(Enemy enemy)
        {
            Owner = enemy;
        }
    }
}