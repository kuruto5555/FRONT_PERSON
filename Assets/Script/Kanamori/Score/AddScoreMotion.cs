using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FrontPerson.Manager;
using UnityEngine.UI;

namespace FrontPerson.Score
{
    /// <summary>
    /// 増減するスコアの動作
    /// </summary>
    public class AddScoreMotion : MonoBehaviour
    {
        private Animator anim_;

        /// <summary>
        /// モーションが終わった際に呼ぶイベント
        /// </summary>
        public UnityAction<int> end_motion_;

        // Start is called before the first frame update
        private void Start()
        {
            anim_ = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update()
        {
            // アニメーションが終了したかどうか
            if (1f < anim_.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                Destroy(this.gameObject);

                if (end_motion_!= null)
                {
                    // アニメーションが終了したのでイベントを呼ぶ
                    int score = int.Parse(GetComponent<Text>().text.ToString());
                    end_motion_.Invoke(score);
                }
            }
        }
    }
}
