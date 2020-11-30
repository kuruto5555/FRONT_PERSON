using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Weapon;
using FrontPerson.Enemy;
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
            Type = EnemyType.OLD_BATTLEAXE;
        }

        protected override void OnUpdate()
        {
        }

        public override void HitBullet(Bullet bullet)
        {
            if (isDown) return;

            AddVitamins(bullet.Power);

            if (insufficiency <= 0)
            {
                SetDown();
            }

            if (lack_vitamins != bullet.BulletType)
            {
                // 弾の種類と足りないビタミンが違う
                isHit = true;
            }
            else
            {
                isHit = false;
            }
        }
    }
}