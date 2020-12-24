using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Bounty
{
    public class BountyCombo : Bounty
    {
        [Header("コンボ数")]
        [SerializeField,Range(1, 100)] 
        int ComboMax = 10;

        //private bool _isStart = false;

        public int GetComboMax { get { return ComboMax; } }

        // Start is called before the first frame update
        new void Start()
        {
            base.Start();

            _progressString = _Bmanager.GetNowCombo().ToString() + " / " + ComboMax.ToString();

            //_isStart = false;
            _missionName = MissionNames;
        }

        // Update is called once per frame
        public override void CheckUpdate()
        {
            base.CheckUpdate();

            int nowCombo = _Bmanager.GetNowCombo();

            _progressString = _Bmanager.GetNowCombo().ToString() + " / " + ComboMax.ToString();

            if (nowCombo >= ComboMax)
            {
                MissionClear();
            }
        }
    }
}