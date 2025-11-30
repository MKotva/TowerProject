using Assets.Scripts;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public static RoomController Instance { get; private set; }

    public Transform roomParent;

    public GameObject enemyRoomPrefab;
    public GameObject treasureRoomPrefab;
    public GameObject emptyRoomPrefab;
    public GameObject puzzleRoomPrefab;
    public GameObject restStopRoomPrefab;

    public Transform uiRoot;
    public GameObject announcementPrefab;

    private GameObject _currentRoomInstance;
    private RoomNode _rootNode;
    private RoomNode _currentNode;
    private List<bool> _path = new List<bool>();
    private bool _roomCompleted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        InitializeTree();
        SpawnCurrentRoom();
    }

    private void InitializeTree()
    {
        int reachedLevel = GameManager.Instance != null ? GameManager.Instance.ReachedLevel : 1;
        _rootNode = RoomTreeGenerator.GenerateTreeForPlayer(reachedLevel);
        _currentNode = _rootNode;
        _path.Clear();
        _roomCompleted = false;
    }

    private void SpawnCurrentRoom()
    {
        if (_currentNode == null)
        {
            return;
        }

        if (_currentRoomInstance != null)
        {
            Destroy(_currentRoomInstance);
            _currentRoomInstance = null;
        }

        RoomType roomType = _currentNode.Room;
        GameObject prefab = GetPrefabForRoomType(roomType);
        if (prefab == null)
        {
            Debug.LogError("RoomController: No prefab assigned for room type " + roomType);
            return;
        }

        Transform parent = roomParent != null ? roomParent : transform;
        _currentRoomInstance = GameObject.Instantiate(prefab, parent);

        var controller = _currentRoomInstance.GetComponent<RoomBase>();
        if (controller != null)
            controller.Init(this);

        RoomDoor[] doors = _currentRoomInstance.GetComponentsInChildren<RoomDoor>(true);
        if (doors == null || doors.Length == 0)
        {
            Debug.LogWarning("RoomController: Spawned room has no RoomDoor components.");
        }
        else
        {
            foreach (RoomDoor door in doors)
            {
                door.Init(this);
                door.SetLocked(true);
            }
        }

        _roomCompleted = false;
    }

    private GameObject GetPrefabForRoomType(RoomType type)
    {
        switch (type)
        {
            case RoomType.Enemy: return enemyRoomPrefab;
            case RoomType.Treasure: return treasureRoomPrefab;
            case RoomType.Empty: return emptyRoomPrefab;
            case RoomType.Puzzle: return puzzleRoomPrefab;
            case RoomType.RestStop: return restStopRoomPrefab;
            default: return emptyRoomPrefab;
        }
    }

    public void NotifyRoomCompleted()
    {
        if (_roomCompleted)
            return;

        _roomCompleted = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReachedLevel = Mathf.Min(
                GameManager.Instance.ReachedLevel + 1,
                RoomTreeGenerator.MaxTotalLevels);
        }

        if (_currentRoomInstance != null)
        {
            RoomDoor[] doors = _currentRoomInstance.GetComponentsInChildren<RoomDoor>(true);
            foreach (RoomDoor door in doors)
            {
                door.SetLocked(false);
            }
        }
    }

    public void OnDoorChosen(RoomDoor door)
    {
        if (!_roomCompleted)
        {
            return;
        }

        switch (door.doorType)
        {
            case DoorType.LeftPath:
                ShowPathAnnouncement(true);
                break;

            case DoorType.RightPath:
                ShowPathAnnouncement(false);
                break;

            case DoorType.Escape:
                ShowRetreatAnnouncement();
                break;
        }
    }

    private void ShowPathAnnouncement(bool isLeft)
    {
        string hint = RoomHintGenerator.GetBranchHint(_currentNode, isLeft, 3);
        if (string.IsNullOrEmpty(hint))
        {
            hint = "The path ahead feels uncertain, offering no clear omen.";
        }

        string msg = hint + "\n\n" +
                     "Do you wish to continue down this path?";

        ShowAnnouncement(msg, 0, true, accepted =>
        {
            if (accepted)
            {
                GoToChildNode(isLeft);
                GameManager.Instance.ScreenBlanker.RunFadeSequence(() =>
                {
                    SpawnCurrentRoom();
                });
            }
        });
    }

    private void ShowRetreatAnnouncement()
    {
        int fee = GameManager.Instance.ReachedLevel * GameManager.Instance.towerRescueFee;

        string msg =
            "You think about leaving the battle.\n" +
            "To retreat from the tower, you must call for a rescue squad.\n" +
            "They do not risk their lives for free.\n\n" +
            "This retreat will cost you " + fee + " gold.\n\n" +
            "Do you really want to abandon this run and return?";

        ShowAnnouncement(msg, fee, true, accepted =>
        {
            if (!accepted)
                return;

            HandleEscape();
        });
    }


    private void ShowAnnouncement(string message, int price, bool isDecision, Action<bool> onResult)
    {
        GameObject popup = GameObject.Instantiate(announcementPrefab, uiRoot);
        AnnouncementController ctrl = popup.GetComponent<AnnouncementController>();

        if (ctrl == null)
        {
            GameObject.Destroy(popup);
            return;
        }

        ctrl.Init(message, price, isDecision, onResult);
    }

    private void GoToChildNode(bool isLeft)
    {
        if (_currentNode == null)
        {
            InitializeTree();
            return;
        }

        RoomNode next = isLeft ? _currentNode.Left : _currentNode.Right;

        if (next == null)
        {
            InitializeTree();
        }
        else
        {
            _currentNode = next;
            _path.Add(isLeft);
            _roomCompleted = false;
        }
    }

    private void HandleEscape()
    {
        // Example:
        GameData.manager = GameManager.Instance;
        UnityEngine.SceneManagement.SceneManager.LoadScene("VillageScene");
    }
}