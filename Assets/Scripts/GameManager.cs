using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerHandler _newPlr;
    private UIManager _plrUI;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _newPlr = this.AddComponent<PlayerHandler>();
        _newPlr.Initialize();

        _plrUI = this.AddComponent<UIManager>();
        _plrUI.Initialize();

    }

    public void UpdateValue(string field)
    {
        
    }
}
