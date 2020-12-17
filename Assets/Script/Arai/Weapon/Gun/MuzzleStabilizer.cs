using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrontPerson.Constants;

public class MuzzleStabilizer : MonoBehaviour
{
    private Transform _cameraTransform = null;

    private Vector3 _centerPoint;

    private int _layerMask = 1 << LayerNumber.ENEMY | 1 << LayerNumber.FIELD_OBJECT;

    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = Camera.main.transform;
        _centerPoint = transform.position + _cameraTransform.forward * 50.0f;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out hit, 50.0f, _layerMask))
        {
            transform.LookAt(hit.point);
        }
        else
        {
            _centerPoint = transform.position + _cameraTransform.forward * 50.0f;
            transform.LookAt(_centerPoint);
        }
        
    }
}
