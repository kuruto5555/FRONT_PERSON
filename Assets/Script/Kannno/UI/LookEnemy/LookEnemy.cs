using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using FrontPerson.Character;

namespace FrontPerson.UI
{
    public class LookEnemy : MonoBehaviour
    {
        [SerializeField]
        private List<Image> Images = new List<Image>();

        private Player Player = null;

        private List<Transform> EnemiesTransform = new List<Transform>();

        void Start()
        {
            Player = GameObject.FindGameObjectWithTag(Constants.TagName.PLAYER).GetComponent<Player>();

#if UNITY_EDITOR
            if(0 == Images.Count)
            {
                Debug.Log("Images が設定されていません");
            }
#endif

            foreach(var image in Images)
            {
                image.gameObject.SetActive(false);
            }
        }

        private void LateUpdate()
        {
            if(Player.isAlart)
            {
                Look();
            }
            else
            {
                for (int i = 0; i < Images.Count; i++)
                {
                    Images[i].gameObject.SetActive(false);
                }
            }
        }

        private void Look()
        {
            Vector3 player_pos = Player.transform.position;

            Vector3 front = Player.transform.rotation * Vector3.forward;
            front.y = 0f;

            for (int i = 0; i < Images.Count; i++)
            {
                Images[i].gameObject.SetActive(false);
            }

            foreach (var et in EnemiesTransform)
            {
                Vector3 vec = (et.position - player_pos).normalized;
                vec.y = 0f;

                Vector3 axis = Vector3.Cross(front, vec);

                float angle = Vector3.Angle(front, vec) * (axis.y < 0f ? -1f : 1f);

                // 正面に敵がいる
                if (-60f <= angle && angle <= 60f)
                {
                    Images[0].gameObject.SetActive(true);
                    continue;
                }

                // 右側に敵がいる
                if (60f < angle && angle <= 140f)
                {
                    Images[1].gameObject.SetActive(true);
                    continue;
                }

                //背面に敵がいる
                if (-180f <= angle && angle < -140f || 140f < angle && angle <= 180f)
                {
                    Images[2].gameObject.SetActive(true);
                    continue;
                }

                // 右側に敵がいる
                if (-140f <= angle && angle < -60f)
                {
                    Images[3].gameObject.SetActive(true);
                    continue;
                }
            }
        }

        public void AddEnemy(Transform transform)
        {
            if(false == EnemiesTransform.Contains(transform))
            {
                EnemiesTransform.Add(transform);
            }
        }

        public void DeleteEnemy(Transform transform)
        {
            if (EnemiesTransform.Contains(transform))
            {
                EnemiesTransform.Remove(transform);
            }
        }
    }
}