using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using FrontPerson.Manager;

namespace FrontPerson.UI
{
    public class Ranking : MonoBehaviour
    {
        [Header("スコアのテキスト")]
        [SerializeField]
        private List<Text> ScoreText_List = null;

        [Header("コンボのテキスト")]
        [SerializeField]
        private List<Text> ComboText_List = null;

        void Start()
        {
            if(null != ScoreText_List && null != ComboText_List)
            {
                var manager = GameObject.FindGameObjectWithTag(Constants.TagName.MANAGER).GetComponent<ApplicationManager>();

                var scores = manager.save_data_.RankingScore;

                for (int i = 0; i < scores.Count; i++)
                {
                    ScoreText_List[i].text = scores[i].ToString();
                }

                var combos = manager.save_data_.RankingComboNum;

                for (int i = 0; i < combos.Count; i++)
                {
                    ComboText_List[i].text = combos[i].ToString();
                }
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError("ScoreText_List or ComboText_List が null です");
            }
#endif
        }
    }
}