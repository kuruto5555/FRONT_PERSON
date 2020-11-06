using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Enemy.AI
{
    public class EnemyState_Wait : EnemyState_AI
    {
        protected override void OnStart()
        {
            Owner.SetTarget(null);
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnChangeState()
        {
        }
    }
}
