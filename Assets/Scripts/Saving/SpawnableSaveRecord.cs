using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableSaveRecord : SaveRecord
{

    public SerializableTransform ItemTransform;

    public SpawnableSaveRecord(SerializableTransform withTransform, string targetPath)
    {

        RestoreMethod = RestoreMethod.INSTANTIATE_THEN_RESTORE;
        ItemTransform = withTransform;
        Target = targetPath;

    }

}
