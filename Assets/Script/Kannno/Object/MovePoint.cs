using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace FrontPerson.Enemy
{
    public class MovePoint : MonoBehaviour
    {
        /// <summary>
        /// ルートの順番
        /// </summary>
        public int Index { get; private set; }

        private Vector3 point_;
        public Vector3 Point { get { return point_; } }

        private void Awake()
        {
#if UNITY_EDITOR
            if(false == Regex.IsMatch(gameObject.name, @"[^0-9]"))
            {
                Debug.LogError("MovePoint の name 内に番号が無いです");
            }
#endif
            int str = int.Parse(Regex.Replace(gameObject.name, @"[^0-9]", ""));

            Index = str;

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