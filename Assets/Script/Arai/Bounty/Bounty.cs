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
        protected string MissionName;

        

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
        public Text GetText { get { return _text; } }
        

        /// <summary>
        /// 進捗状況文字列取得
        /// </summary>
        public string GetProgressString { get { return _progressString; } }
        protected string _progressString;


        /// <summary>
        /// バウンティマネージャー参照
        /// </summary>
        protected BountyManager _Bmanager = null;

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
        protected Text _text = null;

        // Start is called before the first frame update
        public void Start()
        {
            _Bmanager = transform.parent.GetComponent<BountyManager>();
            _nowTime = LimitTime;
            _text = GetComponent<Text>();
            _text.text = MissionName;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ImDie()
        {
            Destroy(gameObject);
        }
    }
}

