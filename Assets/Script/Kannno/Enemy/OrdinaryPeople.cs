using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Enemy;
using FrontPerson.Weapon;

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

        void OnCollisionEnter(Collision collision)
        {
            if (Constants.TagName.BULLET == collision.gameObject.tag)
            {
                Bullet bullet = collision.gameObject.GetComponent<Bullet>();

                if (lack_vitamins == bullet.BulletType)
                {
                    AddVitamins(bullet.Power);

                    if (insufficiency <= 0)
                    {
                        SetDestroy();
                    }
                }
            }
        }
    }
}