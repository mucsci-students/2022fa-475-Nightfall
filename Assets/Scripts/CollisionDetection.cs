using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
   private WeaponController weaponController;

    private void Start() 
    {
        weaponController = transform.parent.gameObject.GetComponent<WeaponController>();
    }
   
   private void OnTriggerEnter(Collider other) 
   {
        if(other.tag == "Enemy" && weaponController.isAttacking)
        {
            Debug.Log(other.name);
        }
   }
}
