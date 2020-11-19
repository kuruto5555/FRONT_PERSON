﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FrontPerson.Constants;

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

        private int rand = 0;

        public int GetKillCnt { get { return _killCnt; } }


        // Start is called before the first frame update
        void Start()
        {
            base.Start();
   
            _killCnt = 0;

            rand = Random.Range(0, 2);

            _progressString = _killCnt.ToString();

            MissionName = Nutrients.Type[(int)rand] + MissionName;
        }

        // Update is called once per frame
        void Update()
        {
            base.Update();

            if(rand == 0)
            {
                _killCnt += _Bmanager.GetNumEnemyDeath().x; //今のフレーム死んだ数を数える
            }

            if (rand == 1)
            {
                _killCnt += _Bmanager.GetNumEnemyDeath().y; //今のフレーム死んだ数を数える
            }

            _progressString = _killCnt.ToString();

            //クリア条件
            if (_killCnt > KillMax)
            {
                _isClear = _isFinish = true;
            }

            if (_nowTime < 0)
            {
                _isFinish = true;
            }
        }
    }
}