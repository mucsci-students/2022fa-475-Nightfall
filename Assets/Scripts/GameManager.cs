using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float timeOfDay;

    // Start is called before the first frame update
    void Start()
    {
        timeOfDay = 0;

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
    }

    public void UpdateValue(string field)
    {

    }
}
