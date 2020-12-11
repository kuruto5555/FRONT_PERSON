using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField]
    private GameObject obj = null;

    private ParticleSystemRenderer renderer = null;

    private float time = 0f;

    private bool flag = false;

    // Start is called before the first frame update
    void Start()
    {
        renderer = obj.GetComponent<ParticleSystemRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            Instantiate(obj, transform.position, Quaternion.identity);

            flag = true;
        }

        if (flag)
        {
            renderer.sharedMaterial.SetFloat("Vector1_6A222455", time);

            time = Mathf.Min(time + Time.deltaTime * 0.5f, 0.8f);

            if(0.8f == time)
            {
                time = 0f;

                flag = false;
            }
        }
    }
}
