using FrontPerson.Constants;
using FrontPerson.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace FrontPerson.UI
{
    public class Tutorial : MonoBehaviour
    {
        [Header("表示するチュートリアル画像の親オブジェクト")]
        [SerializeField]
        GameObject images_ = null;

        /// <summary>
        /// チュートリアル画像のリスト
        /// </summary>
        List<GameObject> imagelList_ = new List<GameObject>();

        [Header("ページ数")]
        [SerializeField]
        [Tooltip("ページ数アイコンの親オブジェクト")]
        GameObject pageIcons_ = null;

        [SerializeField]
        [Tooltip("ページ数アイコンのプレハブ")]
        GameObject pageIconPrefab_ = null;

        [SerializeField]
        [Tooltip("ページ数表示Textコンポーネント")]
        Text pageNumText_ = null;

        [SerializeField]
        [Tooltip("選択されているアイコンの色")]
        Color selectIconColor_;

        /// <summary>
        /// アイコンのリスト
        /// </summary>
        List<Button> iconList_ = new List<Button>();

        [Header("矢印アイコン")]
        [SerializeField]
        [Tooltip("左矢印")]
        GameObject left_ = null;

        [SerializeField]
        [Tooltip("右矢印")]
        GameObject right_ = null;


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

        /// <summary>
        /// パネルを変える前のチュートリアルのインデックス
        /// </summary>
        int oldTutorialIndex_ = 0;

        /// <summary>
        /// 表示するパネルの枚数
        /// </summary>
        int panelNum_ = 4;



        // Start is called before the first frame update
        void Start()
        {
            isInputWait_ = true;
            IsFinish = false;
            animator_ = GetComponent<Animator>();

            // 表示するチュートリアルのパネルの枚数を取得
            panelNum_ = images_.transform.childCount;

            // パネルのリスト作成
            for (int i = 0; i < panelNum_; i++)
            {
                var imageObject = images_.transform.GetChild(i).gameObject;
                imagelList_.Add(imageObject);
                imageObject.SetActive(false);
            }

            // 1枚目を有効か
            imagelList_[0].SetActive(true);

            // パネルの数だけページ数のアイコンを生成
            for(int i = 0; i < panelNum_; i++)
            {
                var gameObject = Instantiate(pageIconPrefab_, pageIcons_.transform);
                var icon = gameObject.GetComponent<Tutorial_Icon>();
                if (icon != null)
                {
                    icon.Init(this, i);
                }
                else
                {
                    Debug.LogError("チュートリアルのアイコンボタンにTutorial_Iconコンポーネントがアタッチされていません\nオブジェクト名は、" + gameObject.name + "です。");
                }
                var button = gameObject.GetComponent<Button>();
                if (button != null)
                {
                    iconList_.Add(button);
                }
                else
                {
                    Debug.LogError("チュートリアルのアイコンボタンにButtonコンポーネントがアタッチされていません\nオブジェクト名は、" + gameObject.name + "です。");
                }
            }

            // リストの先頭の色を変える
            var colors = iconList_[0].colors;
            colors.normalColor = selectIconColor_;
            iconList_[0].colors = colors;

            // 最初は左の矢印は無効
            left_.SetActive(false);

            // ページ数の文字更新
            PageNumTextUpdate();
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
        public void PushLeft()
        {
            // 今一番左にいるなら動かさない
            if (tutorialIndex_ <= 0) return;

            animator_.SetBool("PanelChange", true);
            isInputWait_ = false;
            oldTutorialIndex_ = tutorialIndex_;
            tutorialIndex_--;

            // 移動して一番左に行ったら左の矢印を消す
            if (tutorialIndex_ <= 0)
            {
                left_.SetActive(false);
            }
            // 最後のパネルなら右の矢印の文字を変える
            if (!right_.transform.GetChild(0).gameObject.activeSelf)
            {
                right_.transform.GetChild(0).gameObject.SetActive(true);
                right_.transform.GetChild(1).gameObject.SetActive(false);
            }

            // SE再生
            AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_CURSOR);
        }


        /// <summary>
        /// 右入力時
        /// </summary>
        public void PushRight()
        {
            // 今一番右にいるなら終了
            if (tutorialIndex_ >= panelNum_-1)
            {
                animator_.SetBool("Finish", true);
                
            }
            else
            {
                animator_.SetBool("PanelChange", true);
                oldTutorialIndex_ = tutorialIndex_;
                tutorialIndex_++;
                // 左の矢印が出てないなら出す
                if (!left_.activeSelf)
                {
                    left_.SetActive(true);
                }
                // 最後のパネルなら右の矢印の文字を変える
                if(tutorialIndex_ >= panelNum_ - 1)
                {
                    right_.transform.GetChild(0).gameObject.SetActive(false);
                    right_.transform.GetChild(1).gameObject.SetActive(true);
                }
            }

            isInputWait_ = false;
            // SE再生
            AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_CURSOR);
        }


        /// <summary>
        /// エスケイプ押したとき
        /// </summary>
        void PushESC()
        {
            animator_.SetBool("Finish", true);
            isInputWait_ = false;

            AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_CURSOR);
        }

        public void IconClick(int clickIconNumber)
        {
            if (!isInputWait_) return;

            animator_.SetBool("PanelChange", true);
            isInputWait_ = false;
            oldTutorialIndex_ = tutorialIndex_;
            tutorialIndex_ = clickIconNumber;


            // 左の矢印が出てないなら出す
            if (!left_.activeSelf)
            {
                left_.SetActive(true);
            }
            // 最後のパネルなら右の矢印の文字を変える
            if (tutorialIndex_ >= panelNum_ - 1)
            {
                right_.transform.GetChild(0).gameObject.SetActive(false);
                right_.transform.GetChild(1).gameObject.SetActive(true);
            }


            // 移動して一番左に行ったら左の矢印を消す
            if (tutorialIndex_ <= 0)
            {
                left_.SetActive(false);
            }
            // 最後のパネルなら右の矢印の文字を変える
            if (tutorialIndex_ < panelNum_ - 1)
            {
                right_.transform.GetChild(0).gameObject.SetActive(true);
                right_.transform.GetChild(1).gameObject.SetActive(false);
            }


            // SE再生
            AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_CURSOR);
        }

        private void PageNumTextUpdate()
        {
            pageNumText_.text = (tutorialIndex_ + 1).ToString() + " / " + panelNum_.ToString();
        }


        public void ChangeImage()
        {
            imagelList_[oldTutorialIndex_].SetActive(false);
            imagelList_[tutorialIndex_].SetActive(true);
            { 
                var colors = iconList_[oldTutorialIndex_].colors;
                colors.normalColor = Color.white;
                iconList_[oldTutorialIndex_].colors = colors;
            }
            {
                var colors = iconList_[tutorialIndex_].colors;
                colors.normalColor = selectIconColor_;
                iconList_[tutorialIndex_].colors = colors;
            }

            PageNumTextUpdate();
        }


        /// <summary>
        /// アニメーション終了時
        /// </summary>
        public void StopAnim()
        {
            isInputWait_ = true;
            animator_.SetBool("PanelChange", false);
            animator_.SetBool("Finish", false);
        }


        /// <summary>
        /// チュートリアル終了
        /// </summary>
        public void Finish()
        {
            StopAnim();
            IsFinish = true;
            gameObject.SetActive(false);
        }
    }
}
