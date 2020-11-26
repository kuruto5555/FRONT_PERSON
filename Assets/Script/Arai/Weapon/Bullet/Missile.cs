using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 追尾処理
    /// </summary>
    private void Homing()
    {
       // float step = Time.deltaTime * HomingSpeed_;
       //
       // transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, step);
       //
       // var rot = transform.rotation;
       //
       // transform.LookAt(_player.transform);
    }
}
