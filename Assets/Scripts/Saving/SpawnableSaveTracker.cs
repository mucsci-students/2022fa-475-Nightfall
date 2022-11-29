using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnableSaveTracker : SaveGameTrackable
{

    public string PrefabPath;

    public override SaveRecord GenerateSaveRecord()
    {

        SerializableTransform serializableTransform = new SerializableTransform(transform);        
        SpawnableSaveRecord record = new SpawnableSaveRecord(serializableTransform, PrefabPath);
        return record;

    }

    public override void RestoreFromSaveRecord(string recordJson)
    {

        SpawnableSaveRecord record = ObjectExtensions.FromJson<SpawnableSaveRecord>(recordJson);
        gameObject.transform.position = record.ItemTransform.Position;
        gameObject.transform.rotation = record.ItemTransform.Rotation;
        gameObject.transform.localScale = record.ItemTransform.Scale;
    
    }

}
