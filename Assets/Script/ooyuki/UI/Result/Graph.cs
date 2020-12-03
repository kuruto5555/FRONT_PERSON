using UnityEngine;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    public class Graph : MonoBehaviour
    {
        [Header("各ゲージ")]
        [SerializeField]
        RectTransform youGauge_ = null;
        [SerializeField]
        RectTransform averageGauge_ = null;
        [SerializeField]
        RectTransform numberOneGauge_ = null;

        [Header("各データ表示テキスト")]
        [SerializeField]
        Text youDataText_ = null;
        [SerializeField]
        Text averageDataText_ = null;
        [SerializeField]
        Text numberOneDataText_ = null;

        [Header("何秒でアニメーションが終わるか")]
        [SerializeField, Range(0.1f, 5.0f)]
        float animSpeed_ = 2.0f;

        /// <summary>
        /// 今回のデータのゲージ
        /// </summary>
        Gauge youDataGauge_ = new Gauge();

        /// <summary>
        /// 平均値のデータ
        /// </summary>
        Gauge averageDataGauge_ = new Gauge();

        /// <summary>
        /// 一位のデータ
        /// </summary>
        Gauge numberOneDataGauge_ = new Gauge();


        /// <summary>
        /// 何かしらアニメーションしているかどうか
        /// </summary>
        bool isAnimation { get { return youDataGauge_.IsAnimation || averageDataGauge_.IsAnimation || numberOneDataGauge_.IsAnimation;} }


        /// <summary>
        /// アニメーションが終わったかどうか
        /// </summary>
        public bool IsFinish { get; private set; } = false;



        // Start is called before the first frame update
        void Start()
        {
            youGauge_.anchorMax = new Vector2(0.0f, youGauge_.anchorMax.x);
            averageGauge_.anchorMax = new Vector2(0.0f, averageGauge_.anchorMax.x);
            numberOneGauge_.anchorMax = new Vector2(0.0f, numberOneGauge_.anchorMax.x);

            youDataText_.text = "0";
            averageDataText_.text = "0";
            numberOneDataText_.text = "0";
            youDataText_.gameObject.SetActive(false);
            averageDataText_.gameObject.SetActive(false);
            numberOneDataText_.gameObject.SetActive(false);

            IsFinish = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isAnimation) return;

            // 今回のあなたのデータのアニメ―しょん
            if (youDataGauge_.IsAnimation) youDataGauge_.AnimationUpdate();
            // 平均値のアニメーション
            if (averageDataGauge_.IsAnimation) averageDataGauge_.AnimationUpdate();
            // 一位の人のアニメーション
            if (numberOneDataGauge_.IsAnimation) numberOneDataGauge_.AnimationUpdate();

            if (isAnimation) IsFinish = true;
        }


        /// <summary>
        /// アニメーション開始
        /// </summary>
        /// <param name="youData">今回のあなたのデータ</param>
        /// <param name="averageData">平均値</param>
        /// <param name="numberOneData">一位のデータ</param>
        public void StartAnimation(int youData, int averageData, int numberOneData)
        {
            if (isAnimation) return;

            int max = youData > numberOneData ? youData : numberOneData;
            youDataGauge_.StartAnimation(youGauge_, youDataText_, youData, max, animSpeed_);
            averageDataGauge_.StartAnimation(averageGauge_, averageDataText_, averageData, max, animSpeed_);
            numberOneDataGauge_.StartAnimation(numberOneGauge_, numberOneDataText_, numberOneData, max, animSpeed_);

            IsFinish = false;
        }

        /// <summary>
        /// アニメーションを飛ばす
        /// </summary>
        public void StopAnimation()
        {
            if (!isAnimation) return;

            youDataGauge_.StopAnimation();
            averageDataGauge_.StopAnimation();
            numberOneDataGauge_.StopAnimation();

            IsFinish = true;
        }
    }
}
