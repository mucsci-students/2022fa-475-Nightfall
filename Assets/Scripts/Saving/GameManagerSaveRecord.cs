using Newtonsoft.Json;

public class GameManagerSaveRecord : SaveRecord
{

    public float TimeOfDay { get; }
    public int DaysSurvived { get; }
    public int CurrentStructures { get; }

    public GameManagerSaveRecord(string gameObjectName, float timeOfDay, int daysSurvived, int currentStructures)
    {

        Target = gameObjectName;
        RestoreMethod = RestoreMethod.MATCH_AND_RESTORE;

        TimeOfDay = timeOfDay;
        DaysSurvived = daysSurvived;
        CurrentStructures = currentStructures;

    }

}
