using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Enemy.AI
{
    public class EnemyState_Search : EnemyState_AI
    {
        public override void OnStart()
        {
            Owner.SetTarget(Owner.transform.position);
        }

        protected override void OnUpdate()
        {
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

                var ai = Owner.state_AI as EnemyState_Close;

                ai.Goal = Player.transform;
            }
        }

        protected override void OnChangeState_Yakuza()
        {
            if (SearchArea.IsFound && (!Player.IsInvincible && !Player.IsStun) )
            {
                ChangeState<EnemyState_Close>();

                var ai = Owner.state_AI as EnemyState_Close;

                ai.Goal = Player.transform;
            }
        }
    }
}