using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.common;
using FrontPerson.Bounty;
using System.Security.Cryptography;

namespace FrontPerson.Manager
{
    public class BountyManager : MonoBehaviour
    {
        [Header("武器がもらえるミッション数")]
        [SerializeField,Range(1, 10)]
        int GET_WEAPON_MISSON_NUM = 3;

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
        public int _missionCnt { get; private set; } = 0;

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

        }

        private void LateUpdate()
        {
            MissionMonitoring();
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
#if UNITY_EDITOR
            //デバッグ用ミッション全て変更
            if (Input.GetKeyDown(KeyCode.K))
            {

                for (int debug = 0; debug < ACTIV_MISSION; debug++)
                {
                    MissionList[debug].MissionClear();
                }
            }
#endif
        }

        /// <summary>
        /// ミッションを監視してる
        /// </summary>
        private void MissionMonitoring()
        {
            foreach(var b in MissionList)
            {
                b.CheckUpdate();
            }

            for(int i = 0 ; i<MissionList.Count; i++ )
            {
                var mission = MissionList[i];
                if (!mission.IsFinish) continue;

                if (mission.IsCrear)
                {
                    //スコア加算
                    ScoreManager.Instance.AddScore((int)mission.GetScore, Score.ADD_SCORE_TYPE.BOUNTY_SCORE);

                    _missionCnt++;

                    //ミッションクリア数が３つになったら
                    if (_missionCnt % GET_WEAPON_MISSON_NUM > 0)
                    {
                        //武器を出す
                        _player.WeaponUpgrade(Random.Range(0, SpecialWeaponManager._instance._weaponNum));
                    }
                }
            }
        }


        public void ClearMissionDelete(int index)
        {
            var mission = MissionList[index];
            // クリアしたミッションのところに新しいミッションを作成
            MissionList[index] = Instantiate(MissionPrefabList[Random.Range(0, _missionNum)], transform).GetComponent<Bounty.Bounty>();
            // クリアしたほうは消す
            mission.ImDie();
        }
    }
}


