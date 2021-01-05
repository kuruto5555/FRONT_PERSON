using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrontPerson.Weapon
{
    public class LockOnUI : MonoBehaviour
    {
        [Header("オフセット")]
        [SerializeField, Range(0.0f, 10.0f)] float Offset_ = 5.0f;

        private Transform _target = null;
        private RectTransform _rect;
        private Canvas _canvas = null;
        private RectTransform _canvasRect = null;
        private Character.Enemy _enemyData = null;

        Camera _targetCamera; // 映っているか判定するカメラへの参照

        private Image _ui = null;

        Vector4 _copyColor;

        private bool _isDead = false;

        // Start is called before the first frame update
        new void Start()
        {
            _targetCamera = Camera.main;
            
            _ui = GetComponent<Image>();

            _copyColor = new Vector4(_ui.color.r, _ui.color.g, _ui.color.b, _ui.color.a);

            _isDead = false;
        }

        // Update is called once per frame
        new void Update()
        {
            TargetLost();
            DisplayDraw();
        }

        private void LateUpdate()
        {
            if (_isDead) Destroy(gameObject);
        }

        private void TargetLost()
        {
            if (_enemyData.isDown == true) SetDead();
        }

        void DisplayDraw()
        {
            if (_target == null) return;

            var pos = new Vector3(_target.position.x, _target.position.y + Offset_, _target.position.z);

            _rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);

            Vector3 targetToCameraDirection_N = (_targetCamera.transform.position - _target.position).normalized;
            //正規化したベクトルの内積が一定以下なら見たことにする
            if (Vector3.Dot(targetToCameraDirection_N, _targetCamera.transform.forward.normalized) < -0.5)
            {
                _ui.color = _copyColor;
            }
            else
            {
                _ui.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            }

        }

        public void SetData(Transform pos, Canvas canvas)
        {
            _canvas = canvas;
            _canvasRect = _canvas.GetComponent<RectTransform>();
            _target = pos;
            _rect = GetComponent<RectTransform>();
            _rect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.position);
            _enemyData = _target.gameObject.GetComponent<Character.Enemy>();
        }

        public void SetDead()
        {
            _isDead = true;
        }
    }
}