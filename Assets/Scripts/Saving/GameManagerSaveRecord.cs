using UnityEngine;

public class GameManagerSaveRecord : SaveRecord
{

    public float TimeOfDay { get; }
    public GameManagerSaveRecord(string gameObjectName, float timeOfDay)
    {

        Target = gameObjectName;
        RestoreMethod = RestoreMethod.MATCH_AND_RESTORE;

        TimeOfDay = timeOfDay;

    }

}
