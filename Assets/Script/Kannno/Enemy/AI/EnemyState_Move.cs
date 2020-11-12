using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FrontPerson.Enemy.AI
{
    public class EnemyState_Move : EnemyState_AI
    {
        /// <summary>
        /// 移動先の一覧
        /// </summary>
        [SerializeField]
        private List<Transform> MovePoint_List = new List<Transform>();

        private int MovePointIndex = 0;

        protected override void OnStart()
        {
            MovePointIndex = 0;

            Owner.SetTarget(MovePoint_List.First());
        }

        protected override void OnUpdate()
        {
            // 目的地についていたら次の目的地の方に行く
            if (Owner.Agent.remainingDistance <= 0.1f)
            {
                Vector3 destination = MovePoint_List[(MovePointIndex + 1) % MovePoint_List.Count].position;

                Owner.Agent.SetDestination(destination);

                MovePointIndex = (MovePointIndex + 1) % MovePoint_List.Count;
            }
        }

        protected override void OnChangeState_OrdinaryPeople()
        {
        }

        protected override void OnChangeState_OldBattleaxe()
        {
            OldBattleaxe enemy = Owner as OldBattleaxe;

            if (enemy.isHit)
            {
                ChangeState<EnemyState_Close>();
            }
        }

        protected override void OnChangeState_Yakuza()
        {
        }
    }
}