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

    [SerializeField] [Range(0f, .001f)] public float intensityChange;
    [SerializeField] [Range(0f, .01f)] public float rangeChange;
    public Color color1, color2;

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
            lightSource.color = color1;
            sign *= -1;
        }
        lightSource.intensity += sign * intensityChange;
        lightSource.range += sign * rangeChange;
        lightSource.color = new Color(lightSource.color.r + sign * intensityChange, lightSource.color.g, lightSource.color.b);

    }
}
