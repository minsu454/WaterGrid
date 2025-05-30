using Common.SceneEx;
using TMPro;
using UnityEngine;

public class InGame_UI : BaseSceneUI
{
    [SerializeField] private ClockUI clockUI;
    [SerializeField] private UIGridLayout errorlayout;
    [SerializeField] private UIGridLayout uselayout;

    [SerializeField] private TextMeshProUGUI ScoreText;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        clockUI.Init();
        errorlayout.Init();
        uselayout.Init();

        GameManager.Instance.SetScoreEvent += OnSetScore;
    }

    public void Btn()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.Title);
    }

    public void OnSetScore(int score)
    {
        ScoreText.text = score.ToString();
    }
}
