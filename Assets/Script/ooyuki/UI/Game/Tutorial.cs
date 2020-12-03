using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;


namespace FrontPerson.UI
{
    public class Tutorial : MonoBehaviour
    {
        [Header("チュートリアルの画像リスト")]
        [SerializeField]
        List<GameObject> tutotials_ = null;

        [Header("矢印")]
        [SerializeField]
        GameObject left_ = null;
        [SerializeField]
        GameObject right_ = null;

        [Header("入力待ち時間")]
        [SerializeField, Range(0.1f, 2.0f)]
        float inputWaitTime = 0.0f;

        /// <summary>
        /// チュートリアル終了フラグ
        /// ゲームシーンコントローラーに教えるため
        /// </summary>
        public bool IsFinish { get; private set; } = false;

        /// <summary>
        /// 連打対策
        /// trueなら入力可
        /// </summary>
        bool isInput = true;


        /// <summary>
        /// 表示してるチュートリアルのインデックス
        /// </summary>
        int tutorialIndex_ = 0;


        // Start is called before the first frame update
        void Start()
        {
            isInput = true;
            IsFinish = false;
            left_.SetActive(false);
        }

        void OnEnable()
        {
            isInput = true;
            IsFinish = false;
            left_.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            // 入力可能じゃないなら帰る
            //        if (!isInput) return;


            // エスケイプ押されたら、チュートリアル表示をすぐ終わらせる
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PushESC();
            }

            // 一個前のチュートリアルを出す
            else if (Input.GetKeyDown(KeyCode.A))
            {
                PushLeft();
            }

            // 次のチュートリアルを出す
            // 最後のページだったら終了
            else if (Input.GetKeyDown(KeyCode.D))
            {
                PushRight();
            }
        }

        /// <summary>
        /// チュートリアル終了
        /// </summary>
        void Finish()
        {
            IsFinish = true;
        }


        /// <summary>
        /// 左入力時
        /// </summary>
        void PushLeft()
        {
            // 今一番左にいるなら動かさない
            if (tutorialIndex_ <= 0)
            {
                return;
            }

            tutotials_[tutorialIndex_].SetActive(false);
            tutorialIndex_--;
            tutotials_[tutorialIndex_].SetActive(true);
            isInput = false;

            // 動いたということは一番右にいないはずなので
            // 右の矢印が出てないなら出し、終了アイコンを非表示にする
            if (!right_.activeSelf)
            {
                right_.SetActive(true);
                //***.SetActive(false);
            }

            // 動いた結果一番左に行ったら左の矢印を消す
            if (tutorialIndex_ <= 0)
            {
                left_.SetActive(false);
            }
        }


        /// <summary>
        /// 右入力時
        /// </summary>
        void PushRight()
        {
            // 今一番右にいるならチュートリアル終了
            if (tutorialIndex_ >= (tutotials_.Count-1))
            {
                Finish();
                return;
            }


            // 動いたということは一番左にいないはずなので
            // 左の矢印が出てないなら出す
            if (!left_.activeSelf)
            {
                left_.SetActive(true);
            }


            tutotials_[tutorialIndex_].SetActive(false);
            tutorialIndex_++;
            tutotials_[tutorialIndex_].SetActive(true);
            isInput = false;

            // 動いた結果一番右にいるならアイコンを変える
            if (tutorialIndex_ >= (tutotials_.Count-1))
            {
                right_.SetActive(false);
                //***.SetActive(true);
            }
        }


        /// <summary>
        /// エスケイプ押したとき
        /// </summary>
        void PushESC()
        {
            Finish();
        }


        /// <summary>
        /// アニメーション終了時
        /// </summary>
        void StopAnim()
        {
            isInput = true;
        }
    }
}
