using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using FrontPerson.Character;

namespace FrontPerson.Enemy.AI
{
    public class EnemyState_Close : EnemyState_AI
    {
        [Header("目的地")]
        [SerializeField]
        private Transform goal = null;
        public Transform Goal { set { goal = value; } }

        /// <summary>
        /// 半径からのオフセット距離
        /// </summary>
        private float RadiusOffset = 0.3f;

        private Vector3 offset = new Vector3();
        public Vector3 Offset { set { offset = value; } }

        private float time = 0.0f;

        public float max_time = 1.0f;

        private readonly int ATTACK = Animator.StringToHash("Attack");

        public override void OnStart()
        {
            if (null == Player)
            {
                Player = GameObject.FindGameObjectWithTag(Constants.TagName.PLAYER).GetComponent<Player>();
                goal = Player.transform;
            }

            ClalOffset();

            Owner.SetTarget(goal.position + offset);

            time = Time.timeSinceLevelLoad;
        }

        protected override void OnUpdate()
        {
            if(max_time <= (Time.timeSinceLevelLoad - time))
            {
                time = Time.timeSinceLevelLoad;

                ClalOffset();

                Owner.SetTarget(goal.position + offset);
            }
        }

        protected override void OnChangeState_OrdinaryPeople()
        {
        }

        protected override void OnChangeState_OldBattleaxe()
        {
            if (Player.IsStun || Player.IsInvincible || Player.IsTransparent)
            {
                ChangeState<EnemyState_Move>();

                OldBattleaxe enemy = Owner as OldBattleaxe;

                enemy.isAngry = false;

                return;
            }

            if (Mathf.Abs((Owner.transform.position - goal.position).magnitude) <= 3.0f)
            {
                ChangeState<EnemyState_Attack>();

                OldBattleaxe enemy = Owner as OldBattleaxe;

                //enemy.Attack();

                enemy.Animator.Play(ATTACK);

                return;
            }
        }

        protected override void OnChangeState_Yakuza()
        {
            if (Player.IsStun || Player.IsInvincible || Player.IsTransparent)
            {
                ChangeState<EnemyState_Move>();

                return;
            }

            if (Mathf.Abs((Owner.transform.position - goal.position).magnitude) <= 3.0f)
            {
                ChangeState<EnemyState_Attack>();

                return;
            }
        }

        /// <summary>
        /// 半径(ナビゲーション)分のオフセットを計算
        /// </summary>
        private void ClalOffset()
        {
            var ai = Owner.state_AI as EnemyState_Close;

            Vector3 vec_enemy = transform.position - goal.position;
            vec_enemy.y = 0f;

            Vector3 vec_radius = Vector3.Normalize(vec_enemy) * (Player.GetComponent<NavMeshObstacle>().radius + RadiusOffset);
            vec_radius.y = 0f;

            Offset = vec_radius;
        }
    }
}