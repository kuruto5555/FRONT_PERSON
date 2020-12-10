//using System.Collections;
//using System.IO;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Audio;
//using UnityEditor;
//using FrontPerson.Manager;
//using FrontPerson.Constants;


//namespace FrontPerson.Audio
//{
//    public class PlaySound : MonoBehaviour
//    {
//        //オーディオソース
//        private AudioSource audiosorce = null;

//        [Header("SE音源")]
//        [SerializeField] List<AudioClip> se_list = new List<AudioClip>();

//        private List<AudioClip> a = new List<AudioClip>();


//        /// <summary>
//        /// 2Dサウンド再生
//        /// </summary>
//        /// <param name="me">オーディオソースアタッチ用</param>
//        /// <param name="se_name">再生させたいSE音源</param>
//        public void Play2DSound(GameObject me, string se_name)
//        {
//            audiosorce = me.GetComponent<AudioSource>();

//            AudioSourceSetting(me,false);

//            SearchSE(se_name, audiosorce);
//        }

//        /// <summary>
//        /// 3Dサウンド再生
//        /// </summary>
//        /// <param name="me">オーディオソースアタッチ用</param>
//        /// <param name="se_name">再生させたいSE音源</param>
//        public void Play3DSound(GameObject me, string se_name)
//        {
//            audiosorce = me.GetComponent<AudioSource>();

//            AudioSourceSetting(me,true);

//            SearchSE(se_name, audiosorce);
//        }

//        private void AudioSourceSetting(GameObject me, bool sound_3d)
//        {
//            if (audiosorce == null)
//            {
//                me.AddComponent<AudioSource>();

//                audiosorce = me.GetComponent<AudioSource>();
//            }

//            if (sound_3d)
//            {
//                //3Dサウンド設定
//                audiosorce.spatialBlend = 1;

//                //距離減衰グラフ設定
//                audiosorce.rolloffMode = AudioRolloffMode.Linear;

//                //減衰距離設定
//                audiosorce.maxDistance = 50f;
//            }

//            //起動時再生をfalseに
//            audiosorce.playOnAwake = false;

//            audiosorce.priority = 0;
//        }

//        private void SearchSE(string se_name,AudioSource audio)
//        {
//            string sename = se_name.Replace("SE/", "");

//            foreach (var se in se_list)
//            {

//                if (se.name == sename)
//                {
//                    //再生
//                    //SEManager.Instance.AddSEList(audio, se);
//                    a.Add(se);
//                    if(SEManager.Instance.JudgePlaySE(audio,se))
//                        audiosorce.PlayOneShot(se, 1 * AudioManager.Instance.audio_volume_.SEVolume);
//                    Debug.Log(se.name + Time.time);
//                    Debu
//                }
//            }
//        }

        
//    }

//} 
    
