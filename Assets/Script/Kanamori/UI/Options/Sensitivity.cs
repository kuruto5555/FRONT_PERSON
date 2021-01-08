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
        private Slider side_sensi_slider_ = null;
        [SerializeField]
        private Slider vertical_sensi_slider_ = null;
        private Character.Player player_;

        // Start is called before the first frame update
        void Start()
        {
            // センシの初期値を代入
            side_sensi_slider_.value = Character.Player.HorizontalRotetaSpeed;
            vertical_sensi_slider_.value = Character.Player.VerticalRotetaSpeed;

            // スライダーの値を変更したらセンシも変更する
            side_sensi_slider_.onValueChanged.AddListener((value) => { Character.Player.HorizontalRotetaSpeed = (int)value; AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_CURSOR); });
            vertical_sensi_slider_.onValueChanged.AddListener((value) => { Character.Player.VerticalRotetaSpeed = (int)value; AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_CURSOR); });
        }
    }
}