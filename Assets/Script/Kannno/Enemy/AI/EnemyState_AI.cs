using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Enemy.AI
{
    /// <summary>
    /// 敵AIオブジェクトのインターフェースクラス
    /// </summary>
    public abstract class EnemyState_AI : MonoBehaviour
    {
        protected Character.Enemy Owner = null;

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

        public void SetOwner(Character.Enemy enemy)
        {
            Owner = enemy;
        }

        /// <summary>
        /// 敵AIのステートをT型に変更する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected void ChangeState<T>() where T : EnemyState_AI
        {
            Destroy(Owner.state_AI);
            Owner.state_AI = Owner.gameObject.AddComponent<T>();
            Owner.state_AI.SetOwner(Owner);
        }
    }
}