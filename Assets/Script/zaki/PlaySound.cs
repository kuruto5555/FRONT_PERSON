using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
using FrontPerson.Manager;
using FrontPerson.Constants;


namespace FrontPerson.Audio
{
    public class PlaySound : MonoBehaviour
    {
        //オーディオソース
        private AudioSource audiosorce = null;

        //オーディオクリップ
        [Header("SE音源")]
        [SerializeField] List<AudioClip> se_list = new List<AudioClip>();

        /// <summary>
        /// 2Dサウンド再生
        /// </summary>
        /// <param name="me">オーディオソースアタッチ用</param>
        /// <param name="se_name">再生させたいSE音源</param>
        public void Play2DSound(GameObject me,string se_name)
        {
            audiosorce = me.GetComponent<AudioSource>();

            if (audiosorce == null)
            {
                me.AddComponent<AudioSource>();

                audiosorce = me.GetComponent<AudioSource>();
            }

            AudioSourceSetting(false);

            SearchAndPlaySE(se_name);
        }

        /// <summary>
        /// 3Dサウンド再生
        /// </summary>
        /// <param name="me">オーディオソースアタッチ用</param>
        /// <param name="se_name">再生させたいSE音源</param>
        public void Play3DSound(GameObject me, string se_name)
        {
            audiosorce = me.GetComponent<AudioSource>();

            if (audiosorce == null)
            {
                me.AddComponent<AudioSource>();

                audiosorce = me.GetComponent<AudioSource>();
            }

            AudioSourceSetting(true);

            SearchAndPlaySE(se_name);
        }

        private void AudioSourceSetting(bool sound_3d)
        {
            if (sound_3d)
            {
                //3Dサウンド設定
                audiosorce.spatialBlend = 1;

                //距離減衰グラフ設定
                audiosorce.rolloffMode = AudioRolloffMode.Linear;

                //減衰距離設定
                audiosorce.maxDistance = 50f;
            }

            //起動時再生をfalseに
            audiosorce.playOnAwake = false;

            audiosorce.priority = 0;
        }

        private void SearchAndPlaySE(string se_name)
        {
            string sename = se_name.Replace("SE/", "");

            foreach (var se in se_list)
            {

                if (se.name == sename)
                {
                    //再生
                    audiosorce.PlayOneShot(se, 1.0f * AudioManager.Instance.audio_volume_.SEVolume);
                }
            }
        }
    }
}