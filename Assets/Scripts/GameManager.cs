using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : SaveGameTrackable
{
    public float TimeOfDay { get; private set; }
    private int daysSurvived;
    private int currentStructures;
    private AudioSource bgMusic;

    public GameObject sun;
    public Material skyboxMaterial;
    public float timeScale, intensityScale, skyboxRotation;
    public AudioClip dayMusic, nightMusic;
    [SerializeField] [Range(0f, 1f)] float colorStep;
    public Color sunColor;

    // Start is called before the first frame update
    void Start()
    {
        TimeOfDay = 12;
        daysSurvived = 0;
        currentStructures = 0;

        bgMusic = GameObject.Find("Player/CameraHolder").GetComponent<AudioSource>();

        skyboxMaterial.SetFloat("_Exposure", 1);
        RenderSettings.ambientIntensity = 1;

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

        if (TimeOfDay > 20 && TimeOfDay < 21 && bgMusic.clip == dayMusic)
        {
            bgMusic.clip = nightMusic;
            bgMusic.Play();
        }
        if (TimeOfDay > 6 && TimeOfDay < 7 && bgMusic.clip == nightMusic)
        {
            bgMusic.clip = dayMusic;
            bgMusic.Play();
        }

        if (TimeOfDay > 24)
        {
            ++daysSurvived;
            TimeOfDay -= 24f;
            ResourcePool.SharedInstance.RespawnResources();
            GameObject.Find("GameManager").GetComponent<EnemyHandler>().UpdateMaxEnemies(daysSurvived);
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

    public override SaveRecord GenerateSaveRecord() => new GameManagerSaveRecord(gameObject.name, TimeOfDay);

    public override void RestoreFromSaveRecord(string recordJson)
    {

        GameManagerSaveRecord record = ObjectExtensions.FromJson<GameManagerSaveRecord>(recordJson);        
        TimeOfDay = record.TimeOfDay;
        
    }
}
