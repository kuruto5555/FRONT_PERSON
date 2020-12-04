using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FrontPerson
{
    public class TitleCamera : MonoBehaviour
    {
        [SerializeField]
        private float Speed = 0f;

        private Camera camera = null;

        void Start()
        {
            camera = GameObject.FindGameObjectWithTag(Constants.TagName.MAIN_CAMERA).GetComponent<Camera>();
        }

        void Update()
        {
            float y = camera.transform.rotation.eulerAngles.y;

            camera.transform.rotation = Quaternion.Euler(0f, y + Speed * Time.deltaTime, 0f);
        }
    }
}