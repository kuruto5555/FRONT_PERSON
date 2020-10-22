using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FrontPerson
{
    public class Enemy01 : Enemy<Enemy01>
    {
        [Header("目的地")]
        [SerializeField]
        private Transform goal = null;

        private NavMeshAgent agent;

        protected override void OnStart()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        protected override void OnUpdate()
        {
            agent.destination = goal.position;
        }
    }
}