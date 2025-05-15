using Common.SceneEx;
using UnityEngine;

public class InGame_UI : BaseSceneUI
{
    public void Btn()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.Title);
    }

    public void OnClock()
    {
        Debug.Log("clock");
    }
}
