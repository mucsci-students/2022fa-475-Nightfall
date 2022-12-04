using UnityEngine.EventSystems;

public interface ICustomMessenger : IEventSystemHandler
{
    void InventoryMenuMessage();

    void BenderModeMessage();
}
