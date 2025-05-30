using Common.Objects;
using Common.Pool;
using Common.Time;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Dev")]
    [SerializeField] private int timeSpeed;

    private int score = 0;
    public int Score
    {
        set
        {
            score = value;
            SetScoreEvent?.Invoke(score);
        }
    }

    public event Action<int> SetScoreEvent;

    private void Awake()
    {
        Instance = this;

        TimeType.InGame.SetTime(timeSpeed);
    }

    public void PlusScore(int value)
    {
        Score = this.score + value;
    }

    public void GameOver()
    {
        TimeManager.SetTime(TimeType.InGame, 0f);
        //Managers.UI.CreatePopup<GameOverPopup>();
    }

    public void GameClear()
    {
        TimeManager.SetTime(TimeType.InGame, 0f);
        //Managers.UI.CreatePopup<GameClearPopup>();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
