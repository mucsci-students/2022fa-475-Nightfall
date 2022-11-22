using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static Image healthBar;
    public static Image staminaBar;
    public static Image[] tools = new Image[4];
    public static GameObject inventory;

    private static InputAction inventoryAction = new InputAction(binding: "<Keyboard>/tab");

    public static void Initialize()
    {
        healthBar = GameObject.Find("MainUI/StatusBar/Bars/Health").GetComponent<Image>();
        staminaBar = GameObject.Find("MainUI/StatusBar/Bars/Stamina").GetComponent<Image>();

        inventory = GameObject.Find("MainUI/Inventory");
        inventory.SetActive(false);

        inventoryAction.performed += ctx =>
        {
            inventory.SetActive(!inventory.activeSelf);
        };

        inventoryAction.Enable();
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
