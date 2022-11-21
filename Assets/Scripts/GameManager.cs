using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ToolData tools;

    public static PlayerHandler newPlr;
    public Inventory plrInv;
    private UIManager _plrUI;


    // Start is called before the first frame update
    void Start()
    {
        tools.Initialize();

        newPlr = this.AddComponent<PlayerHandler>();
        newPlr.Initialize();

        plrInv = this.AddComponent<Inventory>();
        plrInv.Initialize();

        _plrUI = this.AddComponent<UIManager>();
        _plrUI.Initialize();

    }
    float t = 0;
    void Update()
    {
        t += Time.deltaTime;
        if (t > 1)
        {
            newPlr.AddValue(_plrUI, "health", -1);
            t = 0;
        }
    }

    public void UpdateValue(string field)
    {

    }

    public static PlayerHandler GetPlr()
    {
        return newPlr;
    }
}
