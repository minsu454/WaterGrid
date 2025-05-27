using Common.Pool;
using System.Collections.Generic;
using UnityEngine;

public sealed class NodeContainer
{
    private readonly Dictionary<Vector2Int, NodeObject> _nodeDict = new();

    private ObjectPool<House> housePool;
    private ObjectPool<Pump> pumpPool;
    private ObjectPool<Water> waterPool;

    private Grid hexGrid;

    /// <summary>
    /// 초기화 함수
    /// </summary>
    internal void Init(GameObject housePrefab, GameObject pumpPrefab, GameObject waterPrefab, Grid hexGrid, List<TileData> tileDataList)
    {
        housePool = new ObjectPool<House>(nameof(House), housePrefab, null, 3);
        pumpPool = new ObjectPool<Pump>(nameof(Pump), pumpPrefab, null, 3);
        waterPool = new ObjectPool<Water>(nameof(Water), waterPrefab, null, 1);

        this.hexGrid = hexGrid;

        foreach (TileData tileData in tileDataList)
        {
            Add(tileData.Position, tileData.TileType);
        }
    }

    /// <summary>
    /// 노드 추가 함수
    /// </summary>
    public void Add(Vector2Int data, TileType type = TileType.House)
    {
        NodeObject node = default;

        switch (type)
        {
            case TileType.House:
                node = housePool.GetObject();
                break;
            case TileType.Pump:
                node = pumpPool.GetObject();
                break;
            case TileType.Water:
                node = waterPool.GetObject();
                break;
            default:
                return;
        }
             
        node.gameObject.SetActive(true);
        node.transform.position = hexGrid.CellToWorld((Vector3Int)data);
        _nodeDict.Add(data, node);
    }

    /// <summary>
    /// 노드 삭제 함수
    /// </summary>
    public void Remove(Vector2Int data)
    {
        if (TryGetValue(data, out NodeObject node) is false)
        {
            Debug.LogError($"Data is missing. {data}");
            return;
        }

        node.gameObject.SetActive(false);
        _nodeDict.Remove(data);
    }

    /// <summary>
    /// 노드가 포함되어 있는지 체크 함수
    /// </summary>
    public bool ContainsKey(Vector2Int data)
    {
        return _nodeDict.ContainsKey(data);
    }

    /// <summary>
    /// 노드가 포함되어 있는지 체크 함수
    /// </summary>
    public bool TryGetValue(Vector2Int data, out NodeObject node)
    {
        return _nodeDict.TryGetValue(data, out node);
    }


}