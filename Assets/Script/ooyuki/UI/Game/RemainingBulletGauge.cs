using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FrontPerson.Constants;
using UnityEditorInternal;

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
        Text defualt_ = null;
        [SerializeField]
        Text machineGun_ = null;
        [SerializeField]
        Text shotgun_ = null;
        [SerializeField]
        Text homingLauncher_ = null;


        /// <summary>
        /// プレイヤー
        /// </summary>
        Character.Player player_ = null;

        // 現在の銃の種類
        //GUN_TYPE gunType_ = GUN_TYPE.DEFAULT;


        // Start is called before the first frame update
        void Start()
        {
            player_ = FindObjectOfType<Character.Player>();
            //gunType_ = player.GunType;
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
            /*if(gunType_ != player_.GunType)
            {
                gunType_ = player_.GunType;
                switch (gunType_)
                {
                    case MACHINEGUN:
                        defualt.IsActive(false);
                        MachineGun.IsActive(true);
                        Shotgun.IsActive(false);
                        HomingLauncher.IsActive(false);
                        break;

                    case SHOTGUN:
                        defualt.IsActive(false);
                        MachineGun.IsActive(false);
                        Shotgun.IsActive(true);
                        HomingLauncher.IsActive(false);
                        break;

                    case HOMINGLAUNCHER:
                        defualt.IsActive(false);
                        MachineGun.IsActive(false);
                        Shotgun.IsActive(false);
                        HomingLauncher.IsActive(true);
                        break;

                    case DEFAULT:
                        defualt.IsActive(true);
                        MachineGun.IsActive(flase);
                        Shotgun.IsActive(false);
                        HomingLauncher.IsActive(false);
                        break;
                }
            }*/
        }


        /// <summary>
        /// 弾の残弾ゲージ変更
        /// </summary>
        void SetParameter()
        {
            //switch (gunType_)
            //{
            //    case DEFAULT:
                    L_BulletNumGauge_.rectTransform.localScale = new Vector2((float)player_.GunAmmoL / player_.GunAmmoMAX_L, 1.0f);
                    R_BulletNumGauge_.rectTransform.localScale = new Vector2((float)player_.GunAmmoR / player_.GunAmmoMAX_R, 1.0f);
            //        break;
            //    
            //    default:
            //        SpecialBulletNumGauge_.rectTransform.localRotation = new Vector2((float)player_.SpecialWeaponAmmo / player_.SpecialWeaponAmmoMAX);
            //        break;
            //
            //}
        }
    }

}
