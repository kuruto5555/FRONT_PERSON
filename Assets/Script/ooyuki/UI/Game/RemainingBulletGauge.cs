using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FrontPerson.Constants;

namespace FrontPerson.UI {


    public class RemainingBulletGauge : MonoBehaviour
    {

        [Header("デフォルト銃の弾のゲージ")]
        [SerializeField]
        Image L_BulletNumGauge_ = null;
        [SerializeField]
        Image R_BulletNumGauge_ = null;

        [Header("スペシャル銃の弾のゲージ")]
        [SerializeField]
        Image SpecialBulletNumGauge_ = null;

        /// <summary>
        /// アニメーター
        /// </summary>
        Animator animator_ = null;

        /// <summary>
        /// スペシャル武器のアイコン
        /// </summary>
        [Header("スペシャル武器のアイコン")]
        [SerializeField]
        GameObject shotgunIcon_ = null;
        [SerializeField]
        GameObject machineGunIcon_ = null;
        [SerializeField]
        GameObject missileIcon_ = null;


        /// <summary>
        /// プレイヤー
        /// </summary>
        Character.Player player_ = null;

        /// <summary>
        /// 現在の銃の種類
        /// </summary>
        WEAPON_TYPE weaponType_ = WEAPON_TYPE.HANDGUN;

        /// <summary>
        /// 前フレームの銃の種類
        /// </summary>
        WEAPON_TYPE oldWeaponType_ = WEAPON_TYPE.HANDGUN;

        /// <summary>
        /// 銃が切り替わったかどうか
        /// </summary>
        bool IsChangeWeapon { get { return weaponType_ != oldWeaponType_; } }


        // Start is called before the first frame update
        void Start()
        {
            player_ = FindObjectOfType<Character.Player>();
            //gunType_ = player_.GunType;
            animator_ = GetComponent<Animator>();


            ChangeGaugeUI();
            SetParameter();
        }
    
        // Update is called once per frame
        void Update()
        {
            GetWeaponType();
            ChangeGaugeUI();
            SetParameter();
        }



        /// <summary>
        /// 銃の種類が変わっていたらUIを変更する
        /// </summary>
        void ChangeGaugeUI()
        {
            if (!IsChangeWeapon) return;

            switch (oldWeaponType_)
            {
                // 変わる前の武器がハンドガンで
                case WEAPON_TYPE.HANDGUN:
                    switch (weaponType_)
                    {
                        //ここには来ない（下のそれ以外でスペシャル武器以外来ないようにするため）
                        case WEAPON_TYPE.HANDGUN:
                            break;

                        //ここに起きてはいけない
                        case WEAPON_TYPE.NONE:
                            Debug.LogError("武器のタイプが設定されていません");
                            break;

                        //スペシャル武器になったとき
                        default:
                            // アイコンセット
                            SetSPWeaponIcon();
                            // アニメーション再生
                            animator_.Play("ChangeHandGun");
                            break;
                    }
                    break;

                // ここに来てはいけない
                case WEAPON_TYPE.NONE:
                    Debug.LogError("武器のタイプが設定されていません");
                    break;
                    
                // 前の武器がスペシャル武器で
                default:
                    switch (weaponType_)
                    {
                        // ハンドガンに変わったとき
                        case WEAPON_TYPE.HANDGUN:
                            // アニメーション再生
                            animator_.Play("ChangeSPWeapon");
                            break;

                        case WEAPON_TYPE.ASSAULT_RIFLE:
                            machineGunIcon_.SetActive(true);
                            shotgunIcon_.SetActive(false);
                            missileIcon_.SetActive(false);
                            break;

                        case WEAPON_TYPE.SHOT_GUN:
                            shotgunIcon_.SetActive(true);
                            machineGunIcon_.SetActive(false);
                            missileIcon_.SetActive(false);
                            break;

                        case WEAPON_TYPE.MISSILE:
                            missileIcon_.SetActive(true);
                            machineGunIcon_.SetActive(false);
                            shotgunIcon_.SetActive(false);
                            break;

                        // ここに来てはいけない
                        case WEAPON_TYPE.NONE:
                            Debug.LogError("武器のタイプが設定されていません");
                            break;

                        default:
                            //アイコンセット
                            SetSPWeaponIcon();
                            break;
                    }
                    break;
            }
        }


        /// <summary>
        /// 弾の残弾ゲージ変更
        /// </summary>
        void SetParameter()
        {
            L_BulletNumGauge_.rectTransform.localScale = new Vector2((float)player_.GunAmmoL / player_.GunAmmoMAX_L, 1.0f);
            R_BulletNumGauge_.rectTransform.localScale = new Vector2((float)player_.GunAmmoR / player_.GunAmmoMAX_R, 1.0f);

            if (player_.IsSpecialWeapon)
            {
                SpecialBulletNumGauge_.rectTransform.localScale = new Vector2((float)player_.GetWeaponList[2].Ammo / player_.GetWeaponList[2].MaxAmmo_, 1.0f);
            }
        }


        void GetWeaponType()
        {
            // 前フレームの銃の種類を保存
            oldWeaponType_ = weaponType_;

            if (player_.IsSpecialWeapon)
            {
                weaponType_ = player_.GetWeaponList[2].GetWeaponType;
            }
            else
            {
                weaponType_ = WEAPON_TYPE.HANDGUN;
            }
        }

        void SetSPWeaponIcon()
        {
            switch (weaponType_)
            {
                case WEAPON_TYPE.ASSAULT_RIFLE:
                    machineGunIcon_.SetActive(true);
                    shotgunIcon_.SetActive(false);
                    missileIcon_.SetActive(false);
                    break;

                case WEAPON_TYPE.SHOT_GUN:
                    shotgunIcon_.SetActive(true);
                    machineGunIcon_.SetActive(false);
                    missileIcon_.SetActive(false);
                    break;

                case WEAPON_TYPE.MISSILE:
                    missileIcon_.SetActive(true);
                    machineGunIcon_.SetActive(false);
                    shotgunIcon_.SetActive(false);
                    break;
            }
        }
    }

}
