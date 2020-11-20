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

        //void OnTriggerEnter(Collider collider)
        //{
        //    if (Constants.TagName.BULLET == collider.tag)
        //    {
        //        Bullet bullet = collider.gameObject.GetComponent<Bullet>();

        //        AddVitamins(bullet.Power);

        //        if (insufficiency <= 0)
        //        {
        //            SetDestroy();
        //        }

        //        if (lack_vitamins != bullet.BulletType)
        //        {
        //            // 弾の種類と足りないビタミンが違う
        //            isHit = true;
        //        }
        //        else
        //        {
        //            isHit = false;
        //        }
        //    }
        //}

        public override void HitBullet(Bullet bullet)
        {
            AddVitamins(bullet.Power);

            if (insufficiency <= 0)
            {
                SetDestroy();
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