using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Constants
{
    /// <summary>
    /// プレイヤーでアイテム管理用フラグ
    /// </summary>
    public enum ITEM_STATUS
    {
        NORMAL = 0,             //  0000    初期状態
        INVICIBLE = 1,          //  0001    透明化
        SPEED_UP = 2,           //  0010    速度上昇
        FEVER = 4,              //  0100    フィーバ
        COMBO_ADDITION = 8,     //  1000    コンボ加算
        COMBO_INSURANCE = 16    //1 0000    コンボ保険
        
    }
}