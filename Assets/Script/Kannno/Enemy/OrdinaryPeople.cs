﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Enemy;
using FrontPerson.Weapon;
using FrontPerson.Manager;

namespace FrontPerson.Enemy
{
    public class OrdinaryPeople : Character.Enemy
    {
        protected override void OnAwake()
        {
            Type = EnemyType.ORDINATY_PEOPLE;
        }

        protected override void OnStart()
        {
        }

        protected override void OnUpdate()
        {
        }

        public override void HitBullet(Bullet bullet)
        {
            if (isDown) return;

            if (lack_vitamins == bullet.BulletType || Constants.NUTRIENTS_TYPE._ALL == bullet.BulletType)
            {
                AddVitamins(bullet.Power);

                if (insufficiency <= 0)
                {
                    SetDown();

                    // バウンティの処理
                    var bounty_manager = GameObject.FindGameObjectWithTag(Constants.TagName.BOUNTY_MANAGER).GetComponent<BountyManager>();

                    bounty_manager.EnemyDeath((int)lack_vitamins);
                }
            }
        }
    }
}