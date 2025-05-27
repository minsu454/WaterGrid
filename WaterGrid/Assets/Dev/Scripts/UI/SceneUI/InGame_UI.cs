using Common.SceneEx;
using UnityEngine;

public class InGame_UI : BaseSceneUI
{
    [SerializeField] private ClockUI clockUI;
    [SerializeField] private ButtonUI hammerBtn;
    [SerializeField] private ButtonUI PumpBtn;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        clockUI.Init();
        hammerBtn.Init();
        PumpBtn.Init();
    }

    public void Btn()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.Title);
    }
}
