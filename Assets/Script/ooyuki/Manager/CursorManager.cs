using FrontPerson.common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Manager
{
    public class CursorManager : SingletonMonoBehaviour<CursorManager>
    {
        public void CursorLock()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }


        public void CurcorUnlock()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
