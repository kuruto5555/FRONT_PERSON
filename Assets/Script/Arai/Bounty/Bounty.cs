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
        /// バウンティマネージャー参照
        /// </summary>
        protected BountyManager _Bmanager = null;

        /// <summary>
        /// スコア取得
        /// </summary>
        protected uint GetScore { get { return Score; } }

        /// <summary>
        /// ミッション経過時間取得
        /// </summary>
        public float GetNowTime { get { return _nowTime; } }

        /// <summary>
        /// 今の経過時間
        /// </summary>
        protected float _nowTime = 0.0f;

        /// <summary>
        /// クリアしてるか
        /// </summary>
        protected bool _isClear = false;

        /// <summary>
        /// クリアしてるかどうか取得
        /// </summary>
        public bool IsCrear { get { return _isClear; } }

        /// <summary>
        /// 終わってるかどうか
        /// </summary>
        protected bool _isFinish = false;

        /// <summary>
        /// 終わってるかどうか取得
        /// </summary>
        public bool IsFinish { get { return _isFinish; } }

        /// <summary>
        /// テキストコンポーネント取得
        /// </summary>
        public Text GetText { get { return _text; } }
        protected Text _text;

        // Start is called before the first frame update
        public void Start()
        {
            _Bmanager = transform.parent.GetComponent<BountyManager>();
            _nowTime = LimitTime;
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

