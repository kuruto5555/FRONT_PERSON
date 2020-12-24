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
            // ごめんなさい。
            var obj = GameObject.FindGameObjectWithTag(Constants.TagName.PLAYER);

            if (obj)
            {
                player_ = obj.GetComponent<Character.Player>();
            }

            if (player_)
            {
                // センシの初期値を代入
                sensi_slider_.value = player_.GetViewRotateSpeed();

                // スライダーの値を変更したらセンシも変更する
                sensi_slider_.onValueChanged.AddListener((value) => player_.SetViewRotateSpeed((int)value));
            }
        }
    }
}