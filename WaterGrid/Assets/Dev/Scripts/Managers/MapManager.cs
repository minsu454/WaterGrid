using Common.Pool;
using Common.Save;
using UnityEngine;

public sealed class MapManager : MonoBehaviour
{
    private static MapManager Instance;

    private readonly NodeManager nodeManager = new NodeManager();

    public static NodeManager Node { get { return Instance.nodeManager; } }

    private readonly string path = $"{Application.streamingAssetsPath}/MapData/Map_2.json";

    private MapData mapData;

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
    }

    private void Update()
    {
        nodeManager.OnUpdate();
    }

    public void SpawnTile()
    {

    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
