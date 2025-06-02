using Common.SceneEx;
using Common.Time;

public class PausePopup : BasePopupUI
{
    public override void Init<T>(T option)
    {
        base.Init(option);

        TimeType.InGame.SetTime(0);
    }

    public void Retry()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.InGame);
    }

    public void GoTitle()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.Title);
    }

    public override void Close()
    {
        TimeType.InGame.SetTime(1);

        base.Close();
    }
}