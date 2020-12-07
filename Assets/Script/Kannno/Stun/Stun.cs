using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrontPerson
{
    public class Stun : MonoBehaviour
    {
        [Header("回るスターのリスト")]
        [SerializeField]
        private List<GameObject> Stars = new List<GameObject>();

        [Header("全体の回転する速度")]
        [SerializeField]
        private float Speed = 0f;

        [Header("星自身が回転する速度")]
        [SerializeField]
        private float StarSpeed = 0f;

        private Transform Transform = null;

        void Start()
        {
            Transform = gameObject.transform;
        }

        void Update()
        {
            Transform.rotation *= Quaternion.Euler(0f, Speed * Time.deltaTime, 0f);

            foreach (var star in Stars)
            {
                star.transform.rotation *= Quaternion.Euler(0f, StarSpeed * Time.deltaTime, 0f);
            }
        }
    }
}