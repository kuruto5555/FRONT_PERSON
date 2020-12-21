using FrontPerson.Constants;
using FrontPerson.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FrontPerson.UI
{
    public class Tutorial : MonoBehaviour
    {
        /// <summary>
        /// チュートリアル終了フラグ
        /// ゲームシーンコントローラーに教えるため
        /// </summary>
        public bool IsFinish { get; private set; } = false;

        /// <summary>
        /// 連打対策
        /// trueなら入力可
        /// </summary>
        bool isInputWait_ = true;

        /// <summary>
        /// チュートリアルのアニメーター
        /// </summary>
        Animator animator_ = null;

        /// <summary>
        /// 表示してるチュートリアルのインデックス
        /// </summary>
        int tutorialIndex_ = 0;


        // Start is called before the first frame update
        void Start()
        {
            isInputWait_ = true;
            IsFinish = false;
            animator_ = GetComponent<Animator>();
        }

        void OnEnable()
        {
            isInputWait_ = true;
            IsFinish = false;
            animator_ = GetComponent<Animator>();
        }


        // Update is called once per frame
        void Update()
        {
            // 入力可能じゃないなら帰る
            if (!isInputWait_) return;


            // エスケイプ押されたら、チュートリアル表示をすぐ終わらせる
            if (Input.GetButton(InputName.PAUSE))
            {
                PushESC();
            }

            // 一個前のチュートリアルを出す
            else if (Input.GetAxisRaw(InputName.HORIZONTAL) <= -0.5f)
            {
                PushLeft();
            }

            // 次のチュートリアルを出す
            // 最後のページだったら終了
            else if (Input.GetAxisRaw(InputName.HORIZONTAL) >= 0.5f)
            {
                PushRight();
            }
        }


        /// <summary>
        /// 左入力時
        /// </summary>
        void PushLeft()
        {
            // 今一番左にいるなら動かさない
            if (tutorialIndex_ <= 0) return;

            animator_.SetBool("IsInputLeft", true);
            isInputWait_ = false;
            tutorialIndex_--;
            // SE再生
            AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_CURSOR);
        }


        /// <summary>
        /// 右入力時
        /// </summary>
        void PushRight()
        {
            // 今一番左にいるなら動かさない
            if (tutorialIndex_ >= 3) return;

            animator_.SetBool("IsInputRight", true);
            isInputWait_ = false;
            tutorialIndex_++;
            // SE再生
            AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_CURSOR);
        }


        /// <summary>
        /// エスケイプ押したとき
        /// </summary>
        void PushESC()
        {
            animator_.SetBool("IsInputStartButton", true);
            isInputWait_ = false;

            AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_CURSOR);
        }


        /// <summary>
        /// アニメーション終了時
        /// </summary>
        public void StopAnim()
        {
            isInputWait_ = true;
            animator_.SetBool("IsInputLeft", false);
            animator_.SetBool("IsInputRight", false);
            animator_.SetBool("IsInputStartButton", false);
        }


        /// <summary>
        /// チュートリアル終了
        /// </summary>
        public void Finish()
        {
            isInputWait_ = false;
            IsFinish = true;
            gameObject.SetActive(false);
        }
    }
}
