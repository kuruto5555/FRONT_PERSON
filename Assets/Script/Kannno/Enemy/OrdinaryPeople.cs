using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Enemy;
using FrontPerson.Weapon;
using FrontPerson.Manager;

namespace FrontPerson.Enemy
{
    public class OrdinaryPeople : Character.Enemy
    {
        protected override void OnStart()
        {
            Type = EnemyType.ORDINATY_PEOPLE;
        }

        protected override void OnUpdate()
        {
        }

        public override void HitBullet(Bullet bullet)
        {
            if (lack_vitamins == bullet.BulletType)
            {
                AddVitamins(bullet.Power);

                if (insufficiency <= 0)
                {
                    SetDestroy();

                    var manager = GameObject.FindGameObjectWithTag(Constants.TagName.BOUNTY_MANAGER).GetComponent<BountyManager>();

                    manager.EnemyDeath((int)lack_vitamins);
                }
            }
        }
    }
}