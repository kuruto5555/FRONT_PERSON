using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnCanvas : MonoBehaviour
{
    private bool _isFire = false;
    private GameObject _launcher = null;
    // Start is called before the first frame update
    void Start()
    {
        _isFire = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFire)
        {
            if (transform.childCount == 0)
            {
                Destroy(gameObject);
            }
        }

        if(_launcher == null)
        {
            if (transform.childCount == 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void NowFire()
    {
        _isFire = true;
        
    }

    public void SetData(GameObject obj)
    {
        _launcher = obj;
    }
}
