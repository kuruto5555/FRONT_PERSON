using FrontPerson.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FrontPerson.UI
{
    public class VolumeSettingUI : MonoBehaviour
    {
        [SerializeField]
        private Slider volume_slider_ = null;
        [SerializeField]
        private Slider bgm_volume_slider_ = null;
        [SerializeField]
        private Slider se_volume_slider_ = null;

        // Start is called before the first frame update
        void Start()
        {
            if(!volume_slider_ ||
                !bgm_volume_slider_ ||
                !se_volume_slider_
                )
            {
                Debug.LogError("音量設定のスライダーが設定されてません。");
            }

            volume_slider_.value = AudioManager.Instance.audio_volume_.Volume;
            bgm_volume_slider_.value = AudioManager.Instance.audio_volume_.BGMVolume;
            se_volume_slider_.value = AudioManager.Instance.audio_volume_.SEVolume;

            volume_slider_.onValueChanged.AddListener((value) => { AudioManager.Instance.audio_volume_.Volume = value; });
            bgm_volume_slider_.onValueChanged.AddListener((value) => { AudioManager.Instance.audio_volume_.BGMVolume = value; });
            se_volume_slider_.onValueChanged.AddListener((value) => { AudioManager.Instance.audio_volume_.SEVolume = value; });
        }
    }
}
