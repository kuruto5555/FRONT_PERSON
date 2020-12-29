using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Weapon;
using FrontPerson.Manager;

namespace FrontPerson.Enemy
{
    public class Yakuza : Character.Enemy
    {
        /// <summary>
        /// 撃退を表すアニメーションフラグ
        /// </summary>
        public bool isRepel_anime { get; set; } = false;

        /// <summary>
        /// 敵に見つかったを表すアニメーションフラグ
        /// </summary>
        public bool isDiscovery_anime { get; set; } = false;

        protected override void OnAwake()
        {
            Type = EnemyType.YAKUZA;
        }

        protected override void OnStart()
        {
        }

        protected override void OnUpdate()
        {
            Animation();
        }

        public override void HitBullet(Bullet bullet)
        {
            if (isDown) return;

            AddVitamins(bullet.Power);

            if (insufficiency <= 0)
            {
                SetDown();

                isRepel_anime = true;
            }
        }

        private void Animation()
        {
            Animator.SetBool("isDiscovery", isDiscovery_anime);
            Animator.SetBool("isRepel", isRepel_anime);
        }
    }
}