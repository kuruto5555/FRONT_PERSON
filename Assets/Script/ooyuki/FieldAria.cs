using FrontPerson.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FieldAria : MonoBehaviour
{
    BoxCollider aria_;
    float z_MIN = 0f;
    float z_MAX = 0f;
    float x_MIN = 0f;
    float x_MAX = 0f;

    private void Start()
    {
        aria_ = GetComponent<BoxCollider>();
        z_MAX = aria_.center.z + (aria_.size.z / 2);
        z_MIN = aria_.center.z - (aria_.size.z / 2);
        x_MAX = aria_.center.x + (aria_.size.x / 2);
        x_MAX = aria_.center.x - (aria_.size.x / 2);
    }

    private void OnTriggerExit(Collider other)
    {
        //if(other.tag == TagName.PLAYER)
        //{
        //    Vector3 pos = other.transform.position;
        //    other.transform.position = new Vector3(
        //        Mathf.Clamp(pos.x, x_MIN + other.)
        //        );
        //}
    }
}
