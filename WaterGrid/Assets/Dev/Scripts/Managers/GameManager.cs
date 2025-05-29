using Common.Pool;
using Common.Time;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Dev")]
    [SerializeField] private int timeSpeed;

    private int score = 0;
    public int Score
    {
        get { return score; }
    }

    private void Awake()
    {
        instance = this;

        TimeType.InGame.SetTime(timeSpeed);
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
