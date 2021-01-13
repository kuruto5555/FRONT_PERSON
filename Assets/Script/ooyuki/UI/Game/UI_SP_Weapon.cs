using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FrontPerson.Character;
using FrontPerson.Manager;
using System.CodeDom.Compiler;
using FrontPerson.UI;
using FrontPerson.Constants;

namespace FrontPerson.UI
{
    public class UI_SP_Weapon : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("- None -アイコンのオブジェクト")]
        GameObject noneText_ = null;

        [SerializeField]
        [Tooltip("マシンガンアイコンのオブジェクト")]
        GameObject machineGunIcon_ = null;

        [SerializeField]
        [Tooltip("ショットガンアイコンのオブジェト")]
        GameObject shotgunIcon_ = null;

        [SerializeField]
        [Tooltip("ランチャーアイコンのオブジェト")]
        GameObject launcherIcon_ = null;

        [SerializeField]
        [Tooltip("SP武器がもらえるまでのゲージアイコンのリスト")]
        List<GameObject> gauge_ = null;

        /// <summary>
        /// プレイヤー
        /// </summary>
        Player player_ = null;

        /// <summary>
        /// バウンティーManager
        /// </summary>
        BountyManager bountyManager_ = null;

        /// <summary>
        /// ゲージの値
        /// </summary>
        int meterNum_ = 0;

        /// <summary>
        /// 武器を入手したエフェクトを再生中かどうか
        /// </summary>
        bool isGetWeaponEffect_ = false;

        // Start is called before the first frame update
        void Start()
        {
            player_ = FindObjectOfType<Player>();
            bountyManager_ = BountyManager._instance;

            foreach (var meter in gauge_)
            {
                meter.SetActive(false);
            }


        }

        // Update is called once per frame
        void Update()
        {
            if (isGetWeaponEffect_)
            {
                //アニメーション再生待ち
                if (gauge_[gauge_.Count - 1].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f) return;


                //アニメーションが終わったらゲージをリセット
                foreach (var meter in gauge_)
                {
                    meter.SetActive(false);
                }
                isGetWeaponEffect_ = false;


                //エフェクト中に増えたゲージを有効か
                for(int i = 0; i < meterNum_; i++)
                {
                    gauge_[i].SetActive(true);
                }
                meterNum_ = 0;


                // アイコンをNone以外フォルス
                noneText_.SetActive(true);
                machineGunIcon_.SetActive(false);
                shotgunIcon_.SetActive(false);
                launcherIcon_.SetActive(false);

            }
        }

        public void clearMission()
        {
            if (!isGetWeaponEffect_)
            {
                gauge_[meterNum_].SetActive(true);
                gauge_[meterNum_].GetComponent<Animator>().Play("Effect");
            }

            meterNum_++;
        }

        public void GetSPWeapon(int weponType)
        {
            gauge_[2].SetActive(true);
            isGetWeaponEffect_ = true;
            meterNum_ = 0;

            //入手時は全部のメーターにEffectをかける
            gauge_[0].GetComponent<Animator>().Play("Effect");
            gauge_[1].GetComponent<Animator>().Play("Effect");
            gauge_[2].GetComponent<Animator>().Play("Effect");

            // いったんアイコンを全部フォルス
            noneText_.SetActive(false);
            machineGunIcon_.SetActive(false);
            shotgunIcon_.SetActive(false);
            launcherIcon_.SetActive(false);

            if (player_ == null) player_ = FindObjectOfType<Player>();
            switch (weponType)
            {
                case (int)WEAPON_TYPE.ASSAULT_RIFLE:
                    machineGunIcon_.SetActive(true);
                    break;

                case (int)WEAPON_TYPE.SHOT_GUN:
                    shotgunIcon_.SetActive(true);
                    break;

                case (int)WEAPON_TYPE.MISSILE:
                    launcherIcon_.SetActive(true);
                    break;
            }
        }
    }
}
