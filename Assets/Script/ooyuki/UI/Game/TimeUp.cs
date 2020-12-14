using FrontPerson.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.UI
{
    public class TimeUp : MonoBehaviour
    {
        public bool IsFinissh { get; private set; } = false;

        

        public void Finish()
        {
            IsFinissh = true;
        }

        public void StopCombo_AND_Fever_()
        {
            // ふぇーばータイムだったら終わらせてスコアに入れる
            ScoreManager.Instance.StopFeverTime();
            // コンボが続いてたら終わらせてスコアに入れる
            ComboManager.Instance.LostCombo();
        }
    }
}
