using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Enemy.AI
{
    public class EnemyState_Attack : EnemyState_AI
    {
        //private SearchArea searchArea = null;

        public override void OnStart()
        {
            PlayAnimation(EnemyAnimation.Attack, 0.2f);

            Owner.isStoppingAnimation = true;
        }

        protected override void OnUpdate()
        {
            if (1f <= Owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                Owner.isStoppingAnimation = false;
            }
        }

        protected override void OnChangeState_OrdinaryPeople()
        {
        }

        protected override void OnChangeState_OldBattleaxe()
        {
            OldBattleaxe enemy = Owner as OldBattleaxe;

            enemy.isHit = false;
            enemy.isAngry = false;

            enemy.isDiscover = false;

            Player.Stun();

            ChangeState<EnemyState_Move>(EmotionEmitter_);
        }

        protected override void OnChangeState_Yakuza()
        {
            Player.Stun();

            var enemy = Owner as Yakuza;

            enemy.isDiscover = false;

            ChangeState<EnemyState_Move>(EmotionEmitter_);

            EmotionEmitter_.CloseFire();
        }
    }
}