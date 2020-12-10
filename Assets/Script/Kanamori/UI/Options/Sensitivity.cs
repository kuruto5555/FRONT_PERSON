using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    public class Sensitivity : MonoBehaviour
    {
        [SerializeField]
        private Slider sensi_slider_;
        private Character.Player player_;

        // Start is called before the first frame update
        void Start()
        {
            // ごめんなさい。
            player_ = GameObject.FindGameObjectWithTag(Constants.TagName.PLAYER).GetComponent<Character.Player>();

            if (player_)
            {
                sensi_slider_.onValueChanged.AddListener((value) => player_.SetViewRotateSpeed((int)value));
            }
        }
    }
}