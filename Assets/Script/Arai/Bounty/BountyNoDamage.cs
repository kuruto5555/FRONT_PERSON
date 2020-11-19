﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Bounty
{


    public class BountyNoDamage : Bounty
    {
        // Start is called before the first frame update
        void Start()
        {
            base.Start();

            //_text.text = LimitTime + MissionName;
        }

        // Update is called once per frame
        void Update()
        {
            base.Update();

            if (_Bmanager.GetIsPlayerDamage()) MissionFailed();

            if (_nowTime < 0) MissionClear();
        }
    }
}