using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Item
{
    public enum SpecialItemType
    {

    }
    
    /// <summary>
    /// 獲得できるアイテム
    /// </summary>
    public class AcquirableItem : MonoBehaviour
    {
        public delegate void TakenItem();
    }
}
