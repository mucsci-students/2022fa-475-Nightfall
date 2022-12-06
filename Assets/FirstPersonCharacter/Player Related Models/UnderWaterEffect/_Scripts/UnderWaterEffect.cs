using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class UnderWaterEffect : MonoBehaviour
{
    public Material _mat;

    [Range(0.001f, 0.1f)]
    public float pixelOffset=0.02f;
    [Range(0.1f, 20f)]
    public float noiseScale=1f;
    [Range(0.1f, 20f)]
    public float noiseFrequency=2.2f;
    [Range(0.1f, 30f)]
    public float noiseSpeed=3.8f;

    public float depthStart = 1;
    public float depthDistance = 120;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        _mat.SetFloat("_PixelOffset", pixelOffset);
        _mat.SetFloat("_NoiseScale", noiseScale);
        _mat.SetFloat("_NoiseFrequency", noiseFrequency);
        _mat.SetFloat("_NoiseSpeed", noiseSpeed);
        _mat.SetFloat("_DepthStart", depthStart);
        _mat.SetFloat("_DepthDistance", depthDistance);

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source,destination,_mat);
    }
}
