using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FrontPerson.Manager;

public class UI_Combo : MonoBehaviour
{
    [Header("コンボ数表示用のテキスト")]
    [SerializeField]
    private Text conboNum_ = null;



    // Start is called before the first frame update
    void Start()
    {
        SetComboNum();
    }

    // Update is called once per frame
    void Update()
    {
        SetComboNum();
    }

    void SetComboNum()
    {
        conboNum_.text = ScoreManager.Instance.ComboBonus.ToString();
    }
}
