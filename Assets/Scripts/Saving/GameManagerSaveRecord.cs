using UnityEngine;

public class GameManagerSaveRecord : SaveRecord
{

    public float TimeOfDay { get; }
    public float SunIntensity { get; }
    public float Exposure { get; }
    public float t { get; }
    public int DaysSurvived { get; }
    public int CurrentStructures { get; }
    public SerializableVector3 LightColor { get; }
    public SerializableVector3 FogColor { get; }

    public GameManagerSaveRecord(
        string gameObjectName, 
        float timeOfDay,
        float sunIntensity,
        float exposure,
        float _t,
        int daysSurvived,
        int currentStructures,
        Color lightColor,
        Color fogColor)
    {

        Target = gameObjectName;
        RestoreMethod = RestoreMethod.MATCH_AND_RESTORE;

        TimeOfDay = timeOfDay;
        SunIntensity = sunIntensity;
        Exposure = exposure;
        t = _t;
        DaysSurvived = daysSurvived;
        CurrentStructures = currentStructures;
        LightColor = new SerializableVector3(lightColor.r, lightColor.g, lightColor.b);
        FogColor = new SerializableVector3(fogColor.r, fogColor.g, fogColor.b);
    }

}
