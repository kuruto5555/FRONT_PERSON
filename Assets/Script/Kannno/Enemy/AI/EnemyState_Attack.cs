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
            OldBattleaxe enemy = Owner as OldBattleaxe;

            enemy.isHit = false;
            enemy.isAngry = false;

            Player.Stun();

            //if (!SearchArea.IsFound || Player.IsStun || Player.IsTransparent)
            {
                ChangeState<EnemyState_Move>();
            }
        }

        protected override void OnChangeState_Yakuza()
        {
            Player.Stun();

            //if (!SearchArea.IsFound || Player.IsStun || Player.IsTransparent)
            {
                var enemy = Owner as Yakuza;
                enemy.isDiscovery_anime = false;

                ChangeState<EnemyState_Move>();
            }
        }
    }
}