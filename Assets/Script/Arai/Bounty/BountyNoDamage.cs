using System.Collections;
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

            _progressString = "残り " + LimitTime.ToString("00") + "秒";

            _missionName = MissionNames;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            _progressString = "残り " + _nowTime.ToString("00") + "秒";

            if (_Bmanager.GetIsPlayerDamage()) 
                MissionFailed();

            if (_nowTime < 0) MissionClear();
        }
    }
}