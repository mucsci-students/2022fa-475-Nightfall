public class SpawnableSaveTracker : SaveGameTrackable
{

    public override SaveRecord GenerateSaveRecord()
    {

        string prefabName = gameObject.name.Substring(0, gameObject.name.IndexOf("(Clone)"));

        SerializableTransform serializableTransform = new SerializableTransform(transform);        
        SpawnableSaveRecord record = new SpawnableSaveRecord(serializableTransform, prefabName);
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
