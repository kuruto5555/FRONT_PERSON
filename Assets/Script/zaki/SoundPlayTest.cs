using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FrontPerson.Audio
{
    public class SoundPlayTest : MonoBehaviour
    {
        private GameObject a = null;
        private PlaySound b = null;

        // Start is called before the first frame update
        void Start()
        {
            a = Resources.Load("SE") as GameObject;
            b = a.GetComponent<PlaySound>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                b.SoundPlay("Common_SE_Back1");
            }
        }
    }
}