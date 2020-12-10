using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.common;

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

        public void Init()
        {
            // セーブデータができたら修正
            audio_volume_ = new AudioVolume();
        }
    }
}
