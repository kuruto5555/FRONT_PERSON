using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Manager;
using FrontPerson.Constants;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RankingButton : MonoBehaviour
{
    bool isCrick = false;

    [SerializeField]
    private Button button = null;

    public void OnCrick()
    {
        if (isCrick) return;

        EventSystem.current.enabled = false;
        AudioManager.Instance.Play2DSE(gameObject, SEPath.COMMON_SE_BACK);
        SceneManager.Instance.SceneChange(SceneName.RANKING_SCENE, 1f, Color.black);

        isCrick = true;

        button.enabled = false;
    }
}
