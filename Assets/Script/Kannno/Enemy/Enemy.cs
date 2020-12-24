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
        /// 倒れていることを判断するフラグ(true = 倒れている)
        /// </summary>
        public bool isDown { get; private set; } = false;

        /// <summary>
        /// 死んでいることを判断するフラグ(true = 死んでいる)
        /// </summary>
        public bool isDead { get; private set; } = false;

        /// <summary>
        /// 1回しか実行させない為の変数
        /// </summary>
        private bool Eneble = false; 

        private void Awake()
        {
            OnAwake();

            Set_LackVitamin();
        }

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            if (null == state_AI) state_AI = GetComponent<EnemyState_AI>();

            state_AI.SetOwner(this);

            Set_LackVitamin_Text();

            OnStart();
        }

        private void Update()
        {
            OnUpdate();

            Down();

            Dead();
        }

        /// <summary>
        /// 不足しているビタミンを設定
        /// </summary>
        private void Set_LackVitamin()
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
        protected void SetDown()
        {
            isDown = true;
        }

        /// <summary>
        /// ころす時に呼ぶ関数
        /// </summary>
        public void SetDead()
        {
            isDead = true;
        }

        private void Down()
        {
            if (true == isDown && false == Eneble)
            {
                ScoreManager score_manager = ScoreManager.Instance;
                ComboManager comboManager = ComboManager.Instance;

#if UNITY_EDITOR
                if (null == score_manager)
                {
                    Debug.LogError("ScoreManager が存在しません");

                    GetComponent<EnemyBelongings>().DropItem();

                    state_AI.ChangeState<EnemyState_Escape>();

                    Eneble = true;

                    return;
                }
#endif

                switch (Type)
                {
                    case EnemyType.ORDINATY_PEOPLE:
                        score_manager.AddScore((int)EnemyScore.ORDINATY_PEOPLE, Score.ADD_SCORE_TYPE.BASIC_SCORE);
                        comboManager.AddCombo(1, ADD_COMBO_TYPE.ORDINATY_PEOPLE);

                        AudioManager.Instance.Play3DSE(transform.position, SEPath.GAME_SE_ENEMY_FINE);
                        break;
                
                    case EnemyType.OLD_BATTLEAXE:
                        score_manager.AddScore((int)EnemyScore.OLD_BATTLEAXE, Score.ADD_SCORE_TYPE.BASIC_SCORE);
                        comboManager.AddCombo(1, ADD_COMBO_TYPE.OLD_BATTLEAXE);

                        AudioManager.Instance.Play3DSE(transform.position, SEPath.GAME_SE_ENEMY_FINE);
                        break;
                
                    case EnemyType.YAKUZA:
                        score_manager.AddScore((int)EnemyScore.YAKUZA, Score.ADD_SCORE_TYPE.BASIC_SCORE);
                        comboManager.AddCombo(1, ADD_COMBO_TYPE.YAKUZA);
                        break;
                }

                GetComponent<EnemyBelongings>().DropItem();

                state_AI.ChangeState<EnemyState_Escape>();

                Eneble = true;
            }
        }

        private void Dead()
        {
            if (true == isDead)
            {
                switch (Type)
                {
                    case EnemyType.ORDINATY_PEOPLE:
                        Spawner.Sub_OrdinaryPeople();
                        break;

                    case EnemyType.OLD_BATTLEAXE:
                        Spawner.Sub_OldBattleaxe();
                        break;

                    case EnemyType.YAKUZA:
                        Spawner.Sub_Yakuza();
                        break;
                }

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

        public void SetTarget(Vector3 goal)
        {
            agent.SetDestination(goal);
        }

        /// <summary>
        /// 抽象関数
        /// 継承者は宣言しないとエラー吐きますよ。
        /// </summary>
        protected abstract void OnStart();
        protected abstract void OnUpdate();

        protected abstract void OnAwake();

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