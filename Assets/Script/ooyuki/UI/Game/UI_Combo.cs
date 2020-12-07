using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FrontPerson.Manager;

public class UI_Combo : MonoBehaviour
{
    [Header("コンボ数表示用のテキスト")]
    [SerializeField]
    Text comboNumText_ = null;

    [Header("コンボアイコン")]
    [SerializeField]
    Image comboIcon_ = null;

    [Header("コンボ持続時間ゲージ")]
    [SerializeField]
    Image comboTimeLimitGauge_ = null;


    [Header("コンボアイコンリスト(上から順番に変わる)")]
    [SerializeField]
    List<Sprite> iconImageList_ = null;

    [Header("アイコンが変わるタイミングの値")]
    [SerializeField]
    List<int> iconChengeValue_ = null;


    /// <summary>
    /// スコアマネージャー
    /// </summary>
    ScoreManager scoreManager_ = null;

    /// <summary>
    /// 現在のコンボ数
    /// </summary>
    int comboNum_ = 0;

    /// <summary>
    /// コンボ数の前フレーム
    /// </summary>
    int comboNumPrev_ = 0;

    /// <summary>
    /// コンボのアイコンの種類
    /// </summary>
    int comboIconType_ = 0;


    // Start is called before the first frame update
    void Start()
    {
        comboNumPrev_ = comboNum_ = 0;
        comboIconType_ = 0;
        comboIcon_.sprite = iconImageList_[comboIconType_];
        scoreManager_ = ScoreManager.Instance;

        SetComboNum();
    }

    // Update is called once per frame
    void Update()
    {
        SetComboNum();
    }

    void SetComboNum()
    {
        comboNumPrev_ = comboNum_;
        comboNum_ = scoreManager_.ComboBonus;
        comboNumText_.text = comboNum_.ToString();


        // コンボ数が前フレームのほうが大きかったらそれはきっとコンボが途切れてる
        if(comboNumPrev_ > comboNum_)
        {
            ComboUI_Reset();
        }


        if(comboNum_ > iconChengeValue_[comboIconType_] && comboIconType_ >= iconImageList_.Count)
        {
            comboIcon_.sprite = iconImageList_[comboIconType_];
            comboIconType_++;
        }


    }

    void SetComboTimeLimitGauge()
    {

        comboTimeLimitGauge_.rectTransform.anchorMin = 
            new Vector2(1.0f - (ScoreManager.Instance.ComboBonusTimer / 5f),
                        comboTimeLimitGauge_.rectTransform.anchorMin.y      );
    }


    private void ComboUI_Reset()
    {
        
    }
}
