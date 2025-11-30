using UnityEngine;

public abstract class RoomBase : MonoBehaviour
{
    protected RoomController controller;

    public virtual void Init(RoomController controller)
    {
        this.controller = controller;
    }

    protected void FinishRoom()
    {
        if (controller != null)
        {
            controller.NotifyRoomCompleted();
        }
    }
}
