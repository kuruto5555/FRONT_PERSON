using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FrontPerson.UI {
public class VitaminManager : MonoBehaviour
{
    [Header("ビタミンのゲージ")]
    [SerializeField]
    List<Image> vitamins_ = null;

    [Header("プレイヤー")]
    [SerializeField]
    Character.Player player_ = null;

    // Start is called before the first frame update
    void Start()
    {
        // 
        foreach(var vitamin in vitamins_)
        {
            vitamin.rectTransform.localScale = new Vector2(0, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        vitamins_[0].rectTransform.localScale = new Vector2(player_.GunAmmoL * 0.01f, 1.0f);
        vitamins_[1].rectTransform.localScale = new Vector2(player_.GunAmmoR * 0.01f, 1.0f);
    }
}

}
