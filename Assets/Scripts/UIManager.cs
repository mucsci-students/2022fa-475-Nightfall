using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static Image healthBar;
    public static Image staminaBar;
    public static Dictionary<string, TextMeshProUGUI> counts = new Dictionary<string, TextMeshProUGUI>();

    public static Image[] tools = new Image[4];
    public static GameObject inventory;
    public static GameObject building;
    public static GameObject crafting;
    public static GameObject controls;
    public static GameObject options;
    public static GameObject benderModeText;
    public static GameObject plr;
    public static GameObject musicSlider;
    public static GameObject musicToggle;
    public static GameObject sfxSlider;
    public static GameObject sfxToggle;
    public static GameObject msgText;

    public static AudioMixer mixer;

    private static InputAction inventoryAction = new InputAction(binding: "<Keyboard>/tab");
    private static InputAction pauseAction = new InputAction(binding: "<Keyboard>/escape");

    public static void Initialize()
    {
        mixer = Resources.Load("AudioMixer") as AudioMixer;

        musicSlider = GameObject.Find("PauseMenu/Options/Music/Volume");
        musicToggle = GameObject.Find("PauseMenu/Options/Music/Toggle");
        sfxSlider = GameObject.Find("PauseMenu/Options/SFX/Volume");
        sfxToggle = GameObject.Find("PauseMenu/Options/SFX/Toggle");
        msgText = GameObject.Find("MainUI/MsgText");

        healthBar = GameObject.Find("MainUI/StatusBar/Bars/Health").GetComponent<Image>();
        staminaBar = GameObject.Find("MainUI/StatusBar/Bars/Stamina").GetComponent<Image>();

        counts.Add("Wood", GameObject.Find("MainUI/Inventory/WoodCount").GetComponent<TextMeshProUGUI>());
        counts.Add("Planks", GameObject.Find("MainUI/Inventory/PlanksCount").GetComponent<TextMeshProUGUI>());
        counts.Add("Stone", GameObject.Find("MainUI/Inventory/StoneCount").GetComponent<TextMeshProUGUI>());
        counts.Add("CutStone", GameObject.Find("MainUI/Inventory/CutStoneCount").GetComponent<TextMeshProUGUI>());
        counts.Add("Copper Ore", GameObject.Find("MainUI/Inventory/CopperOreCount").GetComponent<TextMeshProUGUI>());
        counts.Add("Copper Bar", GameObject.Find("MainUI/Inventory/CopperBarCount").GetComponent<TextMeshProUGUI>());
        counts.Add("Iron Ore", GameObject.Find("MainUI/Inventory/IronOreCount").GetComponent<TextMeshProUGUI>());
        counts.Add("Iron Bar", GameObject.Find("MainUI/Inventory/IronBarCount").GetComponent<TextMeshProUGUI>());

        inventory = GameObject.Find("MainUI/Inventory");
        inventory.SetActive(false);
        building = GameObject.Find("MainUI/Building");
        building.SetActive(false);
        crafting = GameObject.Find("MainUI/Crafting");
        crafting.SetActive(false);
        controls = GameObject.Find("PauseMenu/Controls");
        controls.SetActive(false);
        options = GameObject.Find("PauseMenu/Options");
        options.SetActive(false);
        benderModeText = GameObject.Find("PauseMenu/BenderModeText");
        benderModeText.SetActive(false);

        plr = GameObject.Find("Player");

        bool val = false;
        if (PlayerPrefs.GetString("MusicToggle") == "True")
        {
            val = true;
        }
        musicToggle.GetComponent<Toggle>().isOn = val;
        val = false;
        if (PlayerPrefs.GetString("SFXToggle") == "True")
        {
            val = true;
        }
        sfxToggle.GetComponent<Toggle>().isOn = val;

        musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("SFXVolume");

        ToggleMusic();
        ToggleSFX();
        ChangeMusicVol();
        ChangeSFXVol();

        options.SetActive(false);
        msgText.SetActive(false);

        inventoryAction.performed += ctx =>
        {
            if (controls.activeSelf) { return; }

            ExecuteEvents.Execute<ICustomMessenger>(plr, null, (x, y) => x.ToggleMenuMessage());

            UpdateCount("Wood");
            UpdateCount("Planks");
            UpdateCount("Stone");
            UpdateCount("CutStone");
            UpdateCount("Copper Ore");
            UpdateCount("Copper Bar");
            UpdateCount("Iron Ore");
            UpdateCount("Iron Bar");

            inventory.SetActive(!inventory.activeSelf);
            building.SetActive(inventory.activeSelf);
            crafting.SetActive(inventory.activeSelf);
            ToggleMsgText("Click the material icon in the Crafting or Building\n boxes to make it.");
        };
        pauseAction.performed += ctx =>
        {
            if (inventory.activeSelf)
            {
                inventory.SetActive(false);
                building.SetActive(false);
                crafting.SetActive(false);
            }
            else
            {
                ExecuteEvents.Execute<ICustomMessenger>(plr, null, (x, y) => x.ToggleMenuMessage());
            }

            controls.SetActive(!controls.activeSelf);
            options.SetActive(controls.activeSelf);

            if (controls.activeSelf)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        };

        inventoryAction.Enable();
        pauseAction.Enable();
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
        benderModeText.SetActive(!benderModeText.activeSelf);
        if (benderModeText.activeSelf)
        {
            Time.timeScale = 1;
            controls.SetActive(false);
            options.SetActive(false);
        }
    }

    public static void ToggleMsgText(string msg)
    {
        if (msg.Length != 0)
        {
            msgText.GetComponent<TextMeshProUGUI>().text = msg;
        }

        msgText.SetActive(!msgText.activeSelf);
    }

    public static void ToggleMusic()
    {
        if (musicToggle.GetComponent<Toggle>().isOn)
        {
            mixer.SetFloat("MusicVolume", 80 * Mathf.Log10(musicSlider.GetComponent<Slider>().value + .5f) - 10);
        }
        else
        {
            mixer.SetFloat("MusicVolume", -144f);
        }
    }
    public static void ToggleSFX()
    {
        if (sfxToggle.GetComponent<Toggle>().isOn)
        {
            mixer.SetFloat("SFXVolume", 80 * Mathf.Log10(sfxSlider.GetComponent<Slider>().value + .5f) - 10);
        }
        else
        {
            mixer.SetFloat("SFXVolume", -144f);
        }
    }
    public static void ChangeMusicVol()
    {
        mixer.SetFloat("MusicVolume", 80 * Mathf.Log10(musicSlider.GetComponent<Slider>().value + .5f) - 10);
    }
    public static void ChangeSFXVol()
    {
        mixer.SetFloat("SFXVolume", 80 * Mathf.Log10(sfxSlider.GetComponent<Slider>().value + .5f) - 10);
    }

    public static void ExitToMenu()
    {
        SaveEngine.SaveGame();


        Time.timeScale = 1;

        SceneManager.LoadScene(0);
    }

    public static void QuitGame()
    {
        SaveEngine.SaveGame();
        Application.Quit();
    }
}
