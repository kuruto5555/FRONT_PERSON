using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Weapon
{
    public class AssaultRifle : SpecialWeapon
    {
        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            _type = Constants.WEAPON_TYPE.ASSAULT_RIFLE;
        }

        // Update is called once per frame
        void Update()
        {
            base.Update();
        }
    }
}