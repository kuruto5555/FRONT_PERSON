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
                volume_ = Mathf.Clamp01(value * 0.1f);
                bgm_volume_ = bgm_volume_ * volume_;
                se_volume_ = se_volume_ * volume_;
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
                bgm_volume_ = Mathf.Clamp01(value * 0.1f);
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
                se_volume_ = Mathf.Clamp01(value * 0.1f);
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

        /// <summary>
        /// 音源一覧
        /// </summary>
        private List<AudioClip> clip = new List<AudioClip>();

        /// <summary>
        /// 流れる予定の音源
        /// </summary>
        private List<AudioInfo> sound_info_list = new List<AudioInfo>();
        
        /// <summary>
        /// ゲーム全体の最大同時発生音数
        /// </summary>
        private int max_se_num = 32;

        /// <summary>
        /// 同じSEの最大同時発生音数 
        /// </summary>
        private int max_same_se_num = 4;

        /// <summary>
        /// リソースフォルダーから探す用
        /// </summary>
        private const string AUDIO_DIRECTORY_PATH = "Resources/";

        private AudioMixer mixer = null;
        private AudioMixerGroup mixer_master = null;
        private AudioMixerGroup mixer_se = null;
        private AudioMixerGroup mixer_bgm = null;
        private AudioMixerGroup mixer_same_se = null;

        int a = 0;

        private int same_se_count = 0;

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

            //用意したリストに全SE/BGMを入れる
            foreach (var audio_clip in Resources.LoadAll<AudioClip>(directory_name))
            {
                clip.Add(audio_clip);
            }

            mixer = Resources.Load<AudioMixer>("AudioMix");
            mixer_master = mixer.FindMatchingGroups("Master")[0];
            mixer_se = mixer.FindMatchingGroups("Master")[1];
            mixer_same_se = mixer.FindMatchingGroups("Master")[2];
            mixer_bgm = mixer.FindMatchingGroups("Master")[3];
        }

        void Update()
        {
            //再生しているかどうかのチェック
            List<AudioInfo> cheack_sound = new List<AudioInfo>();

            for (int i = 0; i < sound_info_list.Count; i++)
            {
                float new_length = sound_info_list[i].length - Time.deltaTime;

                if (new_length > 0f && sound_info_list[i].source.isPlaying)
                {
                    sound_info_list[i].length = new_length;
                    cheack_sound.Add(sound_info_list[i]);
                }
            }



            //リスト更新
            sound_info_list = cheack_sound;
            cheack_sound.Clear();

            // Debug.Log("sound_info.Count"+sound_info.Count);
            //Debug.Log("play_queue.Count"+play_queue.Count);

            Debug.Log(mixer_master);
            Debug.Log(mixer_se);
            Debug.Log(mixer_same_se);
            Debug.Log(mixer_bgm);

        }

        /// <summary>
        /// 2Dサウンド再生要請
        /// </summary>
        /// <param name="me">オーディオソースアタッチ用</param>
        /// <param name="se_name">再生させたいSE音源</param>
        public void Play2DSE(GameObject me, string se_name)
        {
            AudioSource audiosource = me.GetComponent<AudioSource>();

            AudioSourceSetting(me,ref audiosource, false);

            AudioInfo info = SearchSE(se_name, audiosource);

            PlaySE(info);

        }

        /// <summary>
        /// 3Dサウンド再生要請
        /// </summary>
        /// <param name="me">オーディオソースアタッチ用</param>
        /// <param name="se_name">再生させたいSE音源</param>
        public void Play3DSE(GameObject me, string se_name)
        {
            AudioSource audiosource = me.GetComponent<AudioSource>();

            AudioSourceSetting(me,ref audiosource, true);

            AudioInfo info = SearchSE(se_name,audiosource);

            PlaySE(info);

        }

        private void SetList(AudioInfo info)
        {
            sound_info_list.Add(info);
            //play_queue.Enqueue(info);
        }

        private AudioInfo SearchSE(string path,AudioSource source)
        {
            string sename = path.Replace("SE/", "");

            foreach (var audio in clip)
            {
                if (audio.name == sename)
                {
                    AudioInfo info = new AudioInfo(source, audio, audio.length, max_se_num, max_same_se_num);
                    return info;
                }
            }

            return null;
        }

        private void PlaySE(AudioInfo info)
        {
            //最大同時再生数より少なかったら再生
            if (sound_info_list.Count < max_se_num && SameSoundCheack(info))
            {
                SetList(info);

                SortingList();

                info.source.PlayOneShot(info.clip, 1 * AudioManager.Instance.audio_volume_.SEVolume);

               
                //Debug.Log("a;" + a++);
            }
            else
            {
                //a = 0;

                SetList(info);

                SortingList();

                AudioSource se = sound_info_list[0].source;

                se.Stop();

                info.source.PlayOneShot(info.clip, 1 * AudioManager.Instance.audio_volume_.SEVolume);
            }
        }

        private bool SameSoundCheack(AudioInfo info)
        {
            //same_se_count = 0;

            for (int i = 0; i < sound_info_list.Count; i++)
            {
                if (info.clip.name == sound_info_list[i].clip.name)
                {
                    info.same_count.Add(info.clip.name,same_se_count++);
                }
            }

            Debug.Log(same_se_count++);

            if (same_se_count > max_same_se_num)
            {
                return false;
            }
            
            return true;
        }

        private void AudioSourceSetting(GameObject me,ref AudioSource source, bool sound_3d)
        {
            if (source == null)
            {
                me.AddComponent<AudioSource>();

                source = me.GetComponent<AudioSource>();
            }

            if (sound_3d)
            {
                //3Dサウンド設定
                source.spatialBlend = 1;

                //距離減衰グラフ設定
                source.rolloffMode = AudioRolloffMode.Linear;

                //減衰距離設定
                source.maxDistance = 50f;
            }

            else
            {
                //2Dサウンド設定
                source.spatialBlend = 0;
            }

            //起動時再生をfalseに
            source.playOnAwake = false;

            source.outputAudioMixerGroup = mixer_se;
            
        }

        /// <summary>
        /// 同じフレームで重複したSEを削除
        /// </summary>
        private void SortingList()
        {
            foreach (var base_se in sound_info_list)
            {
                foreach (var target_se in sound_info_list)
                {
                    if (base_se.clip.name == target_se.clip.name && base_se != target_se)
                    {
                        sound_info_list.Remove(target_se);
                    }
                }
            }
        }
    }

    class AudioInfo
    {
        public AudioSource source = null;
        public AudioClip clip = null;
        public float length = 0f;
        public int max_se_num = 0;
        public int max_same_se_num = 0;
        public Dictionary<string, int> same_count = new Dictionary<string, int>();
       
        public AudioInfo(AudioSource source_,AudioClip clip_,float length_,int max_se_num_,int max_same_se_num_)
        {
            source = source_;
            clip = clip_;
            length = length_;
            max_se_num = max_se_num_;
            max_same_se_num = max_same_se_num_;
            same_count.Add(clip.name,0);
        }
    }


}
