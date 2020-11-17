using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrontPerson.Bounty
{
    public class BountyEnemyKill : Bounty
    {
        [Header("殺す数")]
        [SerializeField] int KillMax = 1;

        /// <summary>
        /// 敵を倒した数
        /// </summary>
        private int _killCnt = 0;

        public int KillCnt { get { return _killCnt; } }

        // Start is called before the first frame update
        void Start()
        {
            base.Start();

            
            _killCnt = 0;
            _text = GetComponent<Text>();

            _progressString = _killCnt.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            _killCnt += _Bmanager.GetNumEnemyDeath(); //今のフレーム死んだ数を数える

            LimitTime -= Time.deltaTime;

            //終わったら
            if (_killCnt > KillMax)
            {
                _isClear = _isFinish = true;
            }

            //制限時間が来たら
            if(_nowTime < 0)
            {
                _isFinish = true;
            }
        }

        void Plus()
        {
            _killCnt++;
        }

        
    }
}