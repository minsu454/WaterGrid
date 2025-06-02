using Common.Assets;
using Common.Time;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class InGameLoader : BaseSceneLoader<InGameLoader>
{
    [SerializeField] private DevValue dev;

    private GameManager gameManager;
    private MapManager mapManager;
    private WarningManager warningManager;
    private ItemManager itemManager;

    public static GameManager Game { get { return Instance.gameManager; } }
    public static MapManager Map { get { return Instance.mapManager; } }
    public static WarningManager Warning { get { return Instance.warningManager; } }
    public static ItemManager Item { get { return Instance.itemManager; } }
    public static DevValue Dev { get { return Instance.dev; } }
    
    protected override void InitScene()
    {
        TimeType.InGame.SetTime(1);

        itemManager = new ItemManager(dev.StartPumpCount, dev.StartHammerCount);
        gameManager = Managers.CreateManager<GameManager>(transform);
        warningManager = Managers.CreateManager<WarningManager>(transform);
        mapManager = Managers.CreateManager<MapManager>(transform);
    }
}
