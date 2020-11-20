using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson.Item
{
    /// <summary>
    /// 有効期間
    /// </summary>
    public class PickupDeadline : MonoBehaviour
    {
        /// <summary>
        /// アイテムが自然消滅するまでの時間
        /// </summary>
        private readonly float DEADLINE_DURATION = 30f;

        private Pickup pickup_;

        private void Start()
        {
            pickup_ = GetComponent<Pickup>();

            StartCoroutine(Expired());
        }

        private IEnumerator Expired()
        {
            float end_time = Time.time + DEADLINE_DURATION;

            while (Time.time < end_time)
            {
                yield return null;
            }

            pickup_.Destroyed();

        }
    }
}
