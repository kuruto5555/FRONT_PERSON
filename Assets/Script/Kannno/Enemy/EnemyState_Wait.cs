using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Enemy
{
    public class EnemyState_Wait : EnemyState_AI
    {
        private Enemy01 enemy01 = null;

        public EnemyState_Wait(Enemy enemy) : base(enemy)
        {
        }

        protected override void OnStart()
        {
            enemy01 = (Enemy01)Owner;

            enemy01.SetTarget(null);
        }

        protected override void OnUpdate()
        {

        }

        protected override void OnChangeState()
        {

        }
    }
}
