using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FrontPerson.Constants;

namespace FrontPerson.UI {


    public class RemainingBulletGauge : MonoBehaviour
    {
        [Header("銃の弾のゲージ")]
        [SerializeField]
        Image L_BulletNumGauge_ = null;
        [SerializeField]
        Image R_BulletNumGauge_ = null;
        [SerializeField]
        Image SpecialBulletNumGauge_ = null;

        // 弾丸の形の彫刻の画像なんだけど今はないから
        // テキストで表示する
        /// <summary>
        /// 弾丸の形の彫刻の画像
        /// </summary>
        [Header("弾丸の形の彫刻")]
        [SerializeField]
        GameObject defualt_ = null;
        [SerializeField]
        GameObject machineGun_ = null;
        [SerializeField]
        GameObject shotgun_ = null;
        [SerializeField]
        GameObject homingLauncher_ = null;


        /// <summary>
        /// プレイヤー
        /// </summary>
        Character.Player player_ = null;

        // 現在の銃の種類
        WEAPON_TYPE gunType_ = WEAPON_TYPE.HANDGUN;


        // Start is called before the first frame update
        void Start()
        {
            player_ = FindObjectOfType<Character.Player>();
            //gunType_ = player.GunType;
            ChangeGaugeUI();
            SetParameter();
        }
    
        // Update is called once per frame
        void Update()
        {
            ChangeGaugeUI();
            SetParameter();
        }


        /// <summary>
        /// 銃の種類が変わっていたらUIを変更する
        /// </summary>
        void ChangeGaugeUI()
        {
            //if(gunType_ != player_.GunType)
            //{
            //    gunType_ = player_.GunType;
                switch (gunType_)
                {
                    case WEAPON_TYPE.ASSAULT_RIFLE:
                        defualt_.SetActive(false);
                        machineGun_.SetActive(true);
                        shotgun_.SetActive(false);
                        homingLauncher_.SetActive(false);
                        L_BulletNumGauge_.enabled = false;
                        R_BulletNumGauge_.enabled = false;
                        SpecialBulletNumGauge_.enabled = true;
                        break;
            
                    case WEAPON_TYPE.SHOT_GUN:
                        defualt_.SetActive(false);
                        machineGun_.SetActive(false);
                        shotgun_.SetActive(true);
                        homingLauncher_.SetActive(false);
                        L_BulletNumGauge_.enabled = false;
                        R_BulletNumGauge_.enabled = false;
                        SpecialBulletNumGauge_.enabled = true;
                        break;
            
                    case WEAPON_TYPE.MISSILE:
                        defualt_.SetActive(false);
                        machineGun_.SetActive(false);
                        shotgun_.SetActive(false);
                        homingLauncher_.SetActive(true);
                        L_BulletNumGauge_.enabled = false;
                        R_BulletNumGauge_.enabled = false;
                        SpecialBulletNumGauge_.enabled = true;
                        break;
            
                    case WEAPON_TYPE.HANDGUN:
                        defualt_.SetActive(true);
                        machineGun_.SetActive(false);
                        shotgun_.SetActive(false);
                        homingLauncher_.SetActive(false);
                        L_BulletNumGauge_.enabled = true;
                        R_BulletNumGauge_.enabled = true;
                        SpecialBulletNumGauge_.enabled = false;
                        break;
                }
            //}
        }


        /// <summary>
        /// 弾の残弾ゲージ変更
        /// </summary>
        void SetParameter()
        {
            switch (gunType_)
            {
                case WEAPON_TYPE.HANDGUN:
                    L_BulletNumGauge_.rectTransform.localScale = new Vector2((float)player_.GunAmmoL / player_.GunAmmoMAX_L, 1.0f);
                    R_BulletNumGauge_.rectTransform.localScale = new Vector2((float)player_.GunAmmoR / player_.GunAmmoMAX_R, 1.0f);
                    break;
                
                default:
                    //SpecialBulletNumGauge_.rectTransform.localScale = new Vector2((float)player_.SpecialWeaponAmmo / player_.SpecialWeaponAmmoMAX);
                    break;
            
            }
        }
    }

}
