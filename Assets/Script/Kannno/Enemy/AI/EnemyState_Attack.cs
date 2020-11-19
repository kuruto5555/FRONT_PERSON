using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Enemy.AI
{
    public class EnemyState_Attack : EnemyState_AI
    {
        private SearchArea searchArea = null;

        protected override void OnStart()
        {
            searchArea = GetComponentInChildren<SearchArea>();
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnChangeState_OrdinaryPeople()
        {
        }

        protected override void OnChangeState_OldBattleaxe()
        {
            if (!searchArea.IsFound || Player.IsStun)
            {
                ChangeState<EnemyState_Move>();

                //Owner.state_AI.Load_MovePoint(MoveIndex, MovetList);

                OldBattleaxe enemy = Owner as OldBattleaxe;

                enemy.isHit = false;
            }

            Player.Stun();
        }

        protected override void OnChangeState_Yakuza()
        {
            if (!searchArea.IsFound || Player.IsStun)
            {
                //ChangeState<EnemyState_Move>();
                ChangeState<EnemyState_Search>();

                //Owner.state_AI.Load_MovePoint(MoveIndex, MovetList);
            }

            Player.Stun();
        }
    }
}