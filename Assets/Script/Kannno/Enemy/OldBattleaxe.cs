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
        /// 撃退を表すアニメーションフラグ
        /// </summary>
        protected bool isRepel_anime = false;

        protected override void OnAwake()
        {
            Type = EnemyType.OLD_BATTLEAXE;
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

                Angry();

                // バウンティの処理
                var bounty_manager = GameObject.FindGameObjectWithTag(Constants.TagName.BOUNTY_MANAGER).GetComponent<BountyManager>();

                bounty_manager.EnemyDeath((int)lack_vitamins);
            }

            if (lack_vitamins != bullet.BulletType && Constants.NUTRIENTS_TYPE._ALL != bullet.BulletType)
            {
                // 弾の種類と足りないビタミンが違う
                isHit = true;

                isAngry = true;

                // コンボの終了
                ComboManager.Instance.LostCombo();

                AudioManager.Instance.Play3DSE(transform.position, SEPath.GAME_SE_VOICE_WOMAN);
            }
            else
            {
                isHit = false;
            }
        }

        public void AttackTriggrr()
        {
            Animator.SetTrigger("Attack");
        }

        public void ResetAttackTriggrr()
        {
            Animator.ResetTrigger("Attack");
        }

        private void Angry()
        {
            if (isAngry)
            {
                isRepel_anime = true;
            }
            else
            {
                isRepel_anime = false;
                isFine_anime = true;
            }
        }

        private void Animation()
        {
            Animator.SetBool("isFine", isFine_anime);

            Animator.SetBool("isAngry", isAngry);
            Animator.SetBool("isRepel", isRepel_anime);
        }
    }
}