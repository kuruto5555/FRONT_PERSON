using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Enemy;
using FrontPerson.Weapon;
using FrontPerson.Manager;
using FrontPerson.Constants;

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

                    Animator.CrossFadeInFixedTime(EnemyAnimation.Fine, 0.5f);

                    isStoppingAnimation = true;

                    EmotionEmitter_.OpentFire(EMOTION_INDEX.HAPPY, 1.0f);
                }
            }
            else
            {
                // コンボの終了
                ComboManager.Instance.LostCombo();

                AudioManager.Instance.Play3DSE(transform.position, SEPath.GAME_SE_VOICE_MAN);

                Animator.CrossFadeInFixedTime(EnemyAnimation.Angry, 0.5f);

                isStoppingAnimation = true;

                EmotionEmitter_.OpentFire(EMOTION_INDEX.SAD, 0.3f, 3);
            }
        }
    }
}