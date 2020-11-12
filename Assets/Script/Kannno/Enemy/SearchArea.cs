using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using FrontPerson.Constants;

namespace FrontPerson.Enemy.AI
{
    public class SearchArea : MonoBehaviour
    {
        [SerializeField]
        private SphereCollider searchArea = null;
        [SerializeField]
        private float searchAngle = 80f;
        [SerializeField]
        bool AreaDraw = true;

        private bool isFound_ = false;
        public bool IsFound { get { return isFound_; } }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == TagName.PLAYER)
            {
                // データ取得
                var playerPos = other.transform.position;
                var enemyPos = transform.position;
                playerPos.y = enemyPos.y = 1f; // 高さ調整
                var enemyFront = transform.forward;

                // 角度内かどうか
                var playerDirection = Vector3.Normalize(playerPos - enemyPos);
                var angle = Vector3.Angle(enemyFront, playerDirection);
                if(angle >= searchAngle)
                {
                    isFound_ = false;
                    return;
                }

                // 敵とプレイヤーの間に障害物があるか
                Ray ray = new Ray(enemyPos, playerDirection);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, searchArea.radius))
                {
                    Debug.DrawRay(ray.origin, ray.direction * searchArea.radius, Color.green, 0.0f, false);

                    // プレイヤーだったら発見
                    if (hit.transform.tag == TagName.PLAYER)
                    {
                        isFound_ = true;
                        return;
                    }

                    // 壁、障害物、ギミックだったら見つけてない
                    if (hit.transform.tag == TagName.UNTAGGED
                    //|| hit.transform.tag == TagName.WALL
                    //|| hit.transform.tag == TagName.GIMMICK
                    )
                    {
                        isFound_ = false;
                        return;
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == TagName.PLAYER)
                isFound_ = false;
        }

#if UNITY_EDITOR
        //　サーチする角度表示
        private void OnDrawGizmos()
        {
            if (AreaDraw)
            {
                Handles.color = new Color(1, 0, 0, 0.2f);
                Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius);
            }
        }
#endif
    }
}
