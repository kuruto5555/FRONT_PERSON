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
        public float volume_ = 1f;

        [Header("BGM音量")]
        [SerializeField, Range(0f, 1f)]
        public float bgm_volume_ = 1f;

        [Header("SE音量")]
        [SerializeField, Range(0f, 1f)]
        public float se_volume_ = 1f;

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
        /// リソースフォルダーから探す用
        /// </summary>
        private const string AUDIO_DIRECTORY_PATH = "Resources/";

        /// <summary>
        /// ゲーム全体の最大同時発生音数
        /// </summary>
        private const int MAX_SAME_COUNT = 32;

        /// <summary>
        /// 同じSEの最大同時発生音数 
        /// </summary>
        private const int MAX_SAME_SE_COUNT = 4;

        /// <summary>
        /// 音源一覧
        /// </summary>
        private List<AudioClip> clip = new List<AudioClip>();

        /// <summary>
        /// 流れる予定のSE音源
        /// </summary>
        private List<AudioInfo> sound_info_list = new List<AudioInfo>();

        /// <summary>
        /// オーディオ3Dプレハブ
        /// </summary>
        private GameObject audio_3D_prefab = null;

        /// <summary>
        /// BGM専用オーディオソース
        /// </summary>
        private AudioSource bgm_audiosource = null;

        /// <summary>
        /// オーディオミキサー
        /// </summary>
        private AudioMixer mixer = null;

        /// <summary>
        /// 親ミキサー
        /// </summary>
        private AudioMixerGroup mixer_master = null;

        /// <summary>
        /// SEミキサー
        /// </summary>
        private AudioMixerGroup mixer_se = null;

        /// <summary>
        /// BGMミキサー
        /// </summary>
        private AudioMixerGroup mixer_bgm = null;

        /// <summary>
        /// フェードインbool
        /// </summary>
        private bool fade_in = false;

        /// <summary>
        /// フェードアウトbool
        /// </summary>
        private bool fade_out = false;

        /// <summary>
        /// オーディオミキサーの最高音量(db表示)
        /// ここ変えれば100％時の音量上限が変わる
        /// </summary>
        private float bgm_mixer_max_volume = 10f;

        /// <summary>
        /// オーディオミキサーの最低音量(db表示)
        /// </summary>
        private const float bgm_mixer_min_volume = 80f;

        /// <summary>
        /// SEのボリューム
        /// </summary>
        private float se_volume = 1f;

        /// <summary>
        /// BGMフェード完了時間
        /// </summary>
        private  float fade_time = 1f;

        public float Volume
        {
            set
            {
                audio_volume_.volume_ = Mathf.Clamp01(value * 0.1f);
                bgm_audiosource.volume = BGMVolume * audio_volume_.volume_;
                se_volume = SEVolume * audio_volume_.volume_;
            }
            get
            {
                return audio_volume_.volume_;
            }
        }
        public float BGMVolume
        {
            set
            {
                audio_volume_.bgm_volume_ = Mathf.Clamp01(value * 0.1f);
                bgm_audiosource.volume = audio_volume_.bgm_volume_ * Volume;
            }
            get
            {
                return audio_volume_.bgm_volume_;
            }
        }
        public float SEVolume
        {
            set
            {
                audio_volume_.se_volume_ = Mathf.Clamp01(value * 0.1f);
                se_volume = audio_volume_.se_volume_ * Volume;
            }
            get
            {
                return audio_volume_.se_volume_;
            }
        }

        public void Init()
        {
            // セーブデータができたら修正
            audio_volume_ = new AudioVolume();


            //オーディオファイルへのパスを抽出
            string directory_name = Path.GetFileName(AUDIO_DIRECTORY_PATH);
            var audio_path_dict = new Dictionary<string, string>();

            //用意したリストに全SE/BGMを入れる
            foreach (var audio_clip in Resources.LoadAll<AudioClip>(directory_name))
            {
                clip.Add(audio_clip);
            }

            audio_3D_prefab = Resources.Load<GameObject>("3DAudio");

            mixer = Resources.Load<AudioMixer>("AudioMix");
            mixer_master = mixer.FindMatchingGroups("Master")[0];
            mixer_se = mixer.FindMatchingGroups("SE")[0];
            mixer_bgm = mixer.FindMatchingGroups("BGM")[0];
        }

        void LateUpdate()
        {
            if (fade_out)
            {
                if(fade_in)
                    fade_in = false;
                
                mixer_bgm.audioMixer.GetFloat("BGM", out float i);

                mixer.SetFloat("BGM", i + ((bgm_mixer_min_volume - bgm_mixer_max_volume) / fade_time) * Time.deltaTime);

                if (i >= -bgm_mixer_max_volume)
                {
                    fade_out = false;
                }
            }

            if (fade_in)
            {
                mixer_bgm.audioMixer.GetFloat("BGM", out float i);

                mixer.SetFloat("BGM", i + ((bgm_mixer_max_volume - bgm_mixer_min_volume) / fade_time) * Time.deltaTime);

                if (i < -bgm_mixer_min_volume)
                {
                    bgm_audiosource.Stop();
                    fade_in = false;
                }

            }

            SortingList();

            foreach (AudioInfo info in sound_info_list)
            {
                {
                    info.source.PlayOneShot(info.clip, audio_volume_.volume_ * se_volume);
                }
            }

            //SEが再生しているかどうかのチェック
            List<AudioInfo> cheack_sound = new List<AudioInfo>();

            for (int i = 0; i < sound_info_list.Count; i++)
            {
                if (sound_info_list[i].source.isPlaying)
                {
                    cheack_sound.Add(sound_info_list[i]);
                }
            }

            //リスト更新
            sound_info_list = cheack_sound;
            cheack_sound.Clear();
        }

        /// <summary>
        /// 2DSE再生要請
        /// </summary>
        /// <param name="me">オーディオソースをアタッチしたいオブジェクト</param>
        /// <param name="se_name">再生させたいSE音源</param>
        public void Play2DSE(GameObject me, string se_name)
        {
            AudioSource audiosource = me.GetComponent<AudioSource>();

            AudioSourceSetting(me, ref audiosource,true);

            AudioInfo info = SearchSE(se_name, audiosource);

            ReservationSE(info);

        }

        /// <summary>
        /// 3DSE再生要請
        /// </summary>
        /// <param name="set_pos">3DAudioプレハブ生成位置</param>
        /// <param name="se_name">再生させたいSE音源</param>
        public void Play3DSE(Vector3 set_pos, string se_name)
        {
            GameObject audio_3d = Instantiate(audio_3D_prefab, set_pos, Quaternion.identity);

            AudioSource audiosource = audio_3d.GetComponent<AudioSource>();

            AudioInfo info = SearchSE(se_name, audiosource);

            info.obj = audio_3d;

            ReservationSE(info);

        }

        /// <summary>
        /// 一番最初に鳴らすBGMを再生
        /// BGM再生中にこの関数を呼ぶと上書きされます
        /// BGMは1つしか再生できないので停止や一時停止をする際は専用の関数使ってください
        /// </summary>
        /// <param name="me">オーディオソースをアタッチしたいオブジェクト</param>
        /// <param name="bgm_name"></param>
        public void PlayBGM(GameObject me, string bgm_name,float fade_time_)
        {
            fade_out = true;
            
            bgm_audiosource = me.GetComponent<AudioSource>();

            AudioSourceSetting(me, ref bgm_audiosource, false);

            bgm_audiosource.clip = SearchBGM(bgm_name);

            bgm_audiosource.volume = Volume * BGMVolume;

            fade_time = fade_time_;

            bgm_audiosource.Play();
            
        }

        /// <summary>
        /// BGM停止要請
        /// </summary>
        public void StopBGM(float fade_time_)
        {
            fade_time = fade_time_;
            fade_in = true;
        }

        /// <summary>
        /// BGM一時停止
        /// </summary>
        public void PauseBGM()
        {
            bgm_audiosource.Pause();
        }

        /// <summary>
        /// BGM再開
        /// </summary>
        public void UnPauseBGM()
        {
            bgm_audiosource.UnPause();
        }

        /// <summary>
        /// オーディオソースセティング
        /// </summary>
        /// <param name="me">オーディオソースをアタッチするオブジェクト</param>
        /// <param name="source">これから設定するオーディオソース</param>
        private void AudioSourceSetting(GameObject me, ref AudioSource source,bool sound_se)
        {
            if (source == null)
            {
                source = me.AddComponent<AudioSource>();
            }
            //2Dサウンド設定
            source.spatialBlend = 0;


            //起動時再生をfalseに
            source.playOnAwake = false;

            if (sound_se)
                source.outputAudioMixerGroup = mixer_se;

            else
            {
                source.outputAudioMixerGroup = mixer_bgm;

                source.priority = 0;

                source.loop = true;
            }
        }

        /// <summary>
        /// sound_info_listにスタック
        /// </summary>
        /// <param name="info"></param>
        private void SetList(AudioInfo info)
        {
            sound_info_list.Add(info);
            //play_queue.Enqueue(info);
        }


        /// <summary>
        /// 指定されたSEを検索して返す
        /// </summary>
        /// <param name="se_name">探したいSEの名前</param>
        /// <param name="source">オーディオソース</param>
        /// <returns></returns>
        private AudioInfo SearchSE(string se_name, AudioSource source)
        {
            if(se_name.IndexOf("SE")!=0)
            {
                Debug.LogError("指定した定数はSEではないので再生できません");
                return null;
            }

            string sename = se_name.Replace("SE/", "");

            foreach (var target_sound in clip)
            {
                if (target_sound.name == sename)
                {
                    AudioInfo info = new AudioInfo(source, target_sound, target_sound.length, MAX_SAME_COUNT, MAX_SAME_SE_COUNT);
                    return info;
                }
            }

            Debug.LogError("指定したBGMはヒットしませんでした");
            return null;
        }


        /// <summary>
        /// 指定されたBGMを検索して返す
        /// </summary>
        /// <param name="bgm_name">探したいBGMの名前</param>
        /// <returns></returns>
        private AudioClip SearchBGM(string bgm_name)
        {
            if(bgm_name.IndexOf("BGM")!=0)
            {
                Debug.LogError("指定した定数はBGMではないので再生できません");
                return null;
            }

            string bgmname = bgm_name.Replace("BGM/", "");

            foreach (var target_sound in clip)
            {
                if (target_sound.name == bgmname)
                {
                    return target_sound;
                }
            }


            Debug.LogError("指定したBGMはヒットしませんでした");
            return null;
        }

        /// <summary>
        /// SE再生の予約
        /// </summary>
        /// <param name="info"></param>
        private void ReservationSE(AudioInfo info)
        {
            //最大同時再生数より少なかったら再生
            if (sound_info_list.Count < MAX_SAME_SE_COUNT && SameSoundCheack(info))
            {
                SetList(info);
            }
            else
            {
                SetList(info);



                AudioSource se = sound_info_list[0].source;

                se.Stop();
            }
        }

        /// <summary>
        /// 連続で同じSEが再生されそうかチェック
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private bool SameSoundCheack(AudioInfo info)
        {
            /// <summary>
            /// 同じSEを数える
            /// </summary>
            int same_se_count = 0;

            for (int i = 0; i < sound_info_list.Count; i++)
            {
                if (info.clip.name == sound_info_list[i].clip.name)
                {
                    same_se_count++;
                }
            }

            if (same_se_count >= MAX_SAME_SE_COUNT)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// 同じフレームで重複したSEを削除
        /// </summary>
        private void SortingList()
        {
            AudioClip base_info = null;
            List<AudioInfo> leave_list = new List<AudioInfo>();
            int count = 0;

            leave_list = sound_info_list;
            count = sound_info_list.Count;

            for (int i = 0; i < count; i++)
            {
                base_info = sound_info_list[i].clip;
                for (int j = i + 1; j < count; j++)
                {
                    if (base_info.name == leave_list[1].clip.name)
                    {
                        leave_list.Remove(sound_info_list[1]);
                    }
                }
                count = leave_list.Count;
            }

            sound_info_list = leave_list;

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
        public GameObject obj = null;

        public AudioInfo(AudioSource source_, AudioClip clip_, float length_, int max_se_num_, int max_same_se_num_)
        {
            source = source_;
            clip = clip_;
            length = length_;
            max_se_num = max_se_num_;
            max_same_se_num = max_same_se_num_;
            obj = null;
        }
    }


}
