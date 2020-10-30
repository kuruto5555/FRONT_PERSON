using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Enemy.AI
{
    public class EnemyState_Wait : EnemyState_AI
    {
        private Enemy01 enemy01 = null;

        public EnemyState_Wait(Enemy enemy) : base(enemy)
        {
        }

        protected override void OnStart()
        {
            enemy01 = Owner as Enemy01;

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
