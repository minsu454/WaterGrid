using Common.SceneEx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPopup : BasePopupUI
{

    public void Retry()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.InGame);
    }

    public void GoTitle()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.InGame);
    }
}
