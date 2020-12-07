using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FrontPerson.Weapon;
using FrontPerson.Manager;

namespace FrontPerson.Enemy
{
    public class OldBattleaxe : Character.Enemy
    {
        /// <summary>
        /// 違うビタミンを当てられたかのフラグ(true = 当てられた)
        /// </summary>
        public bool isHit { get; set; } = false;

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

                // スコア加算
                var score_manager = ScoreManager.Instance;

                score_manager.AddScore((int)EnemyScore.OLD_BATTLEAXE, Score.ReasonForAddition.Nomal);
            }

            if (lack_vitamins != bullet.BulletType && Constants.NUTRIENTS_TYPE._ALL != bullet.BulletType)
            {
                // 弾の種類と足りないビタミンが違う
                isHit = true;
            }
            else
            {
                isHit = false;
            }
        }
    }
}