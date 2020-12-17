using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastShader : MonoBehaviour
{
    [SerializeField]
    private GameObject obj = null;

    private ParticleSystemRenderer renderer = null;

    private ParticleSystemRenderer _renderer2 = null;

    private float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //renderer = obj.GetComponent<ParticleSystemRenderer>();
        _renderer2 = GetComponent<ParticleSystemRenderer>();

        //_renderer2.material = renderer.sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        _renderer2.sharedMaterial.SetFloat("Vector1_6A222455", time);

        time = Mathf.Min(time + Time.deltaTime * 0.5f, 1.0f);
    }
}
