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
        Image handGunBulletGaugeL_ = null;
        [SerializeField]
        Image handGunBulletGaugeR_ = null;

        [Header("スペシャル銃の弾のゲージ")]
        [SerializeField]
        Image shotgunBulletGauge_ = null;
        [SerializeField]
        Image machineGunBulletGauge_ = null;
        [SerializeField]
        Image MissileBulletGauge_ = null;

        [Header("アニメーター")]
        [SerializeField]
        Animator handGunBulletGaugeAnimator_ = null;
        [SerializeField]
        Animator shotgunBulletGaugeAnimator_ = null;
        [SerializeField]
        Animator machineGunBulletGaugeAnimator_ = null;
        [SerializeField]
        Animator MissileBulletGaugeAnimator_ = null;

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

            handGunBulletGaugeAnimator_.gameObject.SetActive(true);
            machineGunBulletGaugeAnimator_.gameObject.SetActive(false);
            shotgunBulletGaugeAnimator_.gameObject.SetActive(false);
            MissileBulletGaugeAnimator_.gameObject.SetActive(false);

            GetWeaponType();
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
                            Debug.LogError("ハンドガンからハンドガンに武器が変わるという、仕様にない動き");
                            break;

                        case WEAPON_TYPE.ASSAULT_RIFLE:
                            ChangeWeapon_SpecialWeapon_from_HandGun(machineGunBulletGaugeAnimator_);
                            break;

                        case WEAPON_TYPE.SHOT_GUN:
                            ChangeWeapon_SpecialWeapon_from_HandGun(shotgunBulletGaugeAnimator_);
                            break;

                        case WEAPON_TYPE.MISSILE:
                            ChangeWeapon_SpecialWeapon_from_HandGun(MissileBulletGaugeAnimator_);
                            break;

                        // 予期しない値
                        default:
                            Debug.LogError("武器のタイプが設定されていません");
                            break;
                    }
                    break;

                // 前の武器がマシンガン
                case WEAPON_TYPE.ASSAULT_RIFLE:
                    ChangeWeapon_from_SpecialWeapon(machineGunBulletGaugeAnimator_);
                    break;

                // 前の武器がショットガン
                case WEAPON_TYPE.SHOT_GUN:
                    ChangeWeapon_from_SpecialWeapon(shotgunBulletGaugeAnimator_);
                    break;

                // 前の武器がミサイル
                case WEAPON_TYPE.MISSILE:
                    ChangeWeapon_from_SpecialWeapon(MissileBulletGaugeAnimator_);
                    break;


                // ここに来てはいけない
                default:
                    Debug.LogError("武器のタイプが設定されていません");
                    break;
            }
        }


        /// <summary>
        /// 弾の残弾ゲージ変更
        /// </summary>
        void SetParameter()
        {
            handGunBulletGaugeL_.rectTransform.localScale = new Vector2((float)player_.GunAmmoL / player_.GunAmmoMAX_L, 1.0f);
            handGunBulletGaugeR_.rectTransform.localScale = new Vector2((float)player_.GunAmmoR / player_.GunAmmoMAX_R, 1.0f);

            if (player_.IsSpecialWeapon)
            {
                switch (player_.GetWeaponList[2].GetWeaponType)
                {
                    case WEAPON_TYPE.ASSAULT_RIFLE:
                        machineGunBulletGauge_.rectTransform.localScale = new Vector2((float)player_.GetWeaponList[2].Ammo / player_.GetWeaponList[2].MaxAmmo_, 1.0f);
                        break;

                    case WEAPON_TYPE.SHOT_GUN:
                        shotgunBulletGauge_.rectTransform.localScale = new Vector2((float)player_.GetWeaponList[2].Ammo / player_.GetWeaponList[2].MaxAmmo_, 1.0f);
                        break;

                    case WEAPON_TYPE.MISSILE:
                        MissileBulletGauge_.rectTransform.localScale = new Vector2((float)player_.GetWeaponList[2].Ammo / player_.GetWeaponList[2].MaxAmmo_, 1.0f);
                        break;
                }
            }
        }

        /// <summary>
        /// 現在の武器の種類の取得
        /// </summary>
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


        /// <summary>
        /// ハンドガンからスペシャル武器に変わった時
        /// </summary>
        /// <param name="spWeaponAnimator">切り替わったSP武器のアニメーター</param>
        void ChangeWeapon_SpecialWeapon_from_HandGun(Animator spWeaponAnimator)
        {
            // ハンドガンを後ろにするアニメーション再生
            handGunBulletGaugeAnimator_.Play("HandGunBulletGaugeOff");
            // SP武器を出すアニメーション
            spWeaponAnimator.gameObject.SetActive(true);
            spWeaponAnimator.Play("SpecialWeaponBulletGaugeIn");
        }


        /// <summary>
        /// スペシャル武器から変わったとき
        /// </summary>
        /// <param name="oldSPWeaponAnimator">切り替わる前のSP武器のアニメーター</param>
        void ChangeWeapon_from_SpecialWeapon(Animator oldSPWeaponAnimator)
        {
            switch (weaponType_)
            {
                // ハンドガンに変わったとき
                case WEAPON_TYPE.HANDGUN:
                    // SP武器を無くすアニメーション
                    oldSPWeaponAnimator.Play("SpecialWeaponBulletGaugeOut");
                    // ハンドガンを手前にするアニメーション再生
                    handGunBulletGaugeAnimator_.Play("HandGunBulletGaugeOn");
                    break;

                case WEAPON_TYPE.ASSAULT_RIFLE:
                    ChangeWeapon_SpecialWeapon_from_SpecialWeapon(
                        machineGunBulletGaugeAnimator_,
                        oldSPWeaponAnimator);
                    break;

                case WEAPON_TYPE.SHOT_GUN:
                    ChangeWeapon_SpecialWeapon_from_SpecialWeapon(
                        shotgunBulletGaugeAnimator_,
                        oldSPWeaponAnimator);
                    break;

                case WEAPON_TYPE.MISSILE:
                    ChangeWeapon_SpecialWeapon_from_SpecialWeapon(
                        MissileBulletGaugeAnimator_,
                        oldSPWeaponAnimator);
                    break;

                default:
                    Debug.LogError("武器のタイプが設定されていません");
                    break;
            }

        }


        /// <summary>
        /// SP武器からSP武器に切り替わったとき
        /// </summary>
        /// <param name="newSPWeaponAnimator">切り替わったSP武器のアニメーター</param>
        /// <param name="oldSPWeaponAnimator">切り替わる前のSP武器のアニメーター</param>
        void ChangeWeapon_SpecialWeapon_from_SpecialWeapon(Animator newSPWeaponAnimator, Animator oldSPWeaponAnimator)
        {
            // 前のSP武器を無くすアニメーション
            oldSPWeaponAnimator.Play("SpecialWeaponBulletGaugeOut");
            // 新しいSP武器を出すアニメーション
            newSPWeaponAnimator.gameObject.SetActive(true);
            newSPWeaponAnimator.Play("SpecialWeaponBulletGaugeIn");
        }

        public void SetActive(GameObject gameObject, bool value)
        {
            gameObject.SetActive(value);
        }
    }

}
