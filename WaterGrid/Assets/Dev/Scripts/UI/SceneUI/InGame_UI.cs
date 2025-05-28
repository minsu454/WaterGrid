using Common.SceneEx;
using UnityEngine;

public class InGame_UI : BaseSceneUI
{
    [SerializeField] private ClockUI clockUI;
    [SerializeField] private UIGridLayout errorlayout;
    [SerializeField] private UIGridLayout uselayout;
    [SerializeField] private UIDragButton hammerBtn;
    [SerializeField] private UIDragButton PumpBtn;

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
        errorlayout.Init();
        uselayout.Init();
    }

    public void Btn()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.Title);
    }
}
