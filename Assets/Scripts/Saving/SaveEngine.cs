using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        
        if (_worldIsDirty) { Invoke(nameof(RestoreWorld), _activeSaveFile.RestoreCooldown); }

    }

    public static void SaveGame(string saveFileName = "savefile.json")
    {

        // Save the world
        IEnumerable<SaveRecord> saveRecords = FindObjectsOfType<SaveGameTrackable>().Select(savable => savable.GenerateSaveRecord());
        _activeSaveFile = new SaveFile(saveRecords);
        SaveFile.SaveToFile(_activeSaveFile, saveFileName);

        print($"Game saved to {saveFileName}");

    }

    public static void LoadSaveFile(string saveFileName = "savefile.json")
    { 
        
        _activeSaveFile = SaveFile.ReadSaveFile(saveFileName);
        print($"Save data loaded from {saveFileName}");

    }

    public static void ReloadWorldFromLoadedSave()
    {

        // Mark the world as dirty
        _worldIsDirty = true;

        // Reload the world
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

    }

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
