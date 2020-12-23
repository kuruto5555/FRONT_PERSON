using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Weapon
{
    public class SpecialWeapon : Gun
    {
        // Start is called before the first frame update
        protected void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected void Update()
        {
            base.Update();
            //ChangeWeapon();
        }

        public virtual void ChangeWeapon()
        {
            if (Ammo <= 0)
            {
                Destroy(gameObject);
            }

            //アニメーション開始
            //_isAnimation = true;
        }

        public override void PutAnimation()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// 強制的に武器変更
        /// </summary>
        public virtual void WeaponForcedChange()
        {
            Destroy(gameObject);
        }
    }
}