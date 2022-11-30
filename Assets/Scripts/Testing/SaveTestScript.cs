using UnityEngine;

public class SaveTestScript : MonoBehaviour
{

    [SerializeField]
    private string _saveFileName = "manualsave";
    
    [SerializeField]
    private bool _allowManualSaving;

    void Update()
    {
        
        if (_allowManualSaving && Input.GetKeyDown(KeyCode.F3)) { SaveEngine.SaveGame(_saveFileName); }

        else if (_allowManualSaving && Input.GetKeyDown(KeyCode.F4)) 
        { 
            
            SaveEngine.LoadSaveFile(_saveFileName);
            SaveEngine.ReloadWorldFromLoadedSave();

        }

        else if (SaveEngine.HasSaveFileByName() && Input.GetKeyDown(KeyCode.F2))
        {

            SaveEngine.LoadSaveFile();
            SaveEngine.ReloadWorldFromLoadedSave();

        }

    }

}
