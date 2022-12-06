using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreenFlicker : MonoBehaviour
{
    private Light lightSource;

    void Start()
    {
        lightSource = transform.GetComponent<Light>();
    }

    [SerializeField] [Range(0f, .001f)] public float intensityChange;
    [SerializeField] [Range(0f, .01f)] public float rangeChange;
    public Color color;

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
            lightSource.intensity = 2;
            lightSource.range = 100;
            lightSource.color = color;
            sign *= -1;
        }
        lightSource.intensity += sign * intensityChange;
        lightSource.range += sign * rangeChange;
        lightSource.color = new Color(lightSource.color.r + sign * intensityChange / 10, lightSource.color.g - sign * intensityChange / 10, lightSource.color.b);

    }
}
