﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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