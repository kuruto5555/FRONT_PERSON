using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Weapon;
using FrontPerson.Manager;
using FrontPerson.Constants;

namespace FrontPerson.Enemy
{
    public class OldBattleaxe : Character.Enemy
    {
        /// <summary>
        /// 違うビタミンを当てられたかのフラグ(true = 当てられた)
        /// </summary>
        public bool isHit { get; set; } = false;

        /// <summary>
        /// 怒っているかのフラグ(true = 怒っている)
        /// </summary>
        public bool isAngry { get; set; } = false;

        /// <summary>
        /// プレイヤーを追っかけている
        /// </summary>
        public bool isDiscover { get; set; } = false;

        protected override void OnAwake()
        {
            Type = EnemyType.OLD_BATTLEAXE;
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

                // バウンティの処理
                var bounty_manager = GameObject.FindGameObjectWithTag(Constants.TagName.BOUNTY_MANAGER).GetComponent<BountyManager>();

                bounty_manager.EnemyDeath((int)lack_vitamins);

                Animation();

                return;
            }

            if (lack_vitamins != bullet.BulletType && Constants.NUTRIENTS_TYPE._ALL != bullet.BulletType)
            {
                // 弾の種類と足りないビタミンが違う
                isHit = true;

                // コンボの終了
                ComboManager.Instance.LostCombo();

                EmotionEmitter_.OpentFire(EMOTION_INDEX.ANGRY, 0.8f);

                AudioManager.Instance.Play3DSE(transform.position, SEPath.GAME_SE_VOICE_WOMAN);
            }
            else
            {
                isHit = false;
            }
        }

        private void Animation()
        {
            isStoppingAnimation = true;

            if (isAngry)
            {
                EmotionEmitter_.OpentFire(EMOTION_INDEX.SAD, 0.5f);
                Animator.CrossFadeInFixedTime(EnemyAnimation.Repel, 0.5f);
            }
            else
            {
                EmotionEmitter_.OpentFire(EMOTION_INDEX.HAPPY, 1.0f);
                Animator.CrossFadeInFixedTime(EnemyAnimation.Fine, 0.5f);
            }
        }
    }
}