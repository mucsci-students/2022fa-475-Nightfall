using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float timeOfDay;

    public GameObject sun;
    public Material skyboxMaterial;
    public float timeScale, intensityScale;
    [SerializeField] [Range(0f, 1f)] float colorStep;
    public Color settingColor, noonColor;

    // Start is called before the first frame update
    void Start()
    {
        timeOfDay = 12;
        skyboxMaterial.SetFloat("_Exposure", 1);

        ToolData.Initialize();
        PlayerHandler.Initialize();
        Inventory.Initialize();
        UIManager.Initialize();

    }
    float t = 0;
    void Update()
    {
        timeOfDay += Time.deltaTime * timeScale;

        skyboxMaterial.SetFloat("_Rotation", skyboxMaterial.GetFloat("_Rotation") + .01f);

        if (timeOfDay > 7 && timeOfDay < 21)
        {
            sun.transform.SetLocalPositionAndRotation(sun.transform.position, new Quaternion(0.408217877f, (timeOfDay/6f ) - 2f, 0.109381631f, 0.875426114f));
        }
        if (timeOfDay > 6 && timeOfDay < 11)
        {           
            sun.GetComponent<Light>().intensity += Time.deltaTime * intensityScale;
            //sun.GetComponent<Light>().color = Color.Lerp(settingColor, noonColor, colorStep * Time.deltaTime);
            float newR = settingColor.r + ((noonColor.r - settingColor.r) * (timeOfDay - 6) / 5);
            float newG = settingColor.g + ((noonColor.g - settingColor.g) * (timeOfDay - 6) / 5);
            float newB = settingColor.b + ((noonColor.b - settingColor.b) * (timeOfDay - 6) / 5);
            sun.GetComponent<Light>().color = new Color(newR, newG, newB);
            skyboxMaterial.SetFloat("_Exposure", (timeOfDay - 5) / 5);
            float newValue = (timeOfDay - 6) / 10;
            RenderSettings.fogColor = new Color(newValue, newValue, newValue);
        }
        if (timeOfDay > 16 && timeOfDay < 21)
        {
            sun.GetComponent<Light>().intensity -= Time.deltaTime * intensityScale;
            //sun.GetComponent<Light>().color = Color.Lerp(noonColor, settingColor, colorStep * Time.deltaTime);
            float newR = noonColor.r - ((noonColor.r - settingColor.r) * (timeOfDay - 16) / 5);
            float newG = noonColor.g - ((noonColor.g - settingColor.g) * (timeOfDay - 16) / 5);
            float newB = noonColor.b - ((noonColor.b - settingColor.b) * (timeOfDay - 16) / 5);
            sun.GetComponent<Light>().color = new Color(newR, newG, newB);
            skyboxMaterial.SetFloat("_Exposure", 1.4f - ((timeOfDay - 15) / 5));
            float newValue = .5f - (timeOfDay - 16) / 10;
            RenderSettings.fogColor = new Color(newValue, newValue, newValue);
        }
        if (timeOfDay > 21 && timeOfDay < 7)
        {
            sun.GetComponent<Light>().intensity = 0;
        }
        if (timeOfDay > 24)
        {
            timeOfDay -= 24f;
        }

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
