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
            if(searchArea.IsFound)
            {
                OnChangeState();
            }
        }

        protected override void OnChangeState()
        {
            ChangeState<EnemyState_Close>();

            var ai = Owner.state_AI as EnemyState_Close;

            ai.Goal = FindObjectOfType<Character.Player>().transform;
        }
    }
}