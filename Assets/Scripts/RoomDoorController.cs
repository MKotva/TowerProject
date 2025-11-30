namespace Assets.Scripts
{
    using UnityEngine;

    public class RoomDoor : MonoBehaviour
    {
        [Tooltip("Is this the left door (true) or the right door (false)?")]
        public bool IsLeftDoor;

        private RoomController _controller;

        public void Init(RoomController controller, bool isLeft)
        {
            _controller = controller;
            IsLeftDoor = isLeft;
        }

        public void OnDoorChosen()
        {
            if (_controller == null)
            {
                Debug.LogWarning("RoomDoor: No RoomController assigned.");
                return;
            }

            _controller.OnDoorChosen(IsLeftDoor);
        }
    }
}