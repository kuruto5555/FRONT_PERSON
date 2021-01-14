using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Weapon;
using FrontPerson.Manager;
using FrontPerson.Constants;

namespace FrontPerson.Enemy
{
    public class Yakuza : Character.Enemy
    {
        /// <summary>
        /// プレイヤーを追っかけている
        /// </summary>
        public bool isDiscover { get; set; } = false;

        protected override void OnAwake()
        {
            Type = EnemyType.YAKUZA;
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

            AddVitamins(bullet.Power);

            if (insufficiency <= 0)
            {
                SetDown();

                Animation();

                EmotionEmitter_.OpentFire(EMOTION_INDEX.SAD, 0.5f);
            }
        }

        private void Animation()
        {
            Animator.CrossFadeInFixedTime(EnemyAnimation.Escape, 0.5f);
        }
    }
}