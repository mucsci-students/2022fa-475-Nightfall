using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static Image healthBar;
    public static Image staminaBar;
    public static TextMeshProUGUI woodCount;
    public static TextMeshProUGUI stoneCount;

    public static Image[] tools = new Image[4];
    public static GameObject inventory;

    private static InputAction inventoryAction = new InputAction(binding: "<Keyboard>/tab");

    public static void Initialize()
    {
        healthBar = GameObject.Find("MainUI/StatusBar/Bars/Health").GetComponent<Image>();
        staminaBar = GameObject.Find("MainUI/StatusBar/Bars/Stamina").GetComponent<Image>();

        woodCount = GameObject.Find("MainUI/Inventory/WoodCount").GetComponent<TextMeshProUGUI>();
        stoneCount = GameObject.Find("MainUI/Inventory/StoneCount").GetComponent<TextMeshProUGUI>();

        inventory = GameObject.Find("MainUI/Inventory");
        inventory.SetActive(false);

        inventoryAction.performed += ctx =>
        {
            UpdateCount("Wood");
            UpdateCount("Stone");

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

    public static void UpdateCount(string field)
    {
        if (string.Equals(field, "Wood"))
        {
            woodCount.text = Inventory.GetCount(field).ToString();
        }
        else if (string.Equals(field, "Stone"))
        {
            stoneCount.text = Inventory.GetCount(field).ToString();
        }
    }
}
