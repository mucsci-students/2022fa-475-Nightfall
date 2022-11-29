using System;
using UnityEngine;

public abstract class SaveGameTrackable : MonoBehaviour
{

    /// <summary>
    /// Serializes this item to a SaveRecord that can be written to a save file
    /// The save engine will automatically call this when creating a save file
    /// </summary>
    /// <returns>A SaveRecord containing data for saving the object</returns>
    public virtual SaveRecord GenerateSaveRecord() => throw new NotImplementedException("Method MUST be implemented from a subclass");

    /// <summary>
    /// Restores this item from the provided SaveRecord json
    /// The save engine will automatically call this on a matching object
    /// </summary>
    /// <param name="record">The SaveRecord json text to restore from</param>
    public virtual void RestoreFromSaveRecord(string recordJson) => throw new NotImplementedException("Method MUST be implemented from a subclass");

}
