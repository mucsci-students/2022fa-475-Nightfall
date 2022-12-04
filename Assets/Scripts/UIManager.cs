using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static Image healthBar;
    public static Image staminaBar;
    public static Dictionary<string, TextMeshProUGUI> counts = new Dictionary<string, TextMeshProUGUI>();

    public static Image[] tools = new Image[4];
    public static GameObject inventory;
    public static GameObject building;
    public static GameObject crafting;
    public static GameObject plr;

    private static InputAction inventoryAction = new InputAction(binding: "<Keyboard>/tab");

    public static void Initialize()
    {
        healthBar = GameObject.Find("MainUI/StatusBar/Bars/Health").GetComponent<Image>();
        staminaBar = GameObject.Find("MainUI/StatusBar/Bars/Stamina").GetComponent<Image>();

        counts.Add("Wood", GameObject.Find("MainUI/Inventory/WoodCount").GetComponent<TextMeshProUGUI>());
        counts.Add("Planks", GameObject.Find("MainUI/Inventory/PlanksCount").GetComponent<TextMeshProUGUI>());
        counts.Add("Stone", GameObject.Find("MainUI/Inventory/StoneCount").GetComponent<TextMeshProUGUI>());
        counts.Add("CutStone", GameObject.Find("MainUI/Inventory/CutStoneCount").GetComponent<TextMeshProUGUI>());
        counts.Add("CopperOre", GameObject.Find("MainUI/Inventory/CopperOreCount").GetComponent<TextMeshProUGUI>());
        counts.Add("CopperBar", GameObject.Find("MainUI/Inventory/CopperBarCount").GetComponent<TextMeshProUGUI>());
        counts.Add("IronOre", GameObject.Find("MainUI/Inventory/IronOreCount").GetComponent<TextMeshProUGUI>());
        counts.Add("IronBar", GameObject.Find("MainUI/Inventory/IronBarCount").GetComponent<TextMeshProUGUI>());

        inventory = GameObject.Find("MainUI/Inventory");
        inventory.SetActive(false);
        building = GameObject.Find("MainUI/Building");
        building.SetActive(false);
        crafting = GameObject.Find("MainUI/Crafting");
        crafting.SetActive(false);

        plr = GameObject.Find("Player");

        inventoryAction.performed += ctx =>
        {
            ExecuteEvents.Execute<ICustomMessenger>(plr, null, (x, y) => x.InventoryMenuMessage());

            UpdateCount("Wood");
            UpdateCount("Planks");
            UpdateCount("Stone");
            UpdateCount("CutStone");
            UpdateCount("CopperOre");
            UpdateCount("CopperBar");
            UpdateCount("IronOre");
            UpdateCount("IronBar");

            inventory.SetActive(!inventory.activeSelf);
            building.SetActive(inventory.activeSelf);
            crafting.SetActive(inventory.activeSelf);
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
        counts[field].text = Inventory.GetCount(field).ToString();
    }

    public static void ToggleBenderMode()
    {
        ExecuteEvents.Execute<ICustomMessenger>(plr, null, (x, y) => x.BenderModeMessage());
    }
}
