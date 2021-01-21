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
        /// 怒っているかのフラグ(true = 怒っている)
        /// </summary>
        public bool isAngry { get; set; } = false;

        /// <summary>
        /// プレイヤーを追っかけている
        /// </summary>
        public bool isDiscover { get; set; } = false;

        /// <summary>
        /// 1回しか実行させない為の変数
        /// </summary>
        private bool Enable = true;

        private int insufficiency_ = 0;

        protected override void OnAwake()
        {
            Type = EnemyType.OLD_BATTLEAXE;
        }

        protected override void OnStart()
        {
            insufficiency_ = insufficiency;
        }

        protected override void OnUpdate()
        {
        }

        public override void HitBullet(Bullet bullet)
        {
            if (isDown) return;

            if (isAngry)
            {
                if(Enable)
                {
                    Enable = false;

                    insufficiency = insufficiency_;
                }

                AddVitamins(bullet.Power);

                Down();
            }
            else
            {
                if (lack_vitamins == bullet.BulletType || Constants.NUTRIENTS_TYPE._ALL == bullet.BulletType)
                {
                    AddVitamins(bullet.Power);

                    Down();
                }
                else
                {
                    // 弾の種類と足りないビタミンが違う
                    isAngry = true;

                    Enable = true;

                    // コンボの終了
                    ComboManager.Instance.LostCombo();

                    AudioManager.Instance.Play3DSE(transform.position, SEPath.GAME_SE_VOICE_WOMAN);
                }
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

        private void Down()
        {
            if (insufficiency <= 0)
            {
                SetDown();

                // バウンティの処理
                var bounty_manager = GameObject.FindGameObjectWithTag(Constants.TagName.BOUNTY_MANAGER).GetComponent<BountyManager>();

                bounty_manager.EnemyDeath((int)lack_vitamins);

                Animation();
            }
        }
    }
}