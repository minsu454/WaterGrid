using Common.Pool;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ObjectPool<Line> lineObjectPool;
    public GameObject baseObject;

    private void Awake()
    {
        instance = this;
        lineObjectPool = new ObjectPool<Line>(nameof(Line), baseObject, null, 3);
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
