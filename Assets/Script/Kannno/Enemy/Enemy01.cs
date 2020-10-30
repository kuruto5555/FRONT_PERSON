using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using FrontPerson.Weapon;
using FrontPerson.Enemy.AI;

namespace FrontPerson.Enemy
{
    public class Enemy01 : Enemy
    {
        [Header("目的地")]
        [SerializeField]
        public Transform goal = null;

        private NavMeshAgent agent;

        protected override void OnStart()
        {
            agent = GetComponent<NavMeshAgent>();

            SetState(new EnemyState_Wait(this));

            state_AI.Start();
        }

        protected override void OnUpdate()
        {
            state_AI.Update();
        }
        protected override void OnCollisionEnter(Collision collision)
        {
            if (FrontPerson.Constants.TagName.BULLET == collision.gameObject.tag)
            {
                Bullet bullet = collision.gameObject.GetComponent<Bullet>();

                AddVitamins(bullet.Power);

                Destroy(collision.gameObject);

                if (insufficiency <= 0)
                {
                    SetDestroy();
                }
            }
        }

        public void SetTarget(Transform goal)
        {
            if (null != goal)
            {
                agent.destination = goal.position;
            }
            else
            {
                agent.destination = transform.position;
            }
        }
    }
}