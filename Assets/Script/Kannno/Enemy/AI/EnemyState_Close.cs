using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Enemy.AI
{
    public class EnemyState_Close : EnemyState_AI
    {
        //private Enemy01 enemy01 = null;

        private float time = 0.0f;

        public float max_time = 1.0f;

        protected override void OnStart()
        {
            Owner.SetTarget(Owner.Goal);

            time = Time.timeSinceLevelLoad;
        }

        protected override void OnUpdate()
        {
            {
                if (Mathf.Abs((Owner.transform.position - Owner.Goal.position).magnitude) <= 5.0f)
                {
                    OnChangeState();
                }
            }

            if(max_time <= (Time.timeSinceLevelLoad - time))
            {
                time = Time.timeSinceLevelLoad;

                Owner.SetTarget(Owner.Goal);
            }
        }

        protected override void OnChangeState()
        {
            Destroy(Owner.state_AI);
            Owner.state_AI = Owner.gameObject.AddComponent<EnemyState_Wait>();
            Owner.state_AI.SetOwner(Owner);

            Debug.Log("ステートが変わった");
        }
    }
}