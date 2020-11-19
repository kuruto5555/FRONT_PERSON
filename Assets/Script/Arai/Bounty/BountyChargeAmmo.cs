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
        void Start()
        {
            base.Start();

            _progressString = _numCharge.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            base.Update();

            _numCharge += _Bmanager.GetNumNutritionCharge();
            _progressString = _numCharge.ToString();

            if (_numCharge > MaxCharge ) MissionClear();
        }
    }
}