using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveEngine : MonoBehaviour
{

    /// <summary>
    /// The save file that this SaveEngine is tracking
    /// </summary>
    private static SaveFile _activeSaveFile;
    
    /// <summary>
    /// Indicates whether or not the world is dirty and needs to be restored from the save file
    /// </summary>
    private static bool _worldIsDirty;

    private const string _defaultSaveFileName = "autosave";

    // Start is called before the first frame update
    void Start()
    {
        
        if (_worldIsDirty) { Invoke(nameof(RestoreWorld), _activeSaveFile.RestoreCooldown); }

    }

    public static void SaveGame(string saveFileName = _defaultSaveFileName)
    {

        // Save the world
        IEnumerable<SaveRecord> saveRecords = FindObjectsOfType<SaveGameTrackable>().Select(savable => savable.GenerateSaveRecord());
        _activeSaveFile = new SaveFile(saveRecords);
        SaveFile.SaveToFile(_activeSaveFile, saveFileName);

        print($"Game saved to {saveFileName}");

    }

    public static void LoadSaveFile(string saveFileName = _defaultSaveFileName)
    { 
        
        _activeSaveFile = SaveFile.ReadSaveFile(saveFileName);
        print($"Save data loaded from {saveFileName}");

    }

    public static void ReloadWorldFromLoadedSave() => LoadSceneFromLoadedSave(SceneManager.GetActiveScene().buildIndex);
    
    public static void LoadSceneFromLoadedSave(int sceneIndex)
    {

        // Mark the world as dirty
        _worldIsDirty = true;

        // Load the scene
        SceneManager.LoadScene(sceneIndex);

    }

    public static void LoadSaveThenLoadScene(int sceneIndex, string saveFileName = _defaultSaveFileName)
    {

        LoadSaveFile(saveFileName);
        LoadSceneFromLoadedSave(sceneIndex);

    }

    public static bool HasSaveFileByName(string saveFileName = _defaultSaveFileName) => File.Exists(_defaultSaveFileName);

    private void RestoreWorld()
    {

        IEnumerable<SaveGameTrackable> allTrackablesInWorld = FindObjectsOfType<SaveGameTrackable>();
        
        foreach (var recordPair in _activeSaveFile.EveryRecordData)
        {

            RestoreMethod restoreMethod = recordPair.baseRecord.RestoreMethod;
            if (restoreMethod == RestoreMethod.MATCH_AND_RESTORE)
            {

                SaveGameTrackable matchingItem = allTrackablesInWorld.Where(item => item.name == recordPair.baseRecord.Target).FirstOrDefault();
                if (matchingItem != null)
                {

                    matchingItem.RestoreFromSaveRecord(recordPair.recordJson);

                }

                else { Debug.LogError($"No trackable named \"{recordPair.baseRecord.Target}\" found"); }

            }

            else if (restoreMethod == RestoreMethod.INSTANTIATE_THEN_RESTORE)
            {

                var target = (GameObject)Resources.Load(recordPair.baseRecord.Target);
                var spawned = Instantiate(target);
                var trackable = spawned.GetComponent<SaveGameTrackable>();

                trackable.RestoreFromSaveRecord(recordPair.recordJson);

            }

        }

        _worldIsDirty = false;

    }

}
