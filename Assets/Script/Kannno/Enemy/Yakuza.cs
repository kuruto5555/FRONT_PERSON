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

        public override void HitBullet(Bullet bullet)
        {
            if (isDown) return;

            AddVitamins(bullet.Power);

            if (insufficiency <= 0)
            {
                SetDown();
            }
        }
    }
}