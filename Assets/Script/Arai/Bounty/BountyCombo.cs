using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Bounty
{
    public class BountyCombo : Bounty
    {
        [Header("コンボ数")]
        [SerializeField,Range(1, 100)] int ComboMax = 10;

        //private bool _isStart = false;

        public int GetComboMax { get { return ComboMax; } }

        // Start is called before the first frame update
        void Start()
        {
            base.Start();

            _progressString = _Bmanager.GetNowCombo().ToString();

            //_isStart = false;
            MissionName = ComboMax + MissionName;
        }

        // Update is called once per frame
        void Update()
        {
            base.Update();

            int nowCombo = _Bmanager.GetNowCombo();

            _progressString = nowCombo.ToString();

            //if (!_isStart)
            //{
            //    if (nowCombo > 1) _isStart = true;
            //}
            //else //１度コンボをミスしたら終了
            //{
            //    if (nowCombo == 0) _isFinish = true;
            //}

            if(nowCombo > ComboMax)
            {
                MissionClear();
            }
        }
    }
}