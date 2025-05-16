using Common.SceneEx;
using UnityEngine;

public class InGame_UI : BaseSceneUI
{
    [SerializeField] private ClockUI clockUI;

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
