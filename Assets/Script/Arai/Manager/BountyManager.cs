using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.common;
using FrontPerson.Bounty;

namespace FrontPerson.Manager
{
    public class BountyManager : MonoBehaviour
    {
        public static BountyManager _instance { get; private set;}

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

        private Character.Player _player = null;

        public List<Bounty.Bounty> GetBountyList { get { return MissionList; } }

        //private List <GameObject> 

        private void Awake()
        {
            if(_instance == null) _instance = this;

            MissionList = new List<Bounty.Bounty>();
            _missionNum = MissionPrefabList.Count;
            for (int i = 0; i < ACTIV_MISSION; i++)
            {
                MissionList.Add(Instantiate(MissionPrefabList[Random.Range(0, _missionNum)].GetComponent<Bounty.Bounty>(), transform)) ;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            _isPlayerDamage = false;
            
            _missionCnt = 0;
            _nowCombo = 0;
            _fireCount = 0;

            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character.Player>();

        }

        // Update is called once per frame
        void Update()
        {
            _isPlayerDamage = false;

            AllMissionChange();

            MissionMonitoring();
        }

        private void LateUpdate()
        {
            //最後に初期化してほしい
            _numEnemyDeathA = 0;
            _numEnemyDeathB = 0;
            _fireCount = 0;
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
            if (type == 1)
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

        private void AllMissionChange()
        {
            //デバッグ用ミッション全て変更
            if (Input.GetKeyDown(KeyCode.K))
            {

                for (int debug = 0; debug < ACTIV_MISSION; debug++)
                {
                    MissionList[debug].ImDie();
                    MissionList[debug] = Instantiate(MissionPrefabList[Random.Range(0, _missionNum)], transform).GetComponent<Bounty.Bounty>();
                }
            }
        }

        /// <summary>
        /// ミッションを監視してる
        /// </summary>
        private void MissionMonitoring()
        {
            int i = 0;

            foreach (var it in MissionList)
            {
                if (it.IsFinish)
                {
                    if (it.IsCrear)
                    {
                        //スコア加算
                        ScoreManager.Instance.AddScore((int)it.GetScore, FrontPerson.Score.ReasonForAddition.Bounty);

                        _missionCnt++;

                        //ミッションクリア数が３つになったら
                        if (_missionCnt >= 3)
                        {
                            //武器を出す
                            _player.WeaponUpgrade(Random.Range(0, SpecialWeaponManager._instance._weaponNum));
                            _missionCnt = 0;
                        }
                    }
                    MissionList[i].ImDie();
                    MissionList[i] = Instantiate(MissionPrefabList[Random.Range(0, _missionNum)], transform).GetComponent<Bounty.Bounty>();
                }
                i++;
            }
        }
    }
}


