using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class AutoSaver : MonoBehaviour
{

    [SerializeField]
    private float _autoSaveIntervalMinutes = 2;

    private float _saveClock;

    void Start()
    {
    
        if (!SaveEngine.HasSaveFileByName())  { Invoke(nameof(WriteCheckpoint), 2); }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        _saveClock += Time.deltaTime;
        if (_saveClock >= _autoSaveIntervalMinutes * 60)
        {

            // Save only if the player is not dead
            if (!PlayerHandler.PlayerIsDead) { WriteCheckpoint(); }
            _saveClock = 0;

        }

    }

    private void WriteCheckpoint()
    {
        
        print("Auto saving...");
        SaveEngine.SaveGame();

    }

}
