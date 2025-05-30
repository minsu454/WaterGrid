using Common.DotweenEx;
using Common.Event;
using Common.Objects;
using Common.Pool;
using System.Collections.Generic;
using UnityEngine;
using static Common.Event.Args.EventArgs;

public sealed class WarningManager : MonoBehaviour, IInit
{
    private WarningContainer _warningContainer = new WarningContainer();
    private DotweenEx dotween;

    private ObjectPool<WarningIcon> warningIconObjectPool;

    public void Init()
    {
        warningIconObjectPool = new ObjectPool<WarningIcon>(nameof(WarningIcon), ObjectManager.Return<GameObject>("WarningIcon"), null, 10);
        dotween = new DotweenEx(0, 1.5f, 1, TimeType.InGame, () => dotween = null).SetLoop(LoopType.Yoyo);
    }

    /// <summary>
    /// 에러오브젝트 추가
    /// </summary>
    public void Add(string name, IWarningable warningable)
    {
        _warningContainer.Add(name, warningable);
        EventManager.Dispatch(GameEventType.ButtonEvent, new ButtonArgs(warningable.ErrorType, _warningContainer[name].Count));
    }

    /// <summary>
    /// 에러 오브젝트 삭제
    /// </summary>
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

    /// <summary>
    /// 에러 오브젝트 위치 반환 함수
    /// </summary>
    public Transform WarningTransform(string name)
    {
        return _warningContainer.DeQueue(name).transform;
    }

    /// <summary>
    /// 아이콘 반환 함수
    /// </summary>
    public WarningIcon GetObject()
    {
        return warningIconObjectPool.GetObject();
    }

    private void Update()
    {
        dotween.OnUpdate();
        _warningContainer.SetOutLines(dotween.Value);
    }
}
