using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBtnEvents : MonoBehaviour
{
    private BuildingManager BuildableItems;
    private BuildObjectPair spawnables = new BuildObjectPair();
    private Dictionary<string, int> cost = new Dictionary<string, int>();
    (BuildObjectPair, Dictionary<string, int>) result;
    private bool msgDisplaying = false;

    void Start()
    {
        BuildableItems = GameObject.Find("Player").GetComponent<BuildingManager>();
        result = (spawnables, cost);
    }

    public void Workbench()
    {
        TryBuild("Workbench");
    }
    public void Furnace()
    {
        TryBuild("Furnace");
    }
    public void Campfire()
    {
        TryBuild("Campfire");
    }
    public void LogFloor()
    {
        TryBuild("Log Floor");
    }
    public void LogWall()
    {
        TryBuild("Log Wall");
    }
    public void LogWallDoor()
    {
        TryBuild("Log Wall (Doorway)");
    }
    public void LogWallWindow()
    {
        TryBuild("Log Wall (Window)");
    }
    public void LogStairsShort()
    {
        TryBuild("Log Stairs (Short)");
    }
    public void LogStairsLong()
    {
        TryBuild("Log Stairs (Long)");
    }
    public void StoneWall()
    {
        TryBuild("Stone Wall");
    }
    public void StoneWallDoor()
    {
        TryBuild("Stone Wall (Doorway)");
    }

    public void TryBuild(string obj)
    {
        if (BuildableItems.CanBuild(obj, Inventory.GetAllItems(), out result))
        {
            BuildableItems.EnterBuildMode(result);
            UIManager.EnterBuildMode();
        }
        else if (!msgDisplaying)
        {
            msgDisplaying = true;
            UIManager.SetMsgText("You don't have enough materials to make this");
            StartCoroutine(FadeMsg());
        }
    }

    IEnumerator FadeMsg()
    {
        yield return new WaitForSeconds(2);

        UIManager.SetMsgText("");
        msgDisplaying = false;
    }
}
