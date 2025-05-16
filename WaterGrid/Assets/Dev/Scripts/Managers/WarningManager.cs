using Common.DotweenEx;
using UnityEngine;

public sealed class WarningManager : MonoBehaviour
{
    public static WarningManager Instance;

    private WarningContainer _warningContainer = new WarningContainer();
    public WarningContainer warningContainer { get { return _warningContainer; } }
    private DotweenEx dotween;

    private void Awake()
    {
        Instance = this;
        Init();
    }

    private void Init()
    {
        dotween = new DotweenEx(0, 1.5f, 1, () => { dotween = null; }).SetLoop(LoopType.Yoyo);
    }

    private void Update()
    {
        dotween.OnUpdate();
        _warningContainer.SetOutLines(dotween.Value);
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
