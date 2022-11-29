using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
/// Represents a save file containing data for restoring the world
/// </summary>
public class SaveFile
{

    /// <summary>
    /// All records tracked in this save file, serialized to JSON
    /// </summary>
    [JsonProperty]
    private Dictionary<int, string> RecordData;

    /// <summary>
    /// The delay in seconds that the save game engine should wait for before restoring the save game data
    /// Allows time for objects' Start() methods to be processed.
    /// </summary>
    [JsonProperty]
    public float RestoreCooldown { get; private set; }

    /// <summary>
    /// Returns the string contents and base records of every stored record
    /// More specific records will need to be extracted
    /// </summary>
    [JsonIgnore]
    public IEnumerable<(SaveRecord baseRecord, string recordJson)> EveryRecordData
        => RecordData.Select(pair => (ObjectExtensions.FromJson<SaveRecord>(pair.Value), pair.Value));

    public SaveFile() { }

    public SaveFile(IEnumerable<SaveRecord> records)
    {

        RestoreCooldown = 1.2f;

        RecordData = new Dictionary<int, string>();
        foreach(var record in records)
        {

            record.HashCode = record.GetHashCode();
            RecordData.Add(record.HashCode, record.ToJson());

        }

    }

    public static void SaveToFile(SaveFile toSave, string fileName) => File.WriteAllText(fileName, toSave.ToJson());

    public static SaveFile ReadSaveFile(string fileName) => ObjectExtensions.FromJson<SaveFile>(File.ReadAllText(fileName));

    /// <summary>
    /// Returns the JSON text of the serialized record
    /// </summary>
    /// <param name="recordHash">The hash of the record to read</param>
    /// <returns>A string containing the JSON serialization of the specified record</returns>
    public string GetSaveJson(int recordHash) => RecordData[recordHash];

    /// <summary>
    /// Attempts to deserialize the record to the specified type 
    /// </summary>
    /// <typeparam name="T">The type to deserialize the record to. Must extend SaveRecord</typeparam>
    /// <param name="recordText">The JSON text of the record to deserialize</param>
    /// <returns>The deserialized record</returns>
    public static T DeserializeRecord<T>(string recordText) where T : SaveRecord
        => ObjectExtensions.FromJson<T>(recordText);

}
