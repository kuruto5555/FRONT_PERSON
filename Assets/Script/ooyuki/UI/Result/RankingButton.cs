using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FrontPerson.Manager;
using FrontPerson.Constants;


public class RankingButton : MonoBehaviour
{
    bool isCrick = false;


    public void OnCrick()
    {
        if (isCrick) return;

        GetComponent<Button>().enabled = false;
        AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_BACK);
        SceneManager.Instance.SceneChange(SceneName.RANKING_SCENE, 1f, Color.black);

        isCrick = true;
    }
}
