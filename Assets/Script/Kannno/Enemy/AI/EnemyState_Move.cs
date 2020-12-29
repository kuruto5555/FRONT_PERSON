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
        public override void OnStart()
        {
            if (Owner.MovePattern)
            {
                Set_MovePoint();

                Owner.SetTarget(Owner.MoveList[Owner.MoveIndex]);
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError("MovePattern が設定されていません");
            }
#endif
        }

        /// <summary>
        /// MovePoint の設定する関数
        /// </summary>
        private void Set_MovePoint()
        {
            var list = new List<MovePoint>();

            list.AddRange(Owner.MovePattern.GetComponentsInChildren<MovePoint>());

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
                Owner.MoveList.Add(obj.transform.position);
            }
        }

        protected override void OnUpdate()
        {
#if UNITY_EDITOR
            if (null == Owner.MoveList || 0 == Owner.MoveList.Count) return;
#endif

            // 経路探索中なら、調べない
            if (false == Owner.Agent.pathPending)
            {
                // 目的地についていたら次の目的地の方に行く
                if (Owner.Agent.remainingDistance <= 2f)
                {
                    Vector3 destination = Owner.MoveList[(Owner.MoveIndex + 1) % Owner.MoveList.Count];

                    Owner.SetTarget(destination);

                    Owner.MoveIndex = (Owner.MoveIndex + 1) % Owner.MoveList.Count;
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

            //enemy.ResetAttack();

            if (Player.IsStun || Player.IsInvincible || Player.IsTransparent)
            {
                if (enemy.isHit) enemy.isHit = false;
                return;
            }

            if (enemy.isHit)
            {
                ChangeState<EnemyState_Close>();

                var ai = Owner.state_AI as EnemyState_Close;
            }
        }

        protected override void OnChangeState_Yakuza()
        {
            var enemy = Owner as Yakuza;

            if (null == Player) return;

            if (Player.IsStun || Player.IsInvincible || Player.IsTransparent)
            {
                enemy.isDiscovery_anime = false;
                return;
            }

            if (SearchArea.IsFound)
            {
                ChangeState<EnemyState_Close>();

                enemy.isDiscovery_anime = true;

                AudioManager.Instance.Play3DSE(Owner.transform.position, SEPath.GAME_SE_VOICE_YAKUZA);
            }
        }
    }
}