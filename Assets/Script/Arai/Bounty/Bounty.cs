using FrontPerson.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrontPerson.Bounty
{
    public abstract class Bounty : MonoBehaviour
    {
        /// <summary>
        /// 制限時間
        /// </summary>
        [Header("制限時間")]
        [SerializeField, Range(1.0f, 60.0f)]
        protected float LimitTime = 1.0f;

        [Header("スコア")]
        [SerializeField]
        protected uint Score = 0;

        [Header("表示したい文言")]
        [SerializeField]
        protected string MissionNames;

        protected string _missionName;

        

        /// <summary>
        /// スコア取得
        /// </summary>
        public uint GetScore { get { return Score; } }

        /// <summary>
        /// ミッション経過時間取得
        /// </summary>
        public float GetNowTime { get { return _nowTime; } }
       

        /// <summary>
        /// 制限時間取得
        /// </summary>
        public float GetLimitTime { get { return LimitTime; } }

        

        /// <summary>
        /// クリアしてるかどうか取得
        /// </summary>
        public bool IsCrear { get { return _isClear; } }

        

        /// <summary>
        /// 終わってるかどうか取得
        /// </summary>
        public bool IsFinish { get { return _isFinish; } }

        /// <summary>
        /// ミッションの文言取得
        /// </summary>
        //public Text GetText { get { return _text; } }
        
        public string GetMissionName { get{ return _missionName; } }

        /// <summary>
        /// 進捗状況文字列取得
        /// </summary>
        public string GetProgressString { get { return _progressString; } }

        /// <summary>
        /// 進捗状況文字列
        /// </summary>
        protected string _progressString;


        /// <summary>
        /// バウンティマネージャー参照
        /// </summary>
        protected Manager.BountyManager _Bmanager = null;

        /// <summary>
        /// 今の経過時間
        /// </summary>
        protected float _nowTime = 0.0f;

        /// <summary>
        /// クリアしてるか
        /// </summary>
        protected bool _isClear = false;

        /// <summary>
        /// 終わってるかどうか
        /// </summary>
        protected bool _isFinish = false;

        /// <summary>
        /// ミッションの文言
        /// </summary>
        //protected Text _text = null;

        // Start is called before the first frame update
        protected void Start()
        {
            _Bmanager = transform.parent.GetComponent<Manager.BountyManager>();
            _nowTime = LimitTime;
            //_text = GetComponent<Text>();
            //_text.text = MissionName;
            _isClear = _isFinish = false;
        }

        // Update is called once per frame
        protected void Update()
        {
            _nowTime -= Time.deltaTime;

            if (_nowTime < 0)
            {
                _isFinish = true;
            }
        }

        public void ImDie()
        {
            Destroy(gameObject);
        }

        protected void MissionClear()
        {
            _isClear = _isFinish = true;
        }

        protected void MissionFailed()
        {
            _isClear = false;
            _isFinish = true;
        }
    }
}

