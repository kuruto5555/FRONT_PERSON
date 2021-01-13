using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FrontPerson.UI
{
    public class Tutorial_Icon : MonoBehaviour
    {
        public int index_ { get; private set; } = 0;

        private Tutorial tutorial_ = null;

        public void Init(Tutorial tutorial, int index)
        {
            index_ = index;
            tutorial_ = tutorial;
        }


        public void OnClick()
        {
            tutorial_.IconClick(index_);
        }
    }
}
