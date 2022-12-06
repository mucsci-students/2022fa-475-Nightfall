using UnityEngine.EventSystems;

public interface ICustomMessenger : IEventSystemHandler
{
    void ToggleMenuMessage();

    void BenderModeMessage();
}
