using Common.SceneEx;
using UnityEngine;

public class InGame_UI : BaseSceneUI
{
    [SerializeField] private ClockUI clockUI;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        clockUI.Init();
    }

    public void Btn()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.Title);
    }
}
