using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Bounty;

namespace FrontPerson.Manager
{
    public class BountyManager : MonoBehaviour
    {
        public List<Bounty.Bounty> MissionList {
            get;
            private set;
        }

        [SerializeField] List<GameObject> MissionPrefabList;

        private const int ACTIV_MISSION = 3;

        /// <summary>
        /// 3ミッションずつ武器を出すため
        /// </summary>
        private int _missionCnt = 0;

        /// <summary>
        /// ミッションの総数
        /// </summary>
        private int _missionNum = 0;

        /// <summary>
        /// このフレーム何人死んだか
        /// </summary>
        private int _numEnemyDeathA = 0;
        private int _numEnemyDeathB = 0;

        /// <summary>
        /// 今何コンボか
        /// </summary>
        private int _nowCombo = 0;

        /// <summary>
        /// このフレーム何発撃ったか
        /// </summary>
        private int _fireCount = 0;

        /// <summary>
        /// プレイヤーがダメージを負ったか
        /// </summary>
        private bool _isPlayerDamage = false;

        /// <summary>
        /// このフレームチャージしたかどうか
        /// </summary>
        private int _numNutritionCharge = 0;

        public List<Bounty.Bounty> GetBountyList { get { return MissionList; } }

        //private List <GameObject> 

        // Start is called before the first frame update
        void Start()
        {
            _missionNum = MissionPrefabList.Count;
            _missionCnt = 0;
            _nowCombo = 0;
            _fireCount = 0;
            _isPlayerDamage = false;
            MissionList = new List<Bounty.Bounty>();

            for (int i = 0; i < ACTIV_MISSION; i++)
            {
                MissionList.Add(Instantiate(MissionPrefabList[Random.Range(0, _missionNum)].GetComponent<Bounty.Bounty>(), transform)) ;
            }
        }

        // Update is called once per frame
        void Update()
        {
            int i = 0;
            foreach (var it in MissionList)
            {
                
                if (it.IsFinish)
                {
                    if (it.IsCrear) 
                    {
                        //スコア加算(it.GetScore();)

                        _missionCnt++;
                        if (_missionCnt == 3)
                        {
                            //武器を出す
                        }
                    }
                    
                    MissionList[i].ImDie();
                    MissionList[i] = Instantiate(MissionPrefabList[Random.Range(0, _missionNum)], transform).GetComponent<Bounty.Bounty>();
                    
                }
                i++;
            }
        }

        private void LateUpdate()
        {
            //最後に初期化してほしい
            _numEnemyDeathA = 0;
            _numEnemyDeathB = 0;
            _fireCount = 0;
            _isPlayerDamage = false;
            _numNutritionCharge = 0;
        }

        /// <summary>
        /// このフレーム何人死んだか
        /// </summary>
        /// <returns></returns>
        public Vector2Int GetNumEnemyDeath()
        {
            return new Vector2Int(_numEnemyDeathA, _numEnemyDeathB);
        }

        /// <summary>
        /// エネミーが死んだとき呼んでね
        /// </summary>
        /// <param name="type">0ならA,1ならB</param>
        public void EnemyDeath(int type)
        {
            if(type == 0)
            {
                _numEnemyDeathA++;
            }
            else
            {
                _numEnemyDeathB++;
            }
            
        }

        /// <summary>
        /// 今のコンボ数(コンボマネージャーに丸投げ)
        /// </summary>
        /// <returns></returns>
        public int GetNowCombo()
        {
            return _nowCombo;
        }

        /// <summary>
        /// コンボ数を管理してる所で呼んでね
        /// </summary>
        /// <param name="combo">今のコンボ数</param>
        public void SetNowCombo(int combo)
        {
            _nowCombo = combo;
        }

        /// <summary>
        /// 武器を発射する所で呼んで
        /// </summary>
        public void FireCount()
        {
            _fireCount++;
        }

        public int GetFireCount()
        {
            return _fireCount;
        }

        /// <summary>
        /// プレイヤーがダメージを負ったら読んで
        /// </summary>
        public void PlayerDamage()
        {
            _isPlayerDamage = true;
        }

        /// <summary>
        /// そのフレームプレイヤーがダメージを負ったか
        /// </summary>
        /// <returns></returns>
        public bool GetIsPlayerDamage()
        {
            return _isPlayerDamage;
        }

        /// <summary>
        /// 補給が終わったタイミングで呼ぶ
        /// </summary>
        public void NutritionCharge()
        {
            _numNutritionCharge++;
        }

        /// <summary>
        /// そのフレーム補給が呼ばれた回数
        /// </summary>
        /// <returns></returns>
        public int GetNumNutritionCharge()
        {
            return _numNutritionCharge;
        }
    }
}


