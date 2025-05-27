using Common.Pool;
using Common.Save;
using System.Collections.Generic;
using UnityEngine;

public sealed class MapManager : MonoBehaviour
{
    private static MapManager Instance;

    private readonly NodeManager nodeManager = new NodeManager();

    public static NodeManager Node { get { return Instance.nodeManager; } }

    private readonly string path = $"{Application.streamingAssetsPath}/MapData/Map_1.json";

    private MapData mapData;
    private readonly CustomRandomList<Vector2Int> _randomList = new();

    [SerializeField] private GameObject housePrefab;
    [SerializeField] private GameObject pumpPrefab;
    [SerializeField] private GameObject waterPrefab;

    [SerializeField] private GameObject linePrefab;

    [SerializeField] private Grid hexGrid;

    private void Awake()
    {
        Instance = this;
        Init();
    }

    public void Init()
    {
        mapData = SaveService.Load<MapData>(path);
        
        nodeManager.Init(linePrefab, housePrefab, pumpPrefab, waterPrefab, hexGrid, mapData.TileDataList);
        SetRandomList();
    }

    private void Start()
    {
        SpawnTile();
    }

    private void Update()
    {
        nodeManager.OnUpdate();
    }

    public void SetRandomList()
    {
        foreach (var data in mapData.TileDataList)
        {
            if(data.AreaIdx >= 0)
                _randomList.Add(mapData.areaDataList[data.AreaIdx].Weight, data.Position);
        }
    }

    public void SpawnTile()
    {
        for(int i = 0; i < 10000; i++)
            _randomList.RandomPick();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
