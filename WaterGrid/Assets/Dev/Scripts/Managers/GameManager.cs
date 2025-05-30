using Common.Objects;
using Common.Pool;
using Common.Time;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour, IInit
{
    [Header("Dev")]
    [SerializeField] private int timeSpeed = 1;

    private int score = 0;
    public int Score
    {
        get { return score; }
        private set
        {
            score = value;
            SetScoreEvent?.Invoke(score);
        }
    }

    public event Action<int> SetScoreEvent;

    private bool isGameOver = false;
    private bool isGameClear = false;

    public void Init()
    {
        TimeType.InGame.SetTime(timeSpeed);
    }

    public void PlusScore(int value)
    {
        Score = this.score + value;
    }

    public void GameOver()
    {
        if (isGameOver is true)
            return;

        TimeManager.SetTime(TimeType.InGame, 0f);
        Managers.UI.CreatePopup<GameOverPopup>();
        isGameOver = true;
    }

    public void GameClear()
    {
        if (isGameClear is true)
            return;

        TimeManager.SetTime(TimeType.InGame, 0f);
        Managers.UI.CreatePopup<GameClearPopup>();
        isGameClear = true;
    }
}
