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
    private Animator playerAnim;
    public int selectedWeapon { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        swordController = transform.GetChild(0).GetComponent<SwordController>();
        axeController = transform.GetChild(1).GetComponent<AxeController>();
        pickaxeController = transform.GetChild(2).GetComponent<PickaxeController>();
        playerAnim = GameObject.Find("Male").GetComponent<Animator>();
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
            playerAnim.Rebind();
            playerAnim.Update(0f);
            Select(selectedWeapon);

            UpdateAnimation();
        
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

    public void ForceWeapon(int weaponIndex)
    {

        selectedWeapon = weaponIndex;
        Select(selectedWeapon);
        UpdateAnimation();

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

    private void UpdateAnimation()
    {
        
        switch (selectedWeapon)
        {
            case 0:
                playerAnim.SetTrigger("swordEquip");
                break;

            case 1:
                playerAnim.SetTrigger("axeEquip");
                break;

            case 2:
                playerAnim.SetTrigger("pickEquip");
                break;

            case 3:
                playerAnim.SetTrigger("torchEquip");
                break;
        }

    }
}