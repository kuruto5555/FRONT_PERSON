using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Enemy.AI
{
    public class EnemyState_Search : EnemyState_AI
    {
        protected override void OnStart()
        {
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnChangeState()
        {
            Destroy(Owner.state_AI);
            Owner.state_AI = Owner.gameObject.AddComponent<EnemyState_Close>();
            Owner.state_AI.SetOwner(Owner);
        }
    }
}