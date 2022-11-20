using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] weapons;

    [Header("Keys")]
    [SerializeField] private KeyCode[] keys;

    private int selectedWeapon;
    private string weaponName;

    // Start is called before the first frame update
    void Start()
    {
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
        weaponName = weapons[weaponIndex].name;
    }

    public string GetName()
    {
        return weaponName;
    }
}