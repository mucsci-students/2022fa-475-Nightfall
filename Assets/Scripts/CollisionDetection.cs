using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
   public WeaponController weaponController;
   
   private void OnTriggerEnter(Collider other) 
   {
        if(other.tag == "Enemy" && weaponController.isAttacking)
        {
            Debug.Log(other.name);
        }
   }
}
