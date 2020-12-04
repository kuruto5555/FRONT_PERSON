using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;

namespace FrontPerson.Audio
{
    public class PlaySound : MonoBehaviour
    { 
        //オーディオソース
        private AudioSource audiosorce = null;

        //オーディオクリップ
        [SerializeField] List<AudioClip> audioclip = new List<AudioClip>();

        // Start is called before the first frame update
        void Start()
        {
            audiosorce = GetComponent<AudioSource>();
        }

        public void SoundPlay(int num)
        {
            audiosorce.PlayOneShot(audioclip[num], 1.0f * AudioManager.Instance.audio_volume_.SEVolume);
        }

    }
}