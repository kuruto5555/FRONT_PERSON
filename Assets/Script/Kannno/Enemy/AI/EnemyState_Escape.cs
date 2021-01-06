using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

using FrontPerson.Enemy;

namespace FrontPerson.Enemy.AI
{
    public class EnemyState_Escape : EnemyState_AI
    {
        private Vector3 Destination = new Vector3();
        

        public override void OnStart()
        {
            Set_AgentGoal();

            if(EnemyType.YAKUZA != Owner.Type) Invoke("StoppingAnimation", 1.0f);
        }

        /// <summary>
        /// 移動先を選択する
        /// </summary>
        private void Set_AgentGoal()
        {
            // 1番近いスポナーを探して、そこに移動する

            var spawners = GameObject.FindGameObjectsWithTag(Constants.TagName.SPAWNER);

            List<Vector3> spawners_position = new List<Vector3>();

            for (int i = 0; i < spawners.Length; i++)
            {
                spawners_position.Add(spawners[i].transform.position);
            }

            float escape_distance = 0f;
            Vector3 escape_pos = new Vector3();

            foreach (var pos in spawners_position)
            {
                float distance = (Owner.transform.position - pos).sqrMagnitude;

                if (distance < escape_distance || 0f == escape_distance)
                {
                    escape_distance = distance;
                    escape_pos = pos;
                }
            }

            Destination = escape_pos;

            Owner.SetTarget(escape_pos);
        }

        protected override void OnUpdate()
        {
            // 目的地がUnity側で変更されていたら
            if (false == CheckDestination())
            {
                Owner.SetTarget(Destination);
                return;
            }

            // 経路探索中なら、調べない
            if (false == Owner.Agent.pathPending)
            {
                // 目的地についていたら敵の消す
                if (Owner.Agent.remainingDistance <= 10f)
                {
                    Owner.SetDead();
                }
            }
        }

        protected override void OnChangeState_OrdinaryPeople()
        {
        }

        protected override void OnChangeState_OldBattleaxe()
        {
        }

        protected override void OnChangeState_Yakuza()
        {
        }

        private bool CheckDestination()
        {
            var dest = Owner.Agent.destination;

            if (Destination.x != dest.x)
            {
                return false;
            }

            if (Destination.z != dest.z)
            {
                return false;
            }

            return true;
        }

        private void StoppingAnimation()
        {
            Owner.isStoppingAnimation = false;
        }
    }
}