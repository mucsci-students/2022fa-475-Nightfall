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

    //private int interpolationFramesCount; // Number of frames to completely interpolate between the 2 positions
   //private int elapsedFrames = 0;

    // Start is called before the first frame update
    void Start()
    {
        timeOfDay = 12;
        //interpolationFramesCount = 45 * (int)(1/timeScale) * 5;
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
        //float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;
        Debug.Log("Time: " + timeOfDay);

        if (timeOfDay > 7 && timeOfDay < 21)
        {
            sun.transform.SetLocalPositionAndRotation(sun.transform.position, new Quaternion(0.408217877f, (timeOfDay/6f ) - 2f, 0.109381631f, 0.875426114f));
        }
        if (timeOfDay > 6 && timeOfDay < 11)
        {           
            sun.GetComponent<Light>().intensity += Time.deltaTime * intensityScale;
            sun.GetComponent<Light>().color = Color.Lerp(settingColor, noonColor, colorStep * Time.deltaTime);
            skyboxMaterial.SetFloat("_Exposure", (timeOfDay - 5) / 5);
            //RenderSettings.fogColor = Color.Lerp(Color.black, Color.grey, colorStep * Time.deltaTime);
            float newValue = (timeOfDay - 6) / 10;
            RenderSettings.fogColor = new Color(newValue, newValue, newValue);
        }
        if (timeOfDay > 16 && timeOfDay < 21)
        {
            sun.GetComponent<Light>().intensity -= Time.deltaTime * intensityScale;
            sun.GetComponent<Light>().color = Color.Lerp(noonColor, settingColor, colorStep * Time.deltaTime);
            skyboxMaterial.SetFloat("_Exposure", 1.4f - ((timeOfDay - 15) / 5));
            //RenderSettings.fogColor = Color.Lerp(Color.grey, Color.black, colorStep * Time.deltaTime);
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
        //elapsedFrames = (elapsedFrames + 1) % (interpolationFramesCount + 1);
    }

    public void UpdateValue(string field)
    {

    }
}
