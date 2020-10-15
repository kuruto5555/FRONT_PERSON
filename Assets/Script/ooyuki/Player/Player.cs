
using UnityEngine;


namespace FrontPerson.Character
{


    public class Player : MonoBehaviour
    {
        [Header("移動速度")]
        [SerializeField, Range(0.1f, 10.0f)]
        float moveSpeed_ = 5.0f;

        [Header("視点感度")]
        [SerializeField, Range(1, 14)]
        int rotationSpeed_ = 7;

        [Header("体力")]
        [SerializeField, Range(1, 100)]
        int hp_ = 10;


        /// <summary>
        /// カメラのトランスフォーム
        /// </summary>
        Transform cameraTransform_ = null;

        /// <summary>
        /// 銃
        /// </summary>
        FrontPerson.Gun.Gun gun_ = null;


        /// <summary>
        /// 座標更新用
        /// </summary>
        Vector3 position_;


        /// <summary>
        /// 
        /// </summary>
        Vector3 Position { get { return position_; } }



        // Start is called before the first frame update
        void Start()
        {
            cameraTransform_ = Camera.main.transform;
            gun_ = GetComponentInChildren<FrontPerson.Gun.Gun>();

            position_ = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            Move();
            Shot();
            Reload();

            transform.position = position_;
        }

        private void Move()
        {
            Vector3 direction = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) direction += transform.forward;
            if (Input.GetKey(KeyCode.S)) direction -= transform.forward;
            if (Input.GetKey(KeyCode.A)) direction -= transform.right;
            if (Input.GetKey(KeyCode.D)) direction += transform.right;

            float X_Rotation = Input.GetAxis("Mouse X") * rotationSpeed_ * 30 * Time.deltaTime;
            float Y_Rotation = Input.GetAxis("Mouse Y") * rotationSpeed_ * 30 * Time.deltaTime;

            transform.Rotate(0, X_Rotation, 0);
            cameraTransform_.Rotate(-Y_Rotation, 0, 0);

            position_ += direction * moveSpeed_ * Time.deltaTime;
        }


        void Shot()
        {
            if (!Input.GetKey(KeyCode.Mouse0)) return;

            gun_.Shot();
        }

        void Reload()
        {
            if (!Input.GetKeyDown(KeyCode.R)) return;

            gun_.Reload();
        }

    }

};
