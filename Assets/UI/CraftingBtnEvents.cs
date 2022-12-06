using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class CraftingBtnEvents : MonoBehaviour
{
    private bool msgDisplaying = false;

    public void Planks()
    {
        if (!FirstPersonController.IsNearWorkbench())
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a workbench nearby to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (Inventory.GetCount("Wood") < 2)
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("Not enough Wood to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        else
        {
            Inventory.RemoveItem("Wood", 2);
            Inventory.AddItem("Planks", 1);
            msgDisplaying = true;
            UIManager.SetMsgText("Created 1 Plank");
            StartCoroutine(FadeMsg());
        }
    }
    public void CutStone()
    {
        if (!FirstPersonController.IsNearWorkbench())
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a workbench nearby to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (Inventory.GetCount("Stone") < 2)
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("Not enough Stone to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        else
        {
            Inventory.RemoveItem("Stone", 2);
            Inventory.AddItem("CutStone", 1);
            msgDisplaying = true;
            UIManager.SetMsgText("Created 1 Cut Stone");
            StartCoroutine(FadeMsg());
        }
    }
    public void CopperBar()
    {
        if (!FirstPersonController.IsNearFurnace())
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a furnace nearby to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (Inventory.GetCount("Copper Ore") < 2)
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("Not enough Copper Ore to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        else
        {
            Inventory.RemoveItem("Copper Ore", 2);
            Inventory.AddItem("Copper Bar", 1);
            msgDisplaying = true;
            UIManager.SetMsgText("Created 1 Copper Bar");
            StartCoroutine(FadeMsg());
        }
    }
    public void IronBar()
    {
        if (!FirstPersonController.IsNearFurnace())
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a furnace nearby to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (Inventory.GetCount("IronOre") < 2)
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("Not enough Iron Ore to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        else
        {
            Inventory.RemoveItem("Iron Ore", 2);
            Inventory.AddItem("Iron Bar", 1);
            msgDisplaying = true;
            UIManager.SetMsgText("Created 1 Iron Bar");
            StartCoroutine(FadeMsg());
        }
    }
    public void StoneSword()
    {
        if (Inventory.GetTool("sword") != "Wood")
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a Wooden Sword to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (!FirstPersonController.IsNearWorkbench())
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a workbench nearby to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (Inventory.GetCount("CutStone") < 10 || Inventory.GetCount("Planks") < 10)
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("Not enough materials to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        else
        {
            Inventory.RemoveItem("CutStone", 10);
            Inventory.RemoveItem("Planks", 10);
            Inventory.ChangeTool("sword", "Stone");
            msgDisplaying = true;
            UIManager.SetMsgText("Upgraded to Stone Sword");
            StartCoroutine(FadeMsg());
        }
    }
    public void StoneAxe()
    {
        if (Inventory.GetTool("axe") != "Wood")
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a Wooden Axe to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (!FirstPersonController.IsNearWorkbench())
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a workbench nearby to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (Inventory.GetCount("CutStone") < 12 || Inventory.GetCount("Planks") < 8)
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("Not enough materials to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        else
        {
            Inventory.RemoveItem("CutStone", 12);
            Inventory.RemoveItem("Planks", 8);
            Inventory.ChangeTool("axe", "Stone");
            msgDisplaying = true;
            UIManager.SetMsgText("Upgraded to Stone Axe");
            StartCoroutine(FadeMsg());
        }
    }
    public void StonePickaxe()
    {
        if (Inventory.GetTool("pickaxe") != "Wood")
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a Wooden Pickaxe to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (!FirstPersonController.IsNearWorkbench())
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a workbench nearby to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (Inventory.GetCount("CutStone") < 11 || Inventory.GetCount("Planks") < 9)
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("Not enough materials to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        else
        {
            Inventory.RemoveItem("CutStone", 11);
            Inventory.RemoveItem("Planks", 9);
            Inventory.ChangeTool("pickaxe", "Stone");
            msgDisplaying = true;
            UIManager.SetMsgText("Upgraded to Stone Pickaxe");
            StartCoroutine(FadeMsg());
        }
    }
    public void CopperSword()
    {
        if (Inventory.GetTool("sword") != "Stone")
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a Stone Sword to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (!FirstPersonController.IsNearWorkbench())
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a workbench nearby to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (Inventory.GetCount("Copper Bar") < 10 || Inventory.GetCount("Planks") < 10)
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("Not enough materials to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        else
        {
            Inventory.RemoveItem("Copper Bar", 10);
            Inventory.RemoveItem("Planks", 10);
            Inventory.ChangeTool("sword", "Copper");
            msgDisplaying = true;
            UIManager.SetMsgText("Upgraded to Copper Sword");
            StartCoroutine(FadeMsg());
        }
    }
    public void CopperAxe()
    {
        if (Inventory.GetTool("axe") != "Stone")
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a Stone Axe to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (!FirstPersonController.IsNearWorkbench())
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a workbench nearby to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (Inventory.GetCount("Copper Bar") < 12 || Inventory.GetCount("Planks") < 8)
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("Not enough materials to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        else
        {
            Inventory.RemoveItem("Copper Bar", 12);
            Inventory.RemoveItem("Planks", 8);
            Inventory.ChangeTool("axe", "Copper");
            msgDisplaying = true;
            UIManager.SetMsgText("Upgraded to Copper Axe");
            StartCoroutine(FadeMsg());
        }
    }
    public void CopperPickaxe()
    {
        if (Inventory.GetTool("pickaxe") != "Stone")
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a Stone Pickaxe to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (!FirstPersonController.IsNearWorkbench())
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a workbench nearby to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (Inventory.GetCount("Copper Bar") < 11 || Inventory.GetCount("Planks") < 9)
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("Not enough materials to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        else
        {
            Inventory.RemoveItem("Copper Bar", 11);
            Inventory.RemoveItem("Planks", 9);
            Inventory.ChangeTool("pickaxe", "Copper");
            msgDisplaying = true;
            UIManager.SetMsgText("Upgraded to Copper Pickaxe");
            StartCoroutine(FadeMsg());
        }
    }
    public void IronSword()
    {
        if (Inventory.GetTool("sword") != "Copper")
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a Copper Sword to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (!FirstPersonController.IsNearWorkbench())
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a workbench nearby to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (Inventory.GetCount("Iron Bar") < 10 || Inventory.GetCount("Planks") < 10)
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("Not enough materials to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        else
        {
            Inventory.RemoveItem("Iron Bar", 10);
            Inventory.RemoveItem("Planks", 10);
            Inventory.ChangeTool("sword", "Iron");
            msgDisplaying = true;
            UIManager.SetMsgText("Upgraded to Iron Sword");
            StartCoroutine(FadeMsg());
        }
    }
    public void IronAxe()
    {
        if (Inventory.GetTool("axe") != "Copper")
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a Copper Axe to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (!FirstPersonController.IsNearWorkbench())
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a workbench nearby to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (Inventory.GetCount("Iron Bar") < 12 || Inventory.GetCount("Planks") < 8)
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("Not enough materials to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        else
        {
            Inventory.RemoveItem("Iron Bar", 12);
            Inventory.RemoveItem("Planks", 8);
            Inventory.ChangeTool("axe", "Iron");
            msgDisplaying = true;
            UIManager.SetMsgText("Upgraded to Iron Axe");
            StartCoroutine(FadeMsg());
        }
    }
    public void IronPickaxe()
    {
        if (Inventory.GetTool("pickaxe") != "Copper")
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a Copper Pickaxe to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (!FirstPersonController.IsNearWorkbench())
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("You must have a workbench nearby to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        if (Inventory.GetCount("Iron Bar") < 11 || Inventory.GetCount("Planks") < 9)
        {
            if (!msgDisplaying)
            {
                msgDisplaying = true;
                UIManager.SetMsgText("Not enough materials to make this");
                StartCoroutine(FadeMsg());
            }
            return;
        }
        else
        {
            Inventory.RemoveItem("Iron Bar", 11);
            Inventory.RemoveItem("Planks", 9);
            Inventory.ChangeTool("pickaxe", "Iron");
            msgDisplaying = true;
            UIManager.SetMsgText("Upgraded to Iron Pickaxe");
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
