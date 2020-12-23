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
    ComboManager comboManager_ = null;

    /// <summary>
    /// コンボ数のテキストの輪郭
    /// </summary>
    Outline comboNumTextOutline_ = null;

    /// <summary>
    /// 現在のコンボ数
    /// </summary>
    int comboNum_ = 0;

    /// <summary>
    /// コンボのアイコンの種類
    /// </summary>
    int comboIconType_ = 0;


    // Start is called before the first frame update
    void Start()
    {
        comboNum_ = 0;
        comboIconType_ = 0;

        comboManager_ = ComboManager.Instance;
        comboNumTextOutline_ = comboNumText_.GetComponent<Outline>();

        SetTextComboNum();
        SetColor();

    }


    // Update is called once per frame
    void Update()
    {
        UpdateComboTimeLimitGauge();

        // コンボ数が0なら無効にする
        if(comboManager_.ComboNum <= 0)
        {
            ComboUI_Reset();
        }
    }


    public void SetComboNum()
    {
        SetTextComboNum();
        SetColor();
        UpdateComboTimeLimitGauge();
        gameObject.SetActive(true);
    }


    /// <summary>
    /// コンボ数をテキストにセット
    /// </summary>
    void SetTextComboNum()
    {
        comboNum_ = comboManager_.ComboNum;
        comboNumText_.text = comboNum_.ToString();
    }


    /// <summary>
    /// コンボの継続時間ゲージの更新
    /// </summary>
    void UpdateComboTimeLimitGauge()
    {
        if (comboNum_ <= 0) return;

        comboTimeLimitGauge_.fillAmount = comboManager_.ComboRemainingTime / comboManager_.ComboDuration;

    }


    /// <summary>
    /// コンボUIのリセット
    /// </summary>
    private void ComboUI_Reset()
    {
        comboNum_ = 0;
        comboNumText_.text = "0".ToString();
        comboIconType_ = 0;
        SetColor();
        comboTimeLimitGauge_.fillAmount = 0f;
        gameObject.SetActive(false);
    }


    private void SetColor()
    {
        if (comboIconType_ >= iconImageList_.Count) return;
        if (comboNum_ < iconChengeValueList_[comboIconType_]) return;

        comboIcon_.sprite = iconImageList_[comboIconType_];
        comboNumTextOutline_.effectColor = comboNumTextColorList_[comboIconType_];
        comboIconType_++;
    }
}
