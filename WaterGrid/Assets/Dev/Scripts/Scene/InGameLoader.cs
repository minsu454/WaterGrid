using Common.Assets;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class InGameLoader : BaseSceneLoader<InGameLoader>
{
    private GameManager gameManager;
    private MapManager mapManager;
    private WarningManager warningManager;

    public static GameManager Game { get { return Instance.gameManager; } }
    public static MapManager Map { get { return Instance.mapManager; } }
    public static WarningManager Warning { get { return Instance.warningManager; } }
    

    protected override void InitScene()
    {
        gameManager = Managers.CreateManager<GameManager>(transform);
        warningManager = Managers.CreateManager<WarningManager>(transform);
        mapManager = Managers.CreateManager<MapManager>(transform);
    }
}
