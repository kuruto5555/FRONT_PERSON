using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoge : MonoBehaviour
{
    [SerializeField]
    private GameObject obj = null;

    private ParticleSystemRenderer renderer = null;

    private float time = 0f;
    ParticleSystemRenderer ins;

    private ParticleSystem _particleSystem = null;

    void Start()
    {
        renderer = obj.GetComponent<ParticleSystemRenderer>();

        var instanc = Instantiate(obj, transform.position, Quaternion.identity);

        ins = instanc.GetComponent<ParticleSystemRenderer>();

        ins.material = renderer.sharedMaterial;

        _particleSystem = instanc.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        ins.material.SetFloat("Vector1_6A222455", time);

        time = Mathf.Min(time + Time.deltaTime * 0.5f, 1f);

        if (!_particleSystem.isPlaying) Destroy(gameObject);
    }
}
