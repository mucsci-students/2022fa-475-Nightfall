using UnityEngine;

public class SaveTestScript : MonoBehaviour
{

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.F3)) { SaveEngine.SaveGame(); }

        else if (Input.GetKeyDown(KeyCode.F4)) 
        { 
            
            SaveEngine.LoadSaveFile();
            SaveEngine.ReloadWorldFromLoadedSave();

        }

    }

}
