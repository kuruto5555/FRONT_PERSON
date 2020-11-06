using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Weapon;
using FrontPerson.Enemy.AI;

namespace FrontPerson.Enemy
{
    public class OrdinaryPeople : Enemy
    {
        protected override void OnStart()
        {
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

                if (insufficiency <= 0)
                {
                    SetDestroy();
                }
            }
        }
    }
}