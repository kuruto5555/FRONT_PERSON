using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FrontPerson
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

        // Update is called once per frame
        void Update()
        {

        }


        public void SoundPlay()
        {
            //
            if (audio == null)
            {
                audio = gameObject.AddComponent<AudioSource>();
            }

        }
    }
}