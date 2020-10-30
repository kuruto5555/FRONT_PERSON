using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Enemy.AI
{
    public class EnemyState_Close : EnemyState_AI
    {
        private Enemy01 enemy01 = null;

        public EnemyState_Close(Enemy enemy) : base(enemy)
        {
        }

        protected override void OnStart()
        {
            enemy01 = Owner as Enemy01;

            enemy01.SetTarget(enemy01.goal);
        }

        protected override void OnUpdate()
        {
            {
                if (Mathf.Abs((Owner.transform.position - enemy01.goal.position).magnitude) <= 10.0f)
                {
                    OnChangeState();
                }
            }
        }

        protected override void OnChangeState()
        {
            enemy01.SetState(new EnemyState_Wait(Owner));
            enemy01.state_AI.Start();

            Debug.Log("ステートが変わった");
        }
    }
}