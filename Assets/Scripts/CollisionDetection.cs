using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // private WeaponController weaponController;
    
    // private PickaxeController pickaxeController;
    // private AxeController axeController;
    // private string currentWep;
    // private bool isAttacking;
    // private bool isChopping;
    // private bool isMining;
    
    // void Start()
    // {
    //     weaponController = transform.parent.GetComponent<WeaponController>();
    //     pickaxeController = transform.parent.GetChild(2).GetComponent<PickaxeController>();
    //     axeController = transform.parent.GetChild(1).GetComponent<AxeController>();
    // }

    // private void OnTriggerEnter(Collider other) 
    // {
    //     isChopping = axeController.GetIsChopping();
    //     isMining = pickaxeController.GetIsMining();
    //     currentWep = weaponController.GetName();

    //     // If swinging at enemy with a sword
    //     if(other.tag == "Enemy" && weaponController.GetName() == "Long Sword")
    //     {
    //         Debug.Log(other.name);
    //     }
        
    //     // If swinging at Wood.
    //     // if(other.tag == "Wood" && weaponController.GetName() == "Hatchet" && isChopping)
    //     if(other.tag == "Wood" && isChopping)
    //     {
    //         axeController.PlayAxeSound();
    //         // Inventory.AddItem("Wood", 1);
    //         Debug.Log(other.name);
    //     }

    //     if(other.tag == "Stone" && isMining)
    //     {
    //         pickaxeController.PlayMiningSound();
    //         // Inventory.AddItem("Stone", 1);
    //         Debug.Log(other.name);
    //     }
    // }
}
