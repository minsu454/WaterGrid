using Common.DotweenEx;
using UnityEngine;

public sealed class WarningManager : MonoBehaviour
{
    private WarningContainer _warningContainer = new WarningContainer();
    private DotweenEx dotween;

    private void Awake()
    {
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
}
