using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Bounty
{
    public class BountyNoMissCombo : Bounty
    {
        /// <summary>
        /// コンボが始まっているか
        /// </summary>
        private bool _isComboStart = false;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();

            _isComboStart = false;

            _progressString = "残り " + LimitTime.ToString("00") + "秒";

            _missionName = MissionNames;
        }

        // Update is called once per frame
        void Update()
        {
            base.Update();
            
            int nowCombo = _Bmanager.GetNowCombo();

            if (!_isComboStart)
            {
                if (nowCombo >= 1) _isComboStart = true;
                //if(GetLimitTime < GetNowTime) _isClear = _isFinish = true;
                _nowTime = LimitTime; //コンボが始まるまでカウントダウンを止めておく
            }
            else //コンボがスタートしてる間
            {
                _progressString = "残り " + _nowTime.ToString("00") + "秒";

                if (0 > GetNowTime) 
                    MissionClear();

                //コンボが初期化されたら失敗
                if (nowCombo == 0) 
                    MissionFailed();
                
            }
        }
    }
}