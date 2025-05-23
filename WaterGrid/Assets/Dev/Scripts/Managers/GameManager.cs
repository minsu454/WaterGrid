using Common.Pool;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ObjectPool<Line> lineObjectPool;
    public GameObject baseObject;

    public ObjectPool<WarningIcon> warningIconObjectPool;
    public GameObject baseIcon;

    private int score = 0;
    public int Score
    {
        get { return score; }
    }

    private void Awake()
    {
        instance = this;

        lineObjectPool = new ObjectPool<Line>(nameof(Line), baseObject, null, 3);
        warningIconObjectPool = new ObjectPool<WarningIcon>(nameof(WarningIcon), baseIcon, null, 10);
    }

    public void SetScore(int score)
    {
        this.score += score;
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
