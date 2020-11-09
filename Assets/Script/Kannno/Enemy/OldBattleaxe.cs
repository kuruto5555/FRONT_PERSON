﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Weapon;

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