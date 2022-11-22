using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] weapons;

    [Header("Keys")]
    [SerializeField] private KeyCode[] keys;

    private PickaxeController pickaxeController;
    private SwordController swordController;
    private AxeController axeController;
    private int selectedWeapon;


    // Start is called before the first frame update
    void Start()
    {
        swordController = transform.GetChild(0).GetComponent<SwordController>();
        axeController = transform.GetChild(1).GetComponent<AxeController>();
        pickaxeController = transform.GetChild(2).GetComponent<PickaxeController>();
        SetWeapons();
        Select(selectedWeapon);
    }

        // Update is called once per frame
    void Update()
    {
        int previousWeapon = selectedWeapon;

        for(int i = 0; i < keys.Length; i++)
        {
            if(Input.GetKeyDown(keys[i]))
            {
                selectedWeapon = i;
            }
        }

        if(previousWeapon != selectedWeapon)
        {
            // Scuffed fix for now. Probably not great to do this in Update(). 
            Animator anim = weapons[previousWeapon].gameObject.GetComponent<Animator>();
            // Allows the animator to reset, so our tools don't get displaced from animation cancels.
            anim.Rebind();
            anim.Update(0f);
            Select(selectedWeapon);
        }

        if(Input.GetMouseButtonDown(0))
        {
            if(selectedWeapon == 0)
            {
                swordController.SwingSword();
            }
        }

        if(Input.GetMouseButton(0))
        {
            switch(selectedWeapon)
            {
                case 1:
                    axeController.SwingAxe();
                    break;

                case 2:
                    pickaxeController.SwingPickaxe();
                    break;
            }
        }
    }

    private void SetWeapons()
    {
        weapons = new Transform[transform.childCount];

        for(int i = 0; i < transform.childCount; i++)
        {
            weapons[i] = transform.GetChild(i);
        }

        if(keys == null)
        {
            keys = new KeyCode[weapons.Length];
        }
    }

    private void Select(int weaponIndex)
    {
        for(int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == weaponIndex);
        }
    }
}