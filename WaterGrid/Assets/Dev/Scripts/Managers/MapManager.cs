using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MapManager : MonoBehaviour
{
    private static MapManager Instance;

    private readonly NodeManager nodeManager = new NodeManager();

    public static NodeManager Node { get { return Instance.nodeManager; } }

    private void Awake()
    {
        Instance = this;
        Init();
    }

    public void Init()
    {

    }

    private void Update()
    {
        nodeManager.OnUpdate();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
