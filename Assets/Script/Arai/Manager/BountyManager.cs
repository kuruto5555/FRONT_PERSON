using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.common;
using FrontPerson.Bounty;
using System.Security.Cryptography;
using FrontPerson.UI;

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
        

        [Header("ミッションのプレハブリスト")]
        [Tooltip("簡単なミッションのPrefabリスト")]
        [SerializeField] 
        List<GameObject> easyMissionPrefabList = null;

        [Tooltip("普通のミッションのPrefabリスト")]
        [SerializeField]
        List<GameObject> normalMissionPrefabList_ = null;

        [Tooltip("難しいミッションのPrefabリスト")]
        [SerializeField]
        List<GameObject> hardMissionPrefabList_ = null;

        [Header("ミッションの難易度が変わるコンボ数")]
        [SerializeField, Range(10, 50)]
        [Tooltip("この値を超えるとミッションがnormalMissionPrefabList_から抽選されるようになる")]
        int normalLine_ = 30;

        [SerializeField, Range(10, 50)]
        [Tooltip("この値を超えるとミッションがhardMissionPrefabList_から抽選されるようになる")]
        int hardLine_ = 30;



        [Header("スペシャル武器UI")]
        [SerializeField]
        [Tooltip("UI_SP_Weaponコンポ―ネント")]
        UI_SP_Weapon spWeaponUI = null;

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
            _missionNum = easyMissionPrefabList.Count;
            for (int i = 0; i < ACTIV_MISSION; i++)
            {
                MissionList.Add(Instantiate(easyMissionPrefabList[Random.Range(0, _missionNum)].GetComponent<Bounty.Bounty>(), transform)) ;
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
                    spWeaponUI.clearMission();

                    //ミッションクリア数が３つになったら
                    if (_missionCnt % GET_WEAPON_MISSON_NUM == 0)
                    {
                        //武器を出す
                        var num = Random.Range(0, SpecialWeaponManager._instance._weaponNum);
                        _player.WeaponUpgrade(num);
                        spWeaponUI?.GetSPWeapon(num);
                    }
                }
            }
        }

        /// <summary>
        /// 新しいMissionの生成&クリアしたミッションの削除
        /// </summary>
        /// <param name="index">クリアしたミッションのインデックス</param>
        public void ClearMissionDelete(int index)
        {
            var mission = MissionList[index];
            // クリアしたミッションのところに新しいミッションを作成
            MissionList[index] = Instantiate(easyMissionPrefabList[Random.Range(0, _missionNum)], transform).GetComponent<Bounty.Bounty>();
            // クリアしたほうは消す
            mission.ImDie();
        }
    }
}


