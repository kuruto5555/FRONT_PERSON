using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastShader : MonoBehaviour
{
    private ParticleSystem _particleSystem = null;

    private ParticleSystemRenderer _renderer = null;

    private float time = 0f;

    private float _timeValue;

    private float _duration = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _renderer = GetComponent<ParticleSystemRenderer>();

        _timeValue = 0.0f;

        _duration = _particleSystem.duration * 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        _renderer.sharedMaterial.SetFloat("Vector1_6A222455", time);

        time = Mathf.Lerp(0, 1, _timeValue);

        _timeValue += _duration * Time.deltaTime;
    }
}
