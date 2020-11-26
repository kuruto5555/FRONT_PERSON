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
            ChangeWeapon();
        }

        protected void ChangeWeapon()
        {
            if (Ammo <= 0)
            {
                Destroy(gameObject);
            }
            
        }
    }
}