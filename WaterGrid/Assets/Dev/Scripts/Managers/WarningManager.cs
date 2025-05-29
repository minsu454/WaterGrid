using Common.DotweenEx;
using Common.Event;
using Common.Pool;
using System.Collections.Generic;
using UnityEngine;
using static Common.Event.Args.EventArgs;

public sealed class WarningManager : MonoBehaviour
{
    public static WarningManager Instance;

    private WarningContainer _warningContainer = new WarningContainer();
    private DotweenEx dotween;

    public ObjectPool<WarningIcon> warningIconObjectPool;
    public GameObject baseIcon;

    private void Awake()
    {
        Instance = this;

        Init();
    }

    private void Init()
    {
        warningIconObjectPool = new ObjectPool<WarningIcon>(nameof(WarningIcon), baseIcon, null, 10);
        dotween = new DotweenEx(0, 1.5f, 1, () => { dotween = null; }).SetLoop(LoopType.Yoyo);
    }

    public void Add(string name, IWarningable warningable)
    {
        _warningContainer.Add(name, warningable);
        EventManager.Dispatch(GameEventType.ButtonEvent, new ButtonArgs(warningable.ErrorType, _warningContainer[name].Count));
    }

    public void Remove(string name, IWarningable warningable)
    {
        if (_warningContainer.TryGetValue(name, out List<IWarningable> warningableList) is false || warningableList.Contains(warningable) is false)
        {
            return;
        }

        warningableList.Remove(warningable);

        EventManager.Dispatch(GameEventType.ButtonEvent, new ButtonArgs(warningable.ErrorType, warningableList.Count));
         _warningContainer.Remove(name, warningable);
    }

    public Transform ErrorTransform(string name)
    {
        return _warningContainer.DeQueue(name).transform;
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
