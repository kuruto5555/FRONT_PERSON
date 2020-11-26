using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [Header("チュートリアルの画像リスト")]
    [SerializeField]
    List<Image> tutotials_ = null;

    [Header("矢印")]
    [SerializeField]
    Image left_ = null;
    [SerializeField]
    Image right_ = null;

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
        IsFinish = false;
    }

    void OnEnable()
    {
        IsFinish = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 入力可能じゃないなら帰る
        if(!isInput) return;




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


    void PushLeft()
    {
        if (tutorialIndex_ <= 0) return;
        tutorialIndex_--;

    }

    void PushRight()
    {

    }

    void PushESC()
    {
        Finish();
    }


    void Finish()
    {
        IsFinish = true;
        gameObject.SetActive(false);
    }


}
