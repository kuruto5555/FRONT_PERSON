using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Enemy.AI
{
    public class EnemyState_Search : EnemyState_AI
    {
        private SearchArea searchArea = null;

        protected override void OnStart()
        {
            searchArea = GetComponentInChildren<SearchArea>();
            Owner.SetTarget(null);
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnChangeState_OrdinaryPeople()
        {
        }

        protected override void OnChangeState_OldBattleaxe()
        {
        }

        protected override void OnChangeState_Yakuza()
        {
            if (searchArea.IsFound)
            {
                ChangeState<EnemyState_Close>();

                var ai = Owner.state_AI as EnemyState_Close;

                ai.Goal = FindObjectOfType<Character.Player>().transform;
            }
        }
    }
}