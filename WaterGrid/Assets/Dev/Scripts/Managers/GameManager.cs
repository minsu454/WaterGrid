using Common.Pool;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ObjectPool<Line> lineObjectPool;
    public GameObject baseObject;

    public ObjectPool<WarningIcon> warningIconObjectPool;
    public GameObject baseIcon;

    private void Awake()
    {
        instance = this;

        lineObjectPool = new ObjectPool<Line>(nameof(Line), baseObject, null, 3);
        warningIconObjectPool = new ObjectPool<WarningIcon>(nameof(WarningIcon), baseIcon, null, 10);
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
