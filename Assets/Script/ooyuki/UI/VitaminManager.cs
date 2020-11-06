using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FrontPerson.UI {
    public class VitaminManager : MonoBehaviour
    {
        [Header("ビタミンのゲージ")]
        [SerializeField]
        List<Image> vitamins_ = null;
    
        [Header("プレイヤー")]
        [SerializeField]
        Character.Player player_ = null;


        // Start is called before the first frame update
        void Start()
        {
            SetParameter();
        }
    
        // Update is called once per frame
        void Update()
        {
            SetParameter();
        }

        void SetParameter()
        {
            vitamins_[0].rectTransform.localScale = new Vector2((float)player_.GunAmmoL / player_.GunAmmoMAX_L, 1.0f);
            vitamins_[1].rectTransform.localScale = new Vector2((float)player_.GunAmmoR / player_.GunAmmoMAX_R, 1.0f);
        }
    }

}
