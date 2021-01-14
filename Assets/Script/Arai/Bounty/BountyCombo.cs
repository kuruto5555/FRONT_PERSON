using FrontPerson.Manager;
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

        /// <summary>
        /// ミッション開始時のコンボ数
        /// </summary>
        int startCombo = 0;

        // Start is called before the first frame update
        new void Start()
        {
            base.Start();

            _progressString = _Bmanager.GetNowCombo().ToString() + " / " + ComboMax.ToString();

            startCombo = _Bmanager.GetNowCombo();

            //_isStart = false;
            _missionName = MissionNames;
        }

        // Update is called once per frame
        public override void CheckUpdate()
        {
            base.CheckUpdate();

            int nowCombo = _Bmanager.GetNowCombo();

            // ミッション開始時のコンボ数より今のコンボのほうが小さかったら、
            // それはコンボが途切れたはずだからミッション開始時のコンボ数を0に初期化
            if (startCombo > nowCombo)
            {
                startCombo = 0;
            }

            _progressString = (nowCombo - startCombo).ToString() + " / " + ComboMax.ToString();

            if (nowCombo >= startCombo + ComboMax)
            {
                MissionClear();
            }
        }
    }
}