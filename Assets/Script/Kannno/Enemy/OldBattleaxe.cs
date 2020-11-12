using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Weapon;
using FrontPerson.Enemy.AI;

namespace FrontPerson.Enemy
{
    public class OldBattleaxe : Character.Enemy
    {
        protected override void OnStart()
        {
            Type = Character.EnemyType.OLD_BATTLEAXE;
        }

        protected override void OnUpdate()
        {
        }

        void OnCollisionEnter(Collision collision)
        {
            if (Constants.TagName.BULLET == collision.gameObject.tag)
            {
                Bullet bullet = collision.gameObject.GetComponent<Bullet>();

                //if(lack_vitamins != bullet.bulletType_)
                //{
                //    // 弾の種類と足りないビタミンが違う

                //    // 仮
                //    Destroy(collision.gameObject);

                //    state_AI.ChangeState<EnemyState_Close>();
                //}

                // 弾の種類と足りないビタミンが同じ

                AddVitamins(bullet.Power);

                // 仮
                Destroy(collision.gameObject);

                if(insufficiency <= 0)
                {
                    SetDestroy();
                }
            }
        }
    }
}