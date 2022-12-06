using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterWater : MonoBehaviour
{
    public FogEffect fogEffect;
    public UnderWaterEffect underwater;

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "DetectWater")
        {
            fogEffect.enabled = true;
            underwater.enabled = true;
        }
    }

    void OnTriggerExit(Collider collision) 
    {
        if(collision.gameObject.name == "DetectWater")
        {
            fogEffect.enabled = false;
            underwater.enabled = false;
        }
    }
}
