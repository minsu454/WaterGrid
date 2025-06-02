using Common.SceneEx;
using TMPro;
using UnityEngine;

public class InGame_UI : BaseSceneUI
{
    [SerializeField] private ClockUI clockUI;
    [SerializeField] private UIGridLayout errorlayout;
    [SerializeField] private UIGridLayout uselayout;

    [SerializeField] private TextMeshProUGUI ScoreText;

    public override void Init()
    {
        base.Init();

        clockUI.Init();
        errorlayout.Init();
        uselayout.Init();

        InGameLoader.Game.SetScoreEvent += OnSetScore;
    }

    public void Back()
    {
        
    }

    public void OnSetScore(int score)
    {
        ScoreText.text = score.ToString();
    }
}
