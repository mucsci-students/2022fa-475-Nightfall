using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class CraftingBtnEvents : MonoBehaviour
{
    private bool msgDisplaying = false;

    public void Planks()
    {
        if (!FirstPersonController.IsNearFurnace() && !msgDisplaying)
        {
            msgDisplaying = true;
            UIManager.SetMsgText("You must have a workbench nearby to make this");
            StartCoroutine(FadeMsg());
        }
        else
        {
            Debug.Log("made plank");
        }
    }
    public void CutStone()
    {
        if (!FirstPersonController.IsNearFurnace() && !msgDisplaying)
        {
            msgDisplaying = true;
            UIManager.SetMsgText("You must have a workbench nearby to make this");
            StartCoroutine(FadeMsg());
        }
    }
    public void CopperBar()
    {
        if (!FirstPersonController.IsNearFurnace() && !msgDisplaying)
        {
            msgDisplaying = true;
            UIManager.SetMsgText("You must have a furnace nearby to make this");
            StartCoroutine(FadeMsg());
        }
    }
    public void IronBar()
    {
        if (!FirstPersonController.IsNearFurnace() && !msgDisplaying)
        {
            msgDisplaying = true;
            UIManager.SetMsgText("You must have a furnace nearby to make this");
            StartCoroutine(FadeMsg());
        }
    }
    public void StoneSword()
    {
        if (Inventory.GetTool("sword") != "Wood" && !msgDisplaying)
        {
            msgDisplaying = true;
            UIManager.SetMsgText("You must have a Wooden Sword to make this");
            StartCoroutine(FadeMsg());
        }
    }
    public void StoneAxe()
    {
        if (Inventory.GetTool("axe") != "Wood" && !msgDisplaying)
        {
            msgDisplaying = true;
            UIManager.SetMsgText("You must have a Wooden Axe to make this");
            StartCoroutine(FadeMsg());
        }
    }
    public void StonePickaxe()
    {
        if (Inventory.GetTool("pickaxe") != "Wood" && !msgDisplaying)
        {
            msgDisplaying = true;
            UIManager.SetMsgText("You must have a Wooden Pickaxe to make this");
            StartCoroutine(FadeMsg());
        }
    }
    public void CopperSword()
    {
        if (Inventory.GetTool("sword") != "Stone" && !msgDisplaying)
        {
            msgDisplaying = true;
            UIManager.SetMsgText("You must have a Stone Sword to make this");
            StartCoroutine(FadeMsg());
        }
    }
    public void CopperAxe()
    {
        if (Inventory.GetTool("axe") != "Stone" && !msgDisplaying)
        {
            msgDisplaying = true;
            UIManager.SetMsgText("You must have a Stone Axe to make this");
            StartCoroutine(FadeMsg());
        }
    }
    public void CopperPickaxe()
    {
        if (Inventory.GetTool("pickaxe") != "Stone" && !msgDisplaying)
        {
            msgDisplaying = true;
            UIManager.SetMsgText("You must have a Stone Pickaxe to make this");
            StartCoroutine(FadeMsg());
        }
    }
    public void IronSword()
    {
        if (Inventory.GetTool("sword") != "Copper" && !msgDisplaying)
        {
            msgDisplaying = true;
            UIManager.SetMsgText("You must have a Copper Sword to make this");
            StartCoroutine(FadeMsg());
        }
    }
    public void IronAxe()
    {
        if (Inventory.GetTool("axe") != "Copper" && !msgDisplaying)
        {
            msgDisplaying = true;
            UIManager.SetMsgText("You must have a Copper Axe to make this");
            StartCoroutine(FadeMsg());
        }
    }
    public void IronPickaxe()
    {
        if (Inventory.GetTool("pickaxe") != "Copper" && !msgDisplaying)
        {
            msgDisplaying = true;
            UIManager.SetMsgText("You must have a Copper Pickaxe to make this");
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
