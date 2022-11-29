using UnityStandardAssets.Characters.FirstPerson;

/// <summary>
/// A class for tracking player data to be saved
/// </summary>
public class PlayerSaveTracker : SaveGameTrackable
{

    private FirstPersonController _controller;
    private MouseLook _mouseLook;
    private PlayerSaveRecord _deserializedRecord;
    private WeaponController _weaponController;

    public void Start()
    {

        _controller = GetComponent<FirstPersonController>();
        _mouseLook = _controller.GetMouseLook();
        _weaponController = GetComponentInChildren<WeaponController>();

    }

    public override SaveRecord GenerateSaveRecord() => new PlayerSaveRecord(
        gameObject.name,
        gameObject.transform,
        new SerializableQuaternion(_mouseLook.m_CharacterTargetRot),
        new SerializableQuaternion(_mouseLook.m_CameraTargetRot),
        _weaponController.selectedWeapon,
        Inventory.GetAllItems(),
        Inventory.GetAllTools(),
        PlayerHandler._health,
        PlayerHandler._stamina
    );

    public override void RestoreFromSaveRecord(string recordJson)
    {

        // Deserialize the save data to the appropriate save record format
        _deserializedRecord = SaveFile.DeserializeRecord<PlayerSaveRecord>(recordJson);

        // Restore the player's location, rotation, and scale
        if (_controller != null) 
        {
            
            _controller.enabled = false;
            gameObject.transform.position = _deserializedRecord.PlayerTransform.Position;
            gameObject.transform.rotation = _deserializedRecord.PlayerTransform.Rotation;
            gameObject.transform.localScale = _deserializedRecord.PlayerTransform.Scale;

            _mouseLook.m_CameraTargetRot = _deserializedRecord.TargetCameraRotation.Value;
            _mouseLook.m_CharacterTargetRot = _deserializedRecord.TargetPlayerRotation.Value;

            // Force this to be different so the animation catches on later
            _weaponController.ForceWeapon(_deserializedRecord.SelectedWeapon);

            // Restore player vitality
            PlayerHandler.SetVitality(_deserializedRecord.Health, _deserializedRecord.Stamina);

            Inventory.AddItems(_deserializedRecord.PlayerInventory);
            Inventory.SetTools(_deserializedRecord.PlayerTools);

            // Wait for all calls to Update() to finish in the controller before continuing
            Invoke(nameof(FinishRestore), 1.2f);
            
        }

    }

    private void FinishRestore() => _controller.enabled = true;

}
