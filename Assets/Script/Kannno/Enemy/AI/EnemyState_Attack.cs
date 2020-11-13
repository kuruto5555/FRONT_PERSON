using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Character;

namespace FrontPerson.Enemy.AI
{
    public class EnemyState_Attack : EnemyState_AI
    {
        private SearchArea searchArea = null;

        private Player Player = null;

        protected override void OnStart()
        {
            searchArea = GetComponentInChildren<SearchArea>();

            Player = FindObjectOfType<Player>();
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnChangeState_OrdinaryPeople()
        {
        }

        protected override void OnChangeState_OldBattleaxe()
        {
            if (!searchArea.IsFound)
            {
                ChangeState<EnemyState_Search>();

                OldBattleaxe enemy = Owner as OldBattleaxe;

                enemy.isHit = false;
            }
        }

        protected override void OnChangeState_Yakuza()
        {
        }
    }
}