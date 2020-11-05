using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FrontPerson.Enemy
{
    public class Enemy02 : Enemy
    {
        [Header("目的地")]
        [SerializeField]
        private Transform goal = null;

        private NavMeshAgent agent;

        protected override void OnStart()
        {
            agent = GetComponent<NavMeshAgent>();

            SetState(new EnemyState_Wait(this));

            state_AI.Start();
        }

        protected override void OnUpdate()
        {
            agent.destination = goal.position;

            state_AI.Update();
        }
    }
}