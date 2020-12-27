using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Enemy.AI
{
    public class EnemyState_Attack : EnemyState_AI
    {
        //private SearchArea searchArea = null;

        public override void OnStart()
        {
        }


        protected override void OnUpdate()
        {
        }

        protected override void OnChangeState_OrdinaryPeople()
        {
        }

        protected override void OnChangeState_OldBattleaxe()
        {
            if (!SearchArea.IsFound || Player.IsStun || Player.IsTransparent)
            {
                ChangeState<EnemyState_Move>();

                OldBattleaxe enemy = Owner as OldBattleaxe;

                enemy.isHit = false;
                return;
            }

            Player.Stun();
        }

        protected override void OnChangeState_Yakuza()
        {
            if (!SearchArea.IsFound || Player.IsStun || Player.IsTransparent)
            {
                ChangeState<EnemyState_Move>();
                return;
            }

            Player.Stun();
        }
    }
}