using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Weapon;
using FrontPerson.Enemy.AI;

namespace FrontPerson.Enemy
{
    public class OldBattleaxe : Character.Enemy
    {
        /// <summary>
        /// 違うビタミンを当てられたかのフラグ(true = 当てられた)
        /// </summary>
        public bool isHit { get; set; } = false;

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

                if (lack_vitamins != bullet.BulletType)
                {
                    // 弾の種類と足りないビタミンが違う

                    // 仮
                    Destroy(collision.gameObject);

                    isHit = true;

                    return;
                }

                // 弾の種類と足りないビタミンが同じ

                AddVitamins(bullet.Power);

                // 仮
                Destroy(collision.gameObject);

                isHit = false;

                if (insufficiency <= 0)
                {
                    SetDestroy();
                }
            }
        }
    }
}