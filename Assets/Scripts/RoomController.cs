using Assets.Scripts;
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
    public GameObject WinRoom;

    private GameObject currRoomInstance;
    private RoomNode root;
    private RoomNode currNode;

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
        var reached = GameManager.Instance != null ? GameManager.Instance.ReachedLevel : 1;
        root = RoomTreeGenerator.GenerateTreeForPlayer(reached);
        currNode = root;
    }

    private void SpawnCurrentRoom()
    {
        if (currRoomInstance != null)
        {
            Destroy(currRoomInstance);
            currRoomInstance = null;
        }

        GameObject prefab = GetPrefabForRoomType(currNode.Room);
        Transform parent = roomParent != null ? roomParent : transform;
        currRoomInstance = Instantiate(prefab, parent);

        var doors = currRoomInstance.GetComponentsInChildren<RoomDoor>();
        foreach (var door in doors)
        {
            door.Init(this, door.IsLeftDoor);
        }
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

    public void OnDoorChosen(bool choseLeftDoor)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReachedLevel = Mathf.Min(GameManager.Instance.ReachedLevel + 1, RoomTreeGenerator.MaxTotalLevels);
        }
        AdvanceToNextNode(choseLeftDoor);
        SpawnCurrentRoom();
    }

    private void AdvanceToNextNode(bool choseLeftDoor)
    {
        RoomNode next = choseLeftDoor ? currNode.Left : currNode.Right;
        if (next == null)
        {
            InitializeTree();
        }
        else
        {
            currNode = next;
        }
    }
}
