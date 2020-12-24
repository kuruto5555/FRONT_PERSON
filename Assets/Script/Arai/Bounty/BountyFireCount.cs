using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FrontPerson.Bounty
{
    public class BountyFireCount : Bounty
    {
        [Header("撃たせる弾の数")]
        [SerializeField, Range(10, 100)]
        int FireMax = 10;

        private int _nowFireCount = 0;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();

            _nowFireCount = 0;

            _progressString = _nowFireCount.ToString() + " / " + FireMax.ToString();

            _missionName = MissionNames;
        }

        // Update is called once per frame
        public override void CheckUpdate()
        {
            base.CheckUpdate();

            _nowFireCount += _Bmanager.GetFireCount();

            _progressString = _nowFireCount.ToString() + " / " + FireMax.ToString();

            //クリア条件
            if (FireMax <= _nowFireCount)
            {
                MissionClear();
            }
        }
    }
}