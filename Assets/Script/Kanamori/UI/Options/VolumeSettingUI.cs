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

            volume_slider_.value = AudioManager.Instance.Volume * 10;
            bgm_volume_slider_.value = AudioManager.Instance.BGMVolume * 10;
            se_volume_slider_.value = AudioManager.Instance.SEVolume * 10;

            volume_slider_.onValueChanged.AddListener((value) => { AudioManager.Instance.Volume = value; });
            bgm_volume_slider_.onValueChanged.AddListener((value) => { AudioManager.Instance.BGMVolume = value; });
            se_volume_slider_.onValueChanged.AddListener((value) => { AudioManager.Instance.SEVolume = value; });
        }
    }
}
