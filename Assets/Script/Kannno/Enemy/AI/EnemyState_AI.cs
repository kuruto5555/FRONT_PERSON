using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Character;

namespace FrontPerson.Enemy.AI
{
    /// <summary>
    /// 敵AIオブジェクトのインターフェースクラス
    /// </summary>
    public abstract class EnemyState_AI : MonoBehaviour
    {
        protected Character.Enemy Owner = null;

        protected Player Player = null;

        // Start is called before the first frame update
        public void Start()
        {
            OnStart();

            Player = FindObjectOfType<Player>();
        }

        // Update is called once per frame
        public void Update()
        {
            OnUpdate();
            OnChangeState();
        }

        /// <summary>
        /// 抽象関数
        /// 継承者は宣言しないとエラー吐きますよ。
        /// </summary>
        protected abstract void OnStart();
        protected abstract void OnUpdate();

        /// <summary>
        /// 一般人のステート遷移インターフェース
        /// </summary>
        protected abstract void OnChangeState_OrdinaryPeople();

        /// <summary>
        /// おばちゃんのステート遷移インターフェース
        /// </summary>
        protected abstract void OnChangeState_OldBattleaxe();

        /// <summary>
        /// ヤクザのステート遷移インターフェース
        /// </summary>
        protected abstract void OnChangeState_Yakuza();

        public void SetOwner(Character.Enemy enemy)
        {
            Owner = enemy;
        }

        /// <summary>
        /// 敵AIのステートをT型に変更する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ChangeState<T>() where T : EnemyState_AI
        {
#if UNITY_EDITOR
            string B_State = Owner.state_AI.ToString();
            int pos = B_State.LastIndexOf(".");
            B_State = B_State.Remove(0, pos + 1);
            B_State = B_State.TrimEnd(')');
#endif

            Destroy(Owner.state_AI);
            Owner.state_AI = Owner.gameObject.AddComponent<T>();
            Owner.state_AI.SetOwner(Owner);

#if UNITY_EDITOR

            string name = Owner.name;
            string A_State = Owner.state_AI.ToString();
            pos = A_State.LastIndexOf(".");
            A_State = A_State.Remove(0, pos + 1);
            A_State = A_State.TrimEnd(')');

            string str =  $"ステートが変わりました : 名前 {name} : BeforeState {B_State} : AfterState {A_State}";

            Debug.Log(str);
#endif
        }

        protected void OnChangeState()
        {
            switch (Owner.Type)
            {
                case EnemyType.ORDINATY_PEOPLE:
                    OnChangeState_OrdinaryPeople();
                    break;

                case EnemyType.OLD_BATTLEAXE:
                    OnChangeState_OldBattleaxe();
                    break;

                case EnemyType.YAKUZA:
                    OnChangeState_Yakuza();
                    break;

                default:
                    Debug.LogError("Owner.Type : 不正な値です");
                    break;
            }
        }
    }
}