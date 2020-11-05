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
            searchArea = GetComponent<SearchArea>();
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
            Destroy(Owner.state_AI);
            Owner.state_AI = Owner.gameObject.AddComponent<EnemyState_Close>();
            Owner.state_AI.SetOwner(Owner);

            var ai = Owner.state_AI as EnemyState_Close;

            ai.Goal = FindObjectOfType<Character.Player>().transform;
        }
    }
}