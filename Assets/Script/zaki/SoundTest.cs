using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Audio;

public class SoundTest : MonoBehaviour
{
    public GameObject a;
    public PlaySound s;

    // Start is called before the first frame update
    void Start()
    {
        a = GameObject.Find("TestGameObject");
        s = a.GetComponent < PlaySound>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            s.SoundPlay();
        }
    }
}
