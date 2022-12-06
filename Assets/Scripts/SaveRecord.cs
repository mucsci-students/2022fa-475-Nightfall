/// <summary>
/// Indicates how the save game engine should go about restoring a save record
/// </summary>
public enum RestoreMethod 
{ 

    /// <summary>
    /// The save record should be matched with an object that already exists and restored on that
    /// The target is expected to implement the ??? interface
    /// </summary>
    MATCH_AND_RESTORE,

    /// <summary>
    /// A new object should be instantiated and then this save record should be restored on the newly spawned item.
    /// The target is expected to implement the ??? interface
    /// </summary>
    INSTANTIATE_THEN_RESTORE,

}

public class SaveRecord 
{

    /// <summary>
    /// Specifies how the save game engine should restore this save record
    /// </summary>
    public RestoreMethod RestoreMethod;

    /// <summary>
    /// Indicates the target that this save game record matches to 
    /// In MATCH_AND_RESTORE mode this should be the name of the object in the world that this matches to
    /// In INSTANTIATE_THEN_RESTORE mode this should be the path of the prefab to spawn <- DOCUMENT MORE
    /// </summary>
    public string Target;

    /// <summary>
    /// Generated/ used by the save engine. You should never set this yourself.
    /// </summary>
    public int HashCode;

}
