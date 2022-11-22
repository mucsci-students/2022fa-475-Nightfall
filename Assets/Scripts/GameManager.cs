using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float lightMultiplier = 2;

    private float timeOfDay;
    private Light sun;

    // Start is called before the first frame update
    void Start()
    {
        timeOfDay = 0;

        sun = GameObject.Find("Directional Light").GetComponent<Light>();

        ToolData.Initialize();
        PlayerHandler.Initialize();
        Inventory.Initialize();
        UIManager.Initialize();

    }
    float t = 0;
    void Update()
    {
        t += Time.deltaTime;
        if (t > 1)
        {
            PlayerHandler.AddValue("health", 1);
            t = 0;
        }

        timeOfDay += Time.deltaTime;
        if (timeOfDay >= 1)
        {
            timeOfDay -= 1;
        }
        if (timeOfDay < .5)
        {
            sun.intensity -= Time.deltaTime * lightMultiplier;
        }
        else
        {
            sun.intensity += Time.deltaTime * lightMultiplier;
        }
        sun.transform.Rotate(new Vector3(timeOfDay, sun.transform.rotation.y, sun.transform.rotation.z));
    }

    public void UpdateValue(string field)
    {

    }
}
