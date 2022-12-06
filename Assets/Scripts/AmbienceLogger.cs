using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceLogger : MonoBehaviour
{

    private AmbienceManager _ambienceManager;
    
    // Start is called before the first frame update
    void Start()
    {

        _ambienceManager = GetComponent<AmbienceManager>();

    }

    // Update is called once per frame
    void Update()
    {

        if (_ambienceManager != null) { print(_ambienceManager.GetRecognizedTimeOfDay()); }

    }
}
