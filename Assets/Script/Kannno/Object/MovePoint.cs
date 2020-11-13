using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FrontPerson.Enemy
{
    public class MovePoint : MonoBehaviour
    {
        private Vector3 point_;
        public Vector3 Point { get { return point_; } }

        public void Awake()
        {
            point_ = transform.position;
        }

#if UNITY_EDITOR
        // 場所がわかるようにGizmo描画
        private void OnDrawGizmos()
        {
            Handles.color = Color.green;
            Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, 0f, 0f) * transform.forward, 360f, 0.5f);
        }
#endif
    }
}