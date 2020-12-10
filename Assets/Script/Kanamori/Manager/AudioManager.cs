using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using FrontPerson.common;
using System.IO;

namespace FrontPerson.Manager
{
    /// <summary>
    /// オーディオの音量情報
    /// </summary>
    [Serializable]
    public class AudioVolume
    {
        [Header("マスタ音量")]
        [SerializeField, Range(0f, 1f)]
        private float volume_ = 1f;

        [Header("BGM音量")]
        [SerializeField, Range(0f, 1f)]
        private float bgm_volume_ = 1f;

        [Header("SE音量")]
        [SerializeField, Range(0f, 1f)]
        private float se_volume_ = 1f;



        public float Volume
        {
            set
            {
                volume_ = Mathf.Clamp01(value);
            }
            get
            {
                return volume_;
            }
        }
        public float BGMVolume
        {
            set
            {
                bgm_volume_ = Mathf.Clamp01(value);
                bgm_volume_ = bgm_volume_ * volume_;
            }
            get
            {
                return bgm_volume_;
            }
        }
        public float SEVolume
        {
            set
            {
                se_volume_ = Mathf.Clamp01(value);
                se_volume_ = se_volume_ * volume_;
            }
            get
            {
                return se_volume_;
            }
        }

        /// <summary>
        /// セーブデータができるまで
        /// </summary>
        public AudioVolume()
        {
            volume_ = 1f;
            bgm_volume_ = 1f;
            se_volume_ = 1f;
        }
    }

    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        public AudioVolume audio_volume_;

        private List<AudioClip> clip = new List<AudioClip>();

        //最大同時発生音数
        private int max_se_num = 32;

        /// <summary>
        /// 今なってるSE/BGMの長さ
        /// </summary>
        private List<float> now_play_sounds_length = new List<float>();

        //オーディオソース
        private AudioSource audiosorce = null;

        //リソースフォルダーから探す用
        private const string AUDIO_DIRECTORY_PATH = "Resources/";

        Dictionary<AudioClip, AudioClipInfo> audioClips = new Dictionary<AudioClip, AudioClipInfo>();

        public void Init()
        {
            // セーブデータができたら修正
            audio_volume_ = new AudioVolume();
        }

        // Start is called before the first frame update
        void Start()
        {
            //オーディオファイルへのパスを抽出
            string directory_name = Path.GetFileName(AUDIO_DIRECTORY_PATH);
            var audio_path_dict = new Dictionary<string, string>();

            foreach (var audio_clip in Resources.LoadAll<AudioClip>(directory_name))
            {
                clip.Add(audio_clip);
            }
        }

        void Update()
        {
            //// playing SE update

            //Debug.Log("更新前" + now_play_sounds_length.Count);

            //List<float> newList = new List<float>();
            //foreach (var len in now_play_sounds_length)
            //{
            //    float newlen = len - Time.deltaTime;
            //    if (newlen > 0.0f)
            //        newList.Add(newlen);
            //}
            //now_play_sounds_length = newList;
            //Debug.Log("更新後" + now_play_sounds_length.Count);



            // playing SE update
            foreach (AudioClipInfo info in audioClips.Values)
            {
                List<SEInfo> newList = new List<SEInfo>();

                foreach (SEInfo seInfo in info.playingList)
                {
                    seInfo.curTime = seInfo.curTime - Time.deltaTime;
                    if (seInfo.curTime > 0.0f)
                        newList.Add(seInfo);
                    else
                        info.stockList.Add(seInfo.index, seInfo);
                }
                info.playingList = newList;
            }


        }

        /// <summary>
        /// 2Dサウンド再生要請
        /// </summary>
        /// <param name="me">オーディオソースアタッチ用</param>
        /// <param name="se_name">再生させたいSE音源</param>
        public void Play2DSound(GameObject me, string se_name)
        {
            audiosorce = me.GetComponent<AudioSource>();

            AudioSourceSetting(me, false);

            AudioClip clip = SearchSE(se_name);

            PlaySE(audiosorce, clip);

        }

        /// <summary>
        /// 3Dサウンド再生要請
        /// </summary>
        /// <param name="me">オーディオソースアタッチ用</param>
        /// <param name="se_name">再生させたいSE音源</param>
        public void Play3DSound(GameObject me, string se_name)
        {
            audiosorce = me.GetComponent<AudioSource>();

            AudioSourceSetting(me, true);

            AudioClip clip = SearchSE(se_name);

            PlaySE(audiosorce, clip);

        }

        private void SetDic(AudioClip clip,AudioClipInfo clipinfo)
        {
            audioClips.Add(clip, clipinfo);
        }

        private AudioClip SearchSE(string path)
        {
            string sename = path.Replace("SE/", "");

            foreach (var audio in clip)
            {
                if (audio.name == sename)
                {
                    SetDic(audio, new AudioClipInfo(path, max_se_num, AudioManager.Instance.audio_volume_.SEVolume));
                    return audio;
                }
            }

            return null;
        }

        //private void PlaySE(AudioSource source, AudioClip clip)
        //{
        //    audiosorce.PlayOneShot(clip, 1 * AudioManager.Instance.audio_volume_.SEVolume);
        //    Debug.Log("SearchSE");
        //}

        private void PlaySE(AudioSource source, AudioClip clip)
        {
            AudioClipInfo info = audioClips[clip];

            float len = info.clip.length;
            if (info.stockList.Count > 0)
            {
                SEInfo seInfo = info.stockList.Values[0];
                seInfo.curTime = len;
                info.playingList.Add(seInfo);

                // remove from stock
                info.stockList.Remove(seInfo.index);

                // Play SE
                source.PlayOneShot(info.clip, seInfo.volume);

              
            }
            
        }

        private void AudioSourceSetting(GameObject me, bool sound_3d)
        {
            if (audiosorce == null)
            {
                me.AddComponent<AudioSource>();

                audiosorce = me.GetComponent<AudioSource>();
            }

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
    }

    public class NewtonMethod
    {
        public delegate float Func(float x);

        public static float run(Func func, Func derive, float initX, int maxLoop)
        {
            float x = initX;
            for (int i = 0; i < maxLoop; i++)
            {
                float curY = func(x);
                if (curY < 0.00001f && curY > -0.00001f)
                    break;
                x = x - curY / derive(x);
            }
            return x;
        }
    }

    class SEInfo
    {
        public int index = 0;
        public float curTime = 0f;
        public float volume = 0f;
        private int i;
        private float v1;
        private float v2;

        public SEInfo(int i, float v1, float v2)
        {
            this.i = i;
            this.v1 = v1;
            this.v2 = v2;
        }
    }

    class AudioClipInfo
    {
        public SortedList<int, SEInfo> stockList = new SortedList<int, SEInfo>();
        public List<SEInfo> playingList = new List<SEInfo>();
        public int maxSENum = 10;
        public float initVolume = 1.0f;
        public float attenuate = 0.0f;
        public AudioClip clip;

        public AudioClipInfo(string name, int maxSENum, float initVolume)
        {
            //this.name = name;
            this.maxSENum = maxSENum;

            this.initVolume = initVolume;
            attenuate = calcAttenuateRate();

            // create stock list
            for (int i = 0; i < maxSENum; i++)
            {
                SEInfo seInfo = new SEInfo(i, 0f, initVolume * Mathf.Pow(attenuate, i));
                stockList.Add(seInfo.index, seInfo);
            }
        }

        private float calcAttenuateRate()
        {
            float n = maxSENum;
            return NewtonMethod.run(
                delegate (float p)
                {
                    return (1.0f - Mathf.Pow(p, n)) / (1.0f - p) - 1.0f / initVolume;
                },
                delegate (float p)
                {
                    float ip = 1.0f - p;
                    float t0 = -n * Mathf.Pow(p, n - 1.0f) / ip;
                    float t1 = (1.0f - Mathf.Pow(p, n)) / ip / ip;
                    return t0 + t1;
                },
                0.9f, 100
            );
        }
    }


    
}
