using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Bounty
{
    public class BountyChargeAmmo : Bounty
    {
        [Header("補充回数")]
        [SerializeField, Range(1, 10)]
        int MaxCharge = 5;

        private int _numCharge = 0;

        // Start is called before the first frame update
        new void Start()
        {
            base.Start();

            _numCharge = 0;
            _progressString = _numCharge.ToString() + " / " + MaxCharge.ToString();
            _missionName = MissionNames;
        }

        // Update is called once per frame
        public override void CheckUpdate()
        {
            base.CheckUpdate();

            _numCharge += _Bmanager.GetNumNutritionCharge();
            _progressString = _numCharge.ToString() + " / " + MaxCharge.ToString();

            if (_numCharge >= MaxCharge ) MissionClear();
        }
    }
}