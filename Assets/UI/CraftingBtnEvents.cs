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

    }
    public void StoneAxe()
    {

    }
    public void StonePickaxe()
    {

    }
    public void CopperSword()
    {

    }
    public void CopperAxe()
    {

    }
    public void CopperPickaxe()
    {

    }
    public void IronSword()
    {

    }
    public void IronAxe()
    {

    }
    public void IronPickaxe()
    {

    }
    IEnumerator FadeMsg()
    {
        yield return new WaitForSeconds(2);

        UIManager.SetMsgText("");
        msgDisplaying = false;
    }
}
