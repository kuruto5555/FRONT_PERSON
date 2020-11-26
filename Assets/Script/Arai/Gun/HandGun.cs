using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Weapon
{

    public class HandGun : Gun
    {
        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            _type = Constants.WEAPON_TYPE.HANDGUN;
        }

        // Update is called once per frame
        void Update()
        {
            base.Update();
        }
    }
}