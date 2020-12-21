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

    ParticleSystemRenderer ins;

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
            var instanc = Instantiate(obj, transform.position, Quaternion.identity);

            //renderer.material = new Material();

            ins = instanc.GetComponent<ParticleSystemRenderer>();

            ins.material = renderer.sharedMaterial;

            flag = true;
        }

        if (flag)
        {
            ins.material.SetFloat("Vector1_6A222455", time);

            time = Mathf.Min(time + Time.deltaTime * 0.5f, 0.8f);

            if(0.8f == time)
            {
                time = 0f;

                flag = false;
            }
        }
    }
}
