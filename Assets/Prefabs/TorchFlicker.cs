using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TorchFlicker : MonoBehaviour
{
    public GameObject torchLight;
    public GameObject flame;

    private Light lightSource;

    void Start()
    {
        lightSource = torchLight.GetComponent<Light>();
    }

    // Update is called once per frame
    private float t = 0;
    private float duration;
    private float sign = -1f;
    void Update()
    {
        if (t <= 0)
        {
            duration = Random.Range(.2f, .4f);
        }

        t += Time.deltaTime;

        if (t >= duration)
        {
            t = 0;
            lightSource.intensity = 1;
            lightSource.range = 10;
            sign *= -1;
        }
        lightSource.intensity += sign * .01f;
        lightSource.range += sign * .1f;

    }
}
