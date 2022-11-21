using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static Image healthBar;
    public static Image staminaBar;
    public static Image[] tools = new Image[4];
    public static Image inventory;
    
    public static void Initialize()
    {
        healthBar = GameObject.Find("MainUI/StatusBar/Bars/Health").GetComponent<Image>();
        staminaBar = GameObject.Find("MainUI/StatusBar/Bars/Stamina").GetComponent<Image>();
    }

    public static void UpdateValue(string field, int v, int maxV)
    {
        if (string.Equals(field, "health"))
        {
            healthBar.fillAmount = (float)v / (float)maxV;
        }
        else if (string.Equals(field, "stamina"))
        {
            staminaBar.fillAmount = (float)v / (float)maxV;
        }
    }
}
