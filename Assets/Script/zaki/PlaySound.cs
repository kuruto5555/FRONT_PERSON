using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;

namespace FrontPerson.Audio
{
    public class PlaySound : MonoBehaviour
    { 
        //オーディオソース
        private AudioSource audio = null;

        // Start is called before the first frame update
        void Start()
        {
            audio = GetComponent<AudioSource>();
        }

        public void SoundPlay()
        {
            audio.PlayOneShot(audio.clip, 1.0f * AudioManager.Instance.audio_volume_.SEVolume);
        }

    }
}