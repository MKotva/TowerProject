using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum DoorType
{
    LeftPath,
    RightPath,
    Escape
}

public class RoomDoor : MonoBehaviour
{
    public DoorType doorType;
    public Button doorButton;
    public GameObject lockedOverlay;
    public GameObject unlockedOverlay;

    private RoomController _controller;
    private bool _isLocked = true;

    private void Awake()
    {
        if (doorButton == null)
            doorButton = GetComponent<Button>();

        SetLocked(true);
    }

    public void Init(RoomController controller)
    {
        _controller = controller;

        if (doorButton != null)
        {
            doorButton.onClick.RemoveAllListeners();
            doorButton.onClick.AddListener(OnButtonClicked);
        }
    }

    private void OnButtonClicked()
    {
        if (_isLocked)
            return;

        _controller.OnDoorChosen(this);
    }

    public void SetLocked(bool locked)
    {
        _isLocked = locked;

        if (doorButton != null)
            doorButton.interactable = !locked;

        if (lockedOverlay != null)
            lockedOverlay.SetActive(locked);

        if (unlockedOverlay != null)
            unlockedOverlay.SetActive(!locked);
    }
}