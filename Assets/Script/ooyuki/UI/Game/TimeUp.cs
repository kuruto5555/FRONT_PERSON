using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.UI
{
    public class TimeUp : MonoBehaviour
    {
        public bool IsFinissh { get; private set; } = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Finish()
        {
            IsFinissh = true;
        }
    }
}
