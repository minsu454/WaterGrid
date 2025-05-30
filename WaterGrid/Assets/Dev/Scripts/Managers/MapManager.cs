using Common.Objects;
using Common.Save;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public sealed class MapManager : MonoBehaviour, IInit
{
    private readonly LineManager LineManager = new();
    public LineManager Line { get { return LineManager; } }

    private readonly NodeContainer _nodeContainer = new();
    private readonly CustomRandomList<Vector2Int> _randomList = new();

    private readonly string path = $"{Application.streamingAssetsPath}/MapData/Map_1.json";
    private MapData mapData;

    [Header("Grid")]
    [SerializeField] private Grid hexGrid;

    public void Init()
    {
        mapData = SaveService.Load<MapData>(path);

        GameObject housePrefab = ObjectManager.Return<GameObject>("House");
        GameObject pumpPrefab = ObjectManager.Return<GameObject>("Pump");
        GameObject waterPrefab = ObjectManager.Return<GameObject>("Water");
        GameObject linePrefab = ObjectManager.Return<GameObject>("Line");

        hexGrid = GetComponent<Grid>();
        hexGrid.cellLayout = GridLayout.CellLayout.Hexagon;
        hexGrid.cellSize = new Vector3(0.8659766f, 1f, 1f);

        _nodeContainer.Init(housePrefab, pumpPrefab, waterPrefab, hexGrid, mapData.TileDataList);
        LineManager.Init(linePrefab);
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
        LineManager.OnUpdate();
    }

    /// <summary>
    /// 난이도 상승하는 함수
    /// </summary>
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
    
    /// <summary>
    /// 노드 추가해주는 함수
    /// </summary>
    public void AddNode(Vector2 pos, TileType type)
    {
        _nodeContainer.AddedByPlayer(pos, type);
    }
}
