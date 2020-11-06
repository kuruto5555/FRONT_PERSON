using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Enemy.AI
{
    public class EnemyState_Close : EnemyState_AI
    {
        [Header("目的地")]
        [SerializeField]
        private Transform goal = null;
        public Transform Goal { set { goal = value; } }

        private float time = 0.0f;

        public float max_time = 1.0f;

        protected override void OnStart()
        {
            Owner.SetTarget(goal);

            time = Time.timeSinceLevelLoad;
        }

        protected override void OnUpdate()
        {
            {
                if (Mathf.Abs((Owner.transform.position - goal.position).magnitude) <= 5.0f)
                {
                    OnChangeState();
                }
            }

            if(max_time <= (Time.timeSinceLevelLoad - time))
            {
                time = Time.timeSinceLevelLoad;

                Owner.SetTarget(goal);
            }
        }

        protected override void OnChangeState()
        {
            Destroy(Owner.state_AI);
            Owner.state_AI = Owner.gameObject.AddComponent<EnemyState_Wait>();
            Owner.state_AI.SetOwner(Owner);

            Debug.Log("ステートが変わった : EnemyState_Wait");
        }
    }
}