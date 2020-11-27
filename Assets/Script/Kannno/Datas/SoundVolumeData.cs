using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Data
{
    [System.Serializable]
    public class SoundVolumeData
    {
        public SoundVolumeData()
        {
            volume_ = 1f;
            bgm_volume_ = 1f;
            se_volume_ = 1f;
        }
        public float volume_;
        public float bgm_volume_;
        public float se_volume_;
    }
}