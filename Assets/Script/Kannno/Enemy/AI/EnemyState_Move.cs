using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

using FrontPerson.Manager;
using FrontPerson.Constants;

namespace FrontPerson.Enemy.AI
{
    public class EnemyState_Move : EnemyState_AI
    {
        [Header("移動ルート")]
        [SerializeField]
        private MovePattern MovePattern  = null;

        [Header("半径からの距離")]
        [SerializeField]
        private float RadiusOffset = 0.3f;

        public void Set_MovePattern(MovePattern move_pattern)
        {
            MovePattern = move_pattern;
        }

        /// <summary>
        /// 移動先の一覧
        /// </summary>
        private List<Vector3> MovePoint_List = new List<Vector3>();

        /// <summary>
        /// 現在のMovePointのインデックス
        /// </summary>
        private int MovePointIndex = 0;

        public override void OnStart()
        {
            if (0 == MoveIndex)
            {
                MovePointIndex = 0;
            }
            else
            {
                MovePointIndex = MoveIndex;
            }

            if (MovePattern)
            {
                Set_MovePoint();

                Owner.SetTarget(MovePoint_List.First());
            }
            else
            {
                if (0 != MoveList.Count)
                {
                    MovePoint_List = MoveList;

                    Owner.SetTarget(MovePoint_List[MovePointIndex]);
                }
#if UNITY_EDITOR
                else
                {
                    Debug.LogError("MovePattern が設定されていません");
                }
#endif
            }
        }

        /// <summary>
        /// MovePoint の設定する関数
        /// </summary>
        private void Set_MovePoint()
        {
            var list = new List<MovePoint>();

            list.AddRange(MovePattern.GetComponentsInChildren<MovePoint>());

            list.Sort((a, b) => a.Index - b.Index);

#if UNITY_EDITOR
            if (null == list)
            {
                Debug.LogError("MovePoint が存在しません");
                return;
            }
#endif

            foreach (var obj in list)
            {
                MovePoint_List.Add(obj.transform.position);
            }
        }

        private void SetMovePoint()
        {
            MoveIndex = MovePointIndex;

            MoveList = MovePoint_List;
        }

        protected override void OnUpdate()
        {
#if UNITY_EDITOR
            if (null == MovePoint_List || 0 == MovePoint_List.Count) return;
#endif

            // 経路探索中なら、調べない
            if (false == Owner.Agent.pathPending)
            {
                // 目的地についていたら次の目的地の方に行く
                if (Owner.Agent.remainingDistance <= 1f)
                {
                    Vector3 destination = MovePoint_List[(MovePointIndex + 1) % MovePoint_List.Count];

                    Owner.SetTarget(destination);

                    MovePointIndex = (MovePointIndex + 1) % MovePoint_List.Count;
                }
            }
        }

        protected override void OnChangeState_OrdinaryPeople()
        {
        }

        protected override void OnChangeState_OldBattleaxe()
        {
            if (null == Player) return;

            OldBattleaxe enemy = Owner as OldBattleaxe;

            if (Player.IsStun || Player.IsInvincible || Player.IsTransparent)
            {
                if (enemy.isHit) enemy.isHit = false;
                return;
            }

            if (enemy.isHit)
            {
                SetMovePoint();

                ChangeState<EnemyState_Close>();

                var ai = Owner.state_AI as EnemyState_Close;

                ClalPosition();
            }
        }

        protected override void OnChangeState_Yakuza()
        {
            if (null == Player) return;

            if (Player.IsStun || Player.IsInvincible || Player.IsTransparent) return;

            if (SearchArea.IsFound)
            {
                SetMovePoint();

                ChangeState<EnemyState_Close>();

                var ai = Owner.state_AI as EnemyState_Close;

                ClalPosition();

                AudioManager.Instance.Play3DSE(Owner.transform.position, SEPath.GAME_SE_VOICE_YAKUZA);
            }
        }

        /// <summary>
        /// 半径(ナビゲーション)分のオフセットを計算
        /// </summary>
        private void ClalPosition()
        {
            var ai = Owner.state_AI as EnemyState_Close;

            Vector3 vec_enemy = transform.position - Player.transform.position;
            vec_enemy.y = 0f;

            Vector3 vec_radius = Vector3.Normalize(vec_enemy) * (Player.GetComponent<NavMeshObstacle>().radius + RadiusOffset);

            ai.Goal = Player.transform;
            ai.Offset = vec_radius;
        }
    }
}