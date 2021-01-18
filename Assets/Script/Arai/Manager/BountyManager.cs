using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.common;
using FrontPerson.Bounty;
using System.Security.Cryptography;
using FrontPerson.UI;
using UnityEngine.Rendering;

namespace FrontPerson.Manager
{
    public class BountyManager : MonoBehaviour
    {
        /// <summary>
        /// ミッションの難易度
        /// </summary>
        public enum Difficulty
        {
            EASY = 0,
            NORMAL,
            HARD,

            Count
        }

        /// <summary>
        /// ミッションの情報クラス
        /// </summary>
        public class MissionInfo
        {
            public Bounty.Bounty mission = null;
            public Difficulty difficulty = Difficulty.EASY;
            public bool IsFinish { get { return mission.IsFinish; } }
            public bool IsClear { get { return mission.IsClear; } }
            
        }

        /// <summary>
        /// インスタンス
        /// </summary>
        public static BountyManager _instance { get; private set;}

        [Header("武器がもらえるミッション数")]
        [SerializeField,Range(1, 10)]
        int GET_WEAPON_MISSON_NUM = 3;

        /// <summary>
        /// 現在発令しているミッションのリスト
        /// </summary>
        public List<MissionInfo> MissionInfoList { get; private set; }
        

        [Header("ミッションのプレハブリスト")]
        [Tooltip("簡単なミッションのPrefabリスト")]
        [SerializeField] 
        List<GameObject> easyMissionPrefabList_ = null;

        /// <summary>
        /// 抽選済みの簡単なミッションのPrefabリスト
        /// </summary>
        List<GameObject> lotteryCompletedEasyMission_ = null;

        [Tooltip("普通のミッションのPrefabリスト")]
        [SerializeField]
        List<GameObject> normalMissionPrefabList_ = null;

        /// <summary>
        /// 抽選済みの普通のミッションのPrefabリスト
        /// </summary>
        List<GameObject> lotteryCompletedNormalMission_ = null;

        [Tooltip("難しいミッションのPrefabリスト")]
        [SerializeField]
        List<GameObject> hardMissionPrefabList_ = null;

        /// <summary>
        /// 抽選済みの難しいミッションのPrefabリスト
        /// </summary>
        List<GameObject> lotteryCompletedHardMission_ = null;

        [Header("ミッションの難易度が変わるコンボ数")]
        [SerializeField, Range(10, 50)]
        [Tooltip("この値を超えるとミッションがnormalMissionPrefabList_から抽選されるようになる")]
        int normalLine_ = 25;

        [SerializeField, Range(10, 50)]
        [Tooltip("この値を超えるとミッションがhardMissionPrefabList_から抽選されるようになる")]
        int hardLine_ = 40;



        [Header("スペシャル武器UI")]
        [SerializeField]
        [Tooltip("UI_SP_Weaponコンポ―ネント")]
        UI_SP_Weapon spWeaponUI = null;


        /// <summary>
        /// 同時に発動するミッションの数
        /// </summary>
        private const int ACTIV_MISSION = 3;

        /// <summary>
        /// ミッションクリアの総数
        /// </summary>
        public int _clearMissionCnt { get; private set; } = 0;

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


        //private List <GameObject> 

        private void Awake()
        {
            // インスタンスの取得
            if (_instance == null) _instance = this;
            else
            { 
                Destroy(this); 
                Debug.LogError("BountyManagerは既に他のオブジェクトにアタッチされているため、コンポーネントを破棄しました。アタッチされているGameObjectは" + _instance.gameObject.name + "です。");
                return;
            }

            // リストのメモリ確保
            MissionInfoList = new List<MissionInfo>();
            lotteryCompletedEasyMission_ = new List<GameObject>();
            lotteryCompletedHardMission_ = new List<GameObject>();
            lotteryCompletedNormalMission_ = new List<GameObject>();


            // 自分のメンバの初期化
            _isPlayerDamage = false;
            _clearMissionCnt = 0;
            _nowCombo = 0;
            _fireCount = 0;
            _missionNum = easyMissionPrefabList_.Count;

            // 最初のミッションを３個選出
            for (int i = 0; i < ACTIV_MISSION; i++)
            {
                MissionInfoList.Add(new MissionInfo());
                SetNewMission(i);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // 他のインスタンスの取得
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character.Player>();
            if (spWeaponUI == null)
            {
                spWeaponUI = FindObjectOfType<UI_SP_Weapon>();
            }
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


        /// <summary>
        /// 全てのミッションを強制的にクリアさせる
        /// デバッグ用の関数
        /// </summary>
        private void AllMissionChange()
        {
#if UNITY_EDITOR
            //デバッグ用ミッション全て変更
            if (Input.GetKeyDown(KeyCode.K))
            {
                for (int i = 0; i < ACTIV_MISSION; i++)
                {
                    if (MissionInfoList[i].mission == null) continue;
                    MissionInfoList[i].mission.MissionClear();
                }
            }
#endif
        }

        /// <summary>
        /// ミッションを監視してる
        /// </summary>
        private void MissionMonitoring()
        {
            foreach(var b in MissionInfoList)
            {
                // ミッションの経過時間を進めて制限時間すぎてたら失敗にする
                if (b.mission == null) continue;
                b.mission.CheckUpdate();
            }

            for(int i = 0 ; i<MissionInfoList.Count; i++ )
            {
                var mission = MissionInfoList[i].mission;
                if (mission == null) continue;
                
                // 終了してないミッションは無視
                if (!mission.IsFinish) continue;
                
                // クリアしてたらスコア加算、カウントを加算
                if (mission.IsClear)
                {
                    // スコア加算
                    ScoreManager.Instance.AddScore((int)mission.GetScore, Score.ADD_SCORE_TYPE.BOUNTY_SCORE);

                    // ミッションクリア数を加算
                    _clearMissionCnt++;
                    // UIスクリプトにミッションをクリアしたことを知らせる
                    spWeaponUI.clearMission();

                    //ミッションクリア数が３つになったら
                    if (_clearMissionCnt % GET_WEAPON_MISSON_NUM == 0)
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
            var mission = MissionInfoList[index].mission;
            // クリアしたほうは消す
            mission.ImDie();
            // クリアしたミッションのところに新しいミッションを作成
            MissionInfoList[index].mission = null;
            SetNewMission(index);
        }


        void SetNewMission(int index)
        {
            var nowComboNum = ComboManager.Instance.ComboNum;

            // コンボ数がノーマル基準未満だったらイージーから抽選
            if (nowComboNum < normalLine_)
            {
                // ミッションの生成
                MissionInfoList[index].mission = LotteryMission(easyMissionPrefabList_, lotteryCompletedEasyMission_).GetComponent<Bounty.Bounty>();
                MissionInfoList[index].difficulty = Difficulty.EASY;
            }
            // コンボ数がノーマル基準以上でハード基準未満だったらノーマルから抽選
            else if (nowComboNum < hardLine_)
            {
                // ミッションの生成
                MissionInfoList[index].mission = LotteryMission(normalMissionPrefabList_, lotteryCompletedNormalMission_).GetComponent<Bounty.Bounty>();
                MissionInfoList[index].difficulty = Difficulty.NORMAL;

            }
            // コンボ数がハード基準以上だったらハードから抽選
            else
            {
                // ミッションの生成
                MissionInfoList[index].mission = LotteryMission(hardMissionPrefabList_, lotteryCompletedHardMission_).GetComponent<Bounty.Bounty>();
                MissionInfoList[index].difficulty = Difficulty.HARD;
            }
        }

        

        /// <summary>
        /// ミッションの抽選
        /// </summary>
        /// <param name="missionList">抽選するミッションのリスト</param>
        /// <param name="lotteryCompletedMissionList">抽選されたミッションをしまうリスト</param>
        /// <returns>生成されたミッション</returns>
        GameObject LotteryMission(List<GameObject> missionList, List<GameObject> lotteryCompletedMissionList)
        {
            bool oneMore = true;
            int lotteryIndex = 0;

            while (oneMore) {
                // ミッションリストのカウントが0だったら抽選リストから戻す
                if (missionList.Count == 0)
                {
                    ReleaseLotteryCompletedMission(missionList, lotteryCompletedMissionList);
                }

                // 抽選するミッションのインデックスを乱数で出す
                lotteryIndex = Random.Range(0, missionList.Count-1);
                // そのミッションのクラスタイプを取得
                var missionType = missionList[lotteryIndex].GetComponent<Bounty.Bounty>().GetType();

                oneMore = false;
                // 同じタイプのミッションが既に抽選されているかどうか
                foreach (var missionInfo in MissionInfoList)
                {
                    if (missionInfo.mission == null) continue;

                    if (missionInfo.mission.GetType() == missionType)
                    {
                        // 同じタイプのミッションがあったので、
                        // このミッションを抽選済みにしまって、
                        lotteryCompletedMissionList.Add(missionList[lotteryIndex]);
                        // ミッションのリストから消し、
                        missionList.RemoveAt(lotteryIndex);
                        // もう一回のフラグを立てる
                        oneMore = true;
                        break;
                    }
                }
            }


            // まだ抽選されてないタイプのミッションだったので生成
            var newMission = Instantiate(missionList[lotteryIndex], transform);
            // 生成したらミッションリストから抽選済みミッションリストに移す
            lotteryCompletedMissionList.Add(missionList[lotteryIndex]);
            missionList.RemoveAt(lotteryIndex);

            // 生成したミッションを返す
            return newMission;
        }

        /// <summary>
        /// 抽選済みミッションを元に戻す
        /// </summary>
        /// <param name="missionList">ミッションを戻す先のリスト</param>
        /// <param name="lotteryCompletedMissionList">抽選済みのミッションのリスト</param>
        void ReleaseLotteryCompletedMission(List<GameObject> missionList, List<GameObject> lotteryCompletedMissionList)
        {
            foreach (var lotteryCompletedMission in lotteryCompletedMissionList)
            {
                missionList.Add(lotteryCompletedMission);
            }
            lotteryCompletedMissionList.Clear();
        }
    }
}


