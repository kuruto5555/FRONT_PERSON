using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FrontPerson.UI {
    public class RemainingBulletGauge : MonoBehaviour
    {
        [Header("ビタミンのゲージ")]
        [SerializeField]
        Image L_BulletNumGauge_ = null;
        [SerializeField]
        Image R_BulletNumGauge_ = null;
    
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
            L_BulletNumGauge_.rectTransform.localScale = new Vector2((float)player_.GunAmmoL / player_.GunAmmoMAX_L, 1.0f);
            R_BulletNumGauge_.rectTransform.localScale = new Vector2((float)player_.GunAmmoR / player_.GunAmmoMAX_R, 1.0f);
        }
    }

}
