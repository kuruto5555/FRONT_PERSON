using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using FrontPerson.Weapon;
using FrontPerson.Constants;
using FrontPerson.Enemy;
using FrontPerson.Enemy.AI;
using FrontPerson.Manager;
using FrontPerson.Item;

namespace FrontPerson.Character
{
    /// <summary>
    /// 敵オブジェクトのインターフェースクラス
    /// </summary>
    public abstract class Enemy : MonoBehaviour
    {
        // 足りないビタミン
        protected NUTRIENTS_TYPE lack_vitamins  = NUTRIENTS_TYPE.COUNT;

        public NUTRIENTS_TYPE LackVitamins { get { return lack_vitamins; } }

        [Header("不足しているビタミンの文字列")]
        [SerializeField]
        protected TextMesh tetxMesh;

        [Header("不足しているビタミンの量")]
        [SerializeField]
        protected int insufficiency = 100;

        /// <summary>
        /// NavMeshAgent
        /// </summary>
        protected NavMeshAgent agent;

        public NavMeshAgent Agent { get { return agent; } }

        /// <summary>
        /// 敵の種類
        /// </summary>
        public EnemyType Type { get; protected set; } = EnemyType.NONE;

        /// <summary>
        /// 敵AIのステート(インターフェース)
        /// </summary>
        public EnemyState_AI state_AI = null;

        /// <summary>
        /// 倒れていることを判断するフラグ(true = 死んでいる)
        /// </summary>
        public bool isDead { get; private set; } = false;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

            if(null == state_AI) state_AI = GetComponent<EnemyState_AI>();

            state_AI.SetOwner(this);
        }

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

        /// <summary>
        /// 不足しているビタミンを設定
        /// </summary>
        protected void Set_LackVitamin()
        {
            if (EnemyType.YAKUZA == Type)
            {
                lack_vitamins = NUTRIENTS_TYPE._ALL;
                return;
            }

            int cnt = Random.Range(0, (int)NUTRIENTS_TYPE._ALL);

            List<NUTRIENTS_TYPE> vitamins = new List<NUTRIENTS_TYPE>() { NUTRIENTS_TYPE._A, NUTRIENTS_TYPE._B };

            lack_vitamins = vitamins[cnt];

        }

        /// <summary>
        ///  表示する文字を設定
        /// </summary>
        protected void Set_LackVitamin_Text()
        {
            tetxMesh.text = Nutrients.Type[(int)lack_vitamins];
        }

        /// <summary>
        /// 不足しているビタミンを増減させる
        /// </summary>
        /// <param name="cnt"></param>
        public void AddVitamins(int cnt)
        {
            insufficiency -= cnt;
        }

        /// <summary>
        /// 倒れた時に呼ぶ関数
        /// </summary>
        protected void SetDestroy()
        {
            isDead = true;
        }

        private void Dead()
        {
            if(true == isDead)
            {
                ScoreManager score_manager = ScoreManager.Instance;

#if UNITY_EDITOR
                if (null == score_manager)
                {
                    Debug.LogError("ScoreManager が存在しません");
                    
                    GetComponent<EnemyBelongings>().DropItem();
                    Destroy(gameObject);
                    return;
                }
#endif

                switch (Type)
                {
                    case EnemyType.ORDINATY_PEOPLE:
                        score_manager.AddScore((int)EnemyScore.ORDINATY_PEOPLE, Score.ReasonForAddition.Nomal);
                        Spawner.Sub_OrdinaryPeople();
                        break;

                    case EnemyType.OLD_BATTLEAXE:
                        score_manager.AddScore((int)EnemyScore.OLD_BATTLEAXE, Score.ReasonForAddition.Nomal);
                        Spawner.Sub_OldBattleaxe();
                        break;

                    case EnemyType.YAKUZA:
                        score_manager.AddScore((int)EnemyScore.YAKUZA, Score.ReasonForAddition.Nomal);
                        Spawner.Sub_Yakuza();
                        break;

                    default:
                        break;
                }

                GetComponent<EnemyBelongings>().DropItem();
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// AIのステートを設定
        /// </summary>
        /// <param name="state"></param>
        public void SetState(EnemyState_AI state)
        {
            state_AI = state;
        }

        public void SetTarget(Transform goal)
        {
            if (null != goal)
            {
                agent.destination = goal.position;
            }
            else
            {
                agent.destination = transform.position;
            }
        }

        /// <summary>
        /// 抽象関数
        /// 継承者は宣言しないとエラー吐きますよ。
        /// </summary>
        protected abstract void OnStart();
        protected abstract void OnUpdate();

        public abstract void HitBullet(Bullet bullet);

        ///// <summary>
        ///// 仮想関数
        ///// 物理的接触が発生した瞬間
        ///// </summary>
        //protected virtual void OnCollisionEnter(Collision collision) { }
        ///// <summary>
        ///// 仮想関数
        ///// 物理的接触が発生している間(１フレームごと)
        ///// </summary>
        //protected virtual void OnCollisionStay(Collision collision) { }
        ///// <summary>
        ///// 仮想関数
        ///// 物体が離れた瞬間
        ///// </summary>
        //protected virtual void OnCollisionExit(Collision collision) { }

        ///// <summary>
        ///// 仮想関数
        ///// トリガーに接触した瞬間
        ///// </summary>
        //protected virtual void OnTriggerEnter(Collider collider) { }
        ///// <summary>
        ///// 仮想関数
        ///// トリガーに接触している間(１フレームごと)
        ///// </summary>
        //protected virtual void OnTriggerStay(Collider collider) { }
        ///// <summary>
        ///// 仮想関数
        ///// トリガーから離れた瞬間
        ///// </summary>
        //protected virtual void OnTriggerExit(Collider collider) { }
    }
}