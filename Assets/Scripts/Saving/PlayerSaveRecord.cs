using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveRecord : SaveRecord
{

    public SerializableTransform PlayerTransform;
    public SerializableQuaternion TargetPlayerRotation;
    public SerializableQuaternion TargetCameraRotation;
    public Dictionary<string, int> PlayerInventory;
    public int SelectedWeapon;
    public int Health;
    public int Stamina;

    public PlayerSaveRecord() {  }

    public PlayerSaveRecord(
        string targetPlayer, 
        Transform playerTransform,
        SerializableQuaternion targetPlayerRotation,
        SerializableQuaternion targetCameraRotation,
        int selectedWeapon,
        Dictionary<string, int> playerInventory,
        int health,
        int stamina)
    {

        PlayerTransform = new SerializableTransform(playerTransform);

        Target = targetPlayer;
        RestoreMethod = RestoreMethod.MATCH_AND_RESTORE;
        TargetPlayerRotation = targetPlayerRotation;
        TargetCameraRotation = targetCameraRotation;
        SelectedWeapon = selectedWeapon;
        PlayerInventory = playerInventory;
        Health = health;
        Stamina = stamina;

    }

}
