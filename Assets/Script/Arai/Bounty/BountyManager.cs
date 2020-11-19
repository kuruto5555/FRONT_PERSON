using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Bounty
{
    public class BountyManager : MonoBehaviour
    {
        [SerializeField] List<Bounty> MissionList;

        [SerializeField] List<GameObject> MissionPrefabList;

        /// <summary>
        /// このフレーム何人死んだか
        /// </summary>
        private int _numEnemyDeath = 0;


        /// <summary>
        /// 3ミッションずつ武器を出すため
        /// </summary>
        private int _missionCnt = 0;

        private int _missionNum = 0;

        public List<Bounty> GetBountyList { get { return MissionList; } }

        //private List <GameObject> 

        // Start is called before the first frame update
        void Start()
        {
            _missionNum = MissionPrefabList.Count;
            _missionCnt = 0;
        }

        // Update is called once per frame
        void Update()
        {
            foreach(var it in MissionList)
            {
                int i = 0; 
                if (it.IsFinish)
                {
                    if (it.IsCrear) _missionCnt++;
                    if(_missionCnt == 3) //武器を出す
                    MissionList[i].ImDie();
                    MissionList[i] = Instantiate(MissionPrefabList[Random.Range(0, _missionNum)]).GetComponent<Bounty>();
                    
                }
                i++;
            }
        }

        private void LateUpdate()
        {
            //最後に初期化してほしい
            _numEnemyDeath = 0;
        }

        /// <summary>
        /// このフレーム何人死んだか
        /// </summary>
        /// <returns></returns>
        public int GetNumEnemyDeath()
        {
            return _numEnemyDeath;
        }

        /// <summary>
        /// エネミーが死んだとき呼んでね
        /// </summary>
        public void EnemyDeath()
        {
            _numEnemyDeath++;
        }
    }
}


