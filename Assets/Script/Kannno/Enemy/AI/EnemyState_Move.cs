using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FrontPerson.Enemy.AI
{
    public class EnemyState_Move : EnemyState_AI
    {
        [Header("移動ルート")]
        [SerializeField]
        private MovePattern MovePattern = null;

        public void Set_MovePattern(MovePattern move_pattern)
        {
            MovePattern = move_pattern;
        }

        /// <summary>
        /// 移動先の一覧
        /// </summary>
        private List<Transform> MovePoint_List = new List<Transform>();

        /// <summary>
        /// 現在のMovePointのインデックス
        /// </summary>
        private int MovePointIndex = 0;

        protected override void OnStart()
        {
            if (MovePattern)
            {
                Set_MovePoint();

                Owner.SetTarget(MovePoint_List.First());
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError("MovePattern が設定されていません");
            }
#endif
        }

        protected override void OnUpdate()
        {
            // 目的地についていたら次の目的地の方に行く
            if (Owner.Agent.remainingDistance <= 0.1f)
            {
                Vector3 destination = MovePoint_List[(MovePointIndex + 1) % MovePoint_List.Count].position;

                Owner.Agent.SetDestination(destination);

                MovePointIndex = (MovePointIndex + 1) % MovePoint_List.Count;
            }
        }

        protected override void OnChangeState_OrdinaryPeople()
        {
        }

        protected override void OnChangeState_OldBattleaxe()
        {
            OldBattleaxe enemy = Owner as OldBattleaxe;

            if (enemy.isHit)
            {
                ChangeState<EnemyState_Close>();
            }
        }

        protected override void OnChangeState_Yakuza()
        {
        }

        /// <summary>
        /// MovePoint の設定する関数
        /// </summary>
        private void Set_MovePoint()
        {
            var list = new List<MovePoint>();

            list.AddRange(MovePattern.GetComponentsInChildren<MovePoint>());

            if (null == list)
            {
                Debug.LogError("MovePoint が存在しません");
                return;
            }

            foreach (var obj in list)
            {
                MovePoint_List.Add(obj.transform);
            }
        }
    }
}