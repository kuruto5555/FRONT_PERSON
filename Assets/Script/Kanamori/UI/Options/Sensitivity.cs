using FrontPerson.Constants;
using FrontPerson.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    public class Sensitivity : MonoBehaviour
    {
        [SerializeField]
        private Slider sensi_slider_ = null;
        private Character.Player player_;

        // Start is called before the first frame update
        void Start()
        {
            // センシの初期値を代入
            sensi_slider_.value = Character.Player.ViewRotetaSpeed;

            // スライダーの値を変更したらセンシも変更する
            sensi_slider_.onValueChanged.AddListener((value) => { Character.Player.ViewRotetaSpeed = (int)value; AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_CURSOR); });
        }
    }
}