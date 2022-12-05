using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : SaveGameTrackable
{
    public float TimeOfDay { get; private set; }
    private int daysSurvived;
    private int currentStructures;

    public GameObject sun;
    public Material skyboxMaterial;
    public float timeScale, intensityScale, skyboxRotation;
    [SerializeField] [Range(0f, 1f)] float colorStep;
    public Color sunColor;

    // Start is called before the first frame update
    void Start()
    {
        TimeOfDay = 12;
        daysSurvived = 0;
        currentStructures = 0;

        skyboxMaterial.SetFloat("_Exposure", 1);

        ToolData.Initialize();
        PlayerHandler.Initialize();
        Inventory.Initialize();
        UIManager.Initialize();

    }
    float t = 0;
    void Update()
    {
        TimeOfDay += Time.deltaTime * timeScale;

        float rot = skyboxMaterial.GetFloat("_Rotation");
        if (rot >= 360)
        {
            rot -= 360;
        }
        skyboxMaterial.SetFloat("_Rotation", rot + skyboxRotation);

        float exposure = Mathf.Clamp(-.1f * Mathf.Pow(.5f * (TimeOfDay - 12f), 2) + 1.5f, .05f, 1f);
        skyboxMaterial.SetFloat("_Exposure", exposure);
        sun.transform.SetLocalPositionAndRotation(sun.transform.position, new Quaternion(0.4f, (TimeOfDay / 6f) - 2f, 0.1f, 0.87f));
        sun.GetComponent<Light>().color = sunColor * exposure;
        sun.GetComponent<Light>().intensity = exposure;
        RenderSettings.fogColor = Color.gray * exposure;
        RenderSettings.ambientIntensity = exposure;

        /*if (TimeOfDay > 7 && TimeOfDay < 21)
        {
            sun.transform.SetLocalPositionAndRotation(sun.transform.position, new Quaternion(0.408217877f, (TimeOfDay/6f ) - 2f, 0.109381631f, 0.875426114f));
        }
        if (TimeOfDay > 6 && TimeOfDay < 11)
        {           
            sun.GetComponent<Light>().intensity += Time.deltaTime * intensityScale;
            float newR = settingColor.r + ((noonColor.r - settingColor.r) * (TimeOfDay - 6) / 5);
            float newG = settingColor.g + ((noonColor.g - settingColor.g) * (TimeOfDay - 6) / 5);
            float newB = settingColor.b + ((noonColor.b - settingColor.b) * (TimeOfDay - 6) / 5);
            sun.GetComponent<Light>().color = new Color(newR, newG, newB);
            float newValue = (TimeOfDay - 6) / 10;
            RenderSettings.fogColor = new Color(newValue, newValue, newValue);
            //RenderSettings.ambientIntensity = ((TimeOfDay - 6) / 5); //* (intensityScale / 5);
        }
        if (TimeOfDay > 16 && TimeOfDay < 21)
        {
            sun.GetComponent<Light>().intensity -= Time.deltaTime * intensityScale;
            float newR = noonColor.r - ((noonColor.r - settingColor.r) * (TimeOfDay - 16) / 5);
            float newG = noonColor.g - ((noonColor.g - settingColor.g) * (TimeOfDay - 16) / 5);
            float newB = noonColor.b - ((noonColor.b - settingColor.b) * (TimeOfDay - 16) / 5);
            sun.GetComponent<Light>().color = new Color(newR, newG, newB);
            skyboxMaterial.SetFloat("_Exposure", 1.4f - ((TimeOfDay - 15) / 5));
            float newValue = .5f - (TimeOfDay - 16) / 10;
            RenderSettings.fogColor = new Color(newValue, newValue, newValue);
            //RenderSettings.ambientIntensity -= Time.deltaTime * (intensityScale / 5);
        }
        if (TimeOfDay > 21 && TimeOfDay < 7)
        {
            sun.GetComponent<Light>().intensity = 0;
        }*/
        if (TimeOfDay > 24)
        {
            ++daysSurvived;
            TimeOfDay -= 24f;
        }

        t += Time.deltaTime;
        if (t > 1)
        {
            PlayerHandler.AddValue("health", 1);
            t = 0;
        }
    }

    public void AddStructure()
    {
        ++currentStructures;
    }

    public int GetDaysSurvived()
    {
        return daysSurvived;
    }

    public float GetTimeOfDay()
    {
        return TimeOfDay;
    }

    public int GetCurrentStructures()
    {
        return currentStructures;
    }

    public override SaveRecord GenerateSaveRecord() => new GameManagerSaveRecord(
        gameObject.name, 
        TimeOfDay, 
        sun.GetComponent<Light>().intensity, 
        skyboxMaterial.GetFloat("_Exposure"), 
        t, 
        daysSurvived, 
        currentStructures,
        sun.GetComponent<Light>().color,
        RenderSettings.fogColor);

    public override void RestoreFromSaveRecord(string recordJson)
    {

        GameManagerSaveRecord record = ObjectExtensions.FromJson<GameManagerSaveRecord>(recordJson);
        Vector3 pulledLightColor = record.LightColor.Value;
        Vector3 pulledFogColor = record.FogColor.Value;

        daysSurvived = record.DaysSurvived;
        currentStructures = record.CurrentStructures;
        TimeOfDay = record.TimeOfDay;
        sun.GetComponent<Light>().intensity = record.SunIntensity;
        skyboxMaterial.SetFloat("_Exposure", record.Exposure);
        t = record.t;        
        sun.GetComponent<Light>().color = new Color(pulledLightColor.x, pulledLightColor.y, pulledLightColor.z);
        RenderSettings.fogColor = new Color(pulledFogColor.x, pulledFogColor.y, pulledFogColor.z);

    }
}
