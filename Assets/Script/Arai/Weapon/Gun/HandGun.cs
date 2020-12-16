using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Constants;
using FrontPerson.Manager;

namespace FrontPerson.Weapon
{

    public class HandGun : Gun
    {
        private 
        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            _type = Constants.WEAPON_TYPE.HANDGUN;
            _shotSoundPath = SEPath.GAME_SE_FIRE_HANDGUN;
        }

        // Update is called once per frame
        void Update()
        {
            base.Update();
        }
    }
}