using Common.Pool;
using Common.Save;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public sealed class MapManager : MonoBehaviour
{
    private static MapManager instance;
    public static MapManager Instance { get { return instance; } }

    public static LineManager Line { get { return Instance.nodeManager; } }

    private readonly LineManager nodeManager = new LineManager();
    private readonly NodeContainer _nodeContainer = new();
    private readonly CustomRandomList<Vector2Int> _randomList = new();

    private readonly string path = $"{Application.streamingAssetsPath}/MapData/Map_1.json";
    private MapData mapData;

    [Header("Node")]
    [SerializeField] private GameObject housePrefab;
    [SerializeField] private GameObject pumpPrefab;
    [SerializeField] private GameObject waterPrefab;

    [Header("Line")]
    [SerializeField] private GameObject linePrefab;

    [Header("Grid")]
    [SerializeField] private Grid hexGrid;

    private void Awake()
    {
        instance = this;
        Init();
    }

    public void Init()
    {
        mapData = SaveService.Load<MapData>(path);

        _nodeContainer.Init(housePrefab, pumpPrefab, waterPrefab, hexGrid, mapData.TileDataList);
        nodeManager.Init(linePrefab);
        InitRandomList(mapData);
    }

    private void Start()
    {
        CameraManager.Instance.Init(mapData.Bounds);
    }

    private void InitRandomList(MapData mapData)
    {
        foreach (var data in mapData.TileDataList)
        {
            if (data.AreaIdx >= 0)
                _randomList.Add(mapData.AreaDataList[data.AreaIdx].Weight, data.Position);
        }
    }

    private void Update()
    {
        nodeManager.OnUpdate();
    }

    public void UpgradeMap()
    {
        Vector2Int vec = _randomList.RandomPick();

        if (_nodeContainer.TryGetValue(vec, out NodeObject node) is true)
        {
            node.Upgrade(1);
        }
        else
        {
            _nodeContainer.Add(vec);
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
