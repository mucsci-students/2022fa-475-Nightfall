using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaver : MonoBehaviour
{

    [SerializeField]
    private float _autoSaveIntervalMinutes = 2;

    private float _saveClock;

    void Start()
    {
    
        if (!SaveEngine.HasSaveFileByName())  { Invoke(nameof(CreateFirstCheckpoint), 2); }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        _saveClock += Time.deltaTime;
        if (_saveClock >= _autoSaveIntervalMinutes * 60)
        {

            print("Auto saving...");
            SaveEngine.SaveGame();
            _saveClock = 0;

        }

    }

    private void CreateFirstCheckpoint() => SaveEngine.SaveGame();

}
