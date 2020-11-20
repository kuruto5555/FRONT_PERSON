using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Enemy;
using FrontPerson.Weapon;

namespace FrontPerson.Enemy
{
    public class Yakuza : Character.Enemy
    {
        protected override void OnStart()
        {
            Type = EnemyType.YAKUZA;

            Set_LackVitamin();
            Set_LackVitamin_Text();
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
        //    }
        //}

        public override void HitBullet(Bullet bullet)
        {
            AddVitamins(bullet.Power);

            if (insufficiency <= 0)
            {
                SetDestroy();
            }
        }
    }
}