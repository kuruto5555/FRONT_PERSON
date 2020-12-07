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
    List<int> iconChengeValueList_ = null;

    [Header("コンボ数の文字の色")]
    [SerializeField]
    List<Color> comboNumTextColorList_ = null;


    /// <summary>
    /// スコアマネージャー
    /// </summary>
    ScoreManager scoreManager_ = null;

    /// <summary>
    /// コンボ数のテキストの輪郭
    /// </summary>
    Outline comboNumTextOutline_ = null;

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
        
        scoreManager_ = ScoreManager.Instance;
        comboNumTextOutline_ = comboNumText_.GetComponent<Outline>();

        SetColor();

        SetComboNum();
    }

    // Update is called once per frame
    void Update()
    {
        SetComboNum();
        SetComboTimeLimitGauge();
    }


    /// <summary>
    /// コンボ数をテキストにセット
    /// </summary>
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

        // コンボの色変更
        if(comboNum_ >= iconChengeValueList_[comboIconType_] && comboIconType_ <= iconImageList_.Count)
        {
            comboIconType_++;
            SetColor();
        }
    }


    /// <summary>
    /// コンボの更新時間ゲージのアップデート
    /// </summary>
    void SetComboTimeLimitGauge()
    {
        if (comboNum_ == 0) return;

        comboTimeLimitGauge_.rectTransform.anchorMin =
            new Vector2(1.0f - (scoreManager_.ComboBonusTimer / scoreManager_.ComboBonusEffectTime),
                        comboTimeLimitGauge_.rectTransform.anchorMin.y );
    }


    /// <summary>
    /// コンボUIのリセット
    /// </summary>
    private void ComboUI_Reset()
    {
        comboNum_ = comboNumPrev_ = 0;
        comboIconType_ = 0;
        SetColor();
        comboTimeLimitGauge_.rectTransform.anchorMin = new Vector2(1.0f, comboTimeLimitGauge_.rectTransform.anchorMin.y);
    }


    private void SetColor()
    {
        comboIcon_.sprite = iconImageList_[comboIconType_];
        comboNumTextOutline_.effectColor = comboNumTextColorList_[comboIconType_];
    }
}
