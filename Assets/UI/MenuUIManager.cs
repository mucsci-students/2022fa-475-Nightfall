using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUIManager : MonoBehaviour
{
    private GameObject mainMenu;
    private GameObject options;
    private GameObject controls;
    private GameObject title;
    private GameObject selections;

    private GameObject musicToggle;
    private GameObject musicVol;
    private GameObject sfxToggle;
    private GameObject sfxVol;

    private AudioMixer mixer;

    private bool hasPreviousSave = false;

    void Start()
    {
        mainMenu = GameObject.Find("MenuScreen");
        options = GameObject.Find("MenuScreen/PauseMenu/Options");
        controls = GameObject.Find("MenuScreen/PauseMenu/Controls");
        title = GameObject.Find("MenuScreen/Title");
        selections = GameObject.Find("MenuScreen/Selections");

        mixer = Resources.Load("AudioMixer") as AudioMixer;

        musicToggle = GameObject.Find("MenuScreen/PauseMenu/Options/Music/Toggle");
        musicVol = GameObject.Find("MenuScreen/PauseMenu/Options/Music/Volume");
        sfxToggle = GameObject.Find("MenuScreen/PauseMenu/Options/SFX/Toggle");
        sfxVol = GameObject.Find("MenuScreen/PauseMenu/Options/SFX/Volume");

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

        musicVol.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
        sfxVol.GetComponent<Slider>().value = PlayerPrefs.GetFloat("SFXVolume");

        ToggleMusic();
        ToggleSFX();
        ChangeMusicVol();
        ChangeSFXVol();

        if (SaveEngine.HasSaveFileByName())
        {
            hasPreviousSave = true;
        }else
        {
            GameObject cont = GameObject.Find("MenuScreen/Selections/Continue");
            TextMeshProUGUI txt = GameObject.Find("MenuScreen/Selections/Continue/BtnText").GetComponent<TextMeshProUGUI>();
            Color col = txt.faceColor;
            txt.faceColor = new Color(col.r / 10, col.g / 10, col.b / 10);
            cont.GetComponent<Button>().interactable = false;
        }

        options.SetActive(false);
        controls.SetActive(false);
    }

    public void ToggleOptions()
    {
        title.SetActive(!title.activeSelf);
        selections.SetActive(title.activeSelf);

        options.SetActive(!options.activeSelf);
        controls.SetActive(options.activeSelf);
        if (!options.activeSelf)
        {
            PlayerPrefs.SetString("MusicToggle", musicToggle.GetComponent<Toggle>().isOn.ToString());
            PlayerPrefs.SetFloat("MusicVolume", musicVol.GetComponent<Slider>().value);
            PlayerPrefs.SetString("SFXToggle", sfxToggle.GetComponent<Toggle>().isOn.ToString());
            PlayerPrefs.SetFloat("SFXVolume", sfxVol.GetComponent<Slider>().value);
        }
    }

    public void ToggleMusic()
    {
        if (musicToggle.GetComponent<Toggle>().isOn)
        {
            mixer.SetFloat("MusicVolume", 80 * Mathf.Log10(musicVol.GetComponent<Slider>().value + .5f) - 10);
        }
        else
        {
                mixer.SetFloat("MusicVolume", -144f);
        }
    }
    public void ToggleSFX()
    {
        if (sfxToggle.GetComponent<Toggle>().isOn)
        {
            mixer.SetFloat("SFXVolume", 80 * Mathf.Log10(sfxVol.GetComponent<Slider>().value + .5f) - 10);
        }
        else
        {
            mixer.SetFloat("SFXVolume", -144f);
        }
    }
    public void ChangeMusicVol()
    {
        mixer.SetFloat("MusicVolume", 80 * Mathf.Log10(musicVol.GetComponent<Slider>().value + .5f) - 10);
    }
    public void ChangeSFXVol()
    {
        mixer.SetFloat("SFXVolume", 80 * Mathf.Log10(sfxVol.GetComponent<Slider>().value + .5f) - 10);
    }

    public void NewGame()
    {
        title.SetActive(false);
        selections.SetActive(false);

        SaveEngine.DeleteSaveFileByName();

        StartCoroutine(AsyncSceneLoad());
    }

    public void Continue()
    {
        SaveEngine.LoadSaveThenLoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator AsyncSceneLoad()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(1);

        while (!load.isDone)
        {
            yield return null;
        }
    }
}
