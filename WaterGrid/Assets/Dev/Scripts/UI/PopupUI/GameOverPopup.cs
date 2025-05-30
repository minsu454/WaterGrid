using Common.ColorEx;
using Common.DotweenEx;
using Common.SceneEx;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : BasePopupUI
{
    [Header(nameof(GameOverPopup))]
    [SerializeField] private Image background;
    [SerializeField] private GameObject activeGameObject;
    [SerializeField] private TextMeshProUGUI scoreText;

    private DotweenEx dotween;

    public override void Init<T>(T option)
    {
        base.Init(option);

        activeGameObject.SetActive(false);
        background.color = background.color.Alpha(0);
        dotween = new DotweenEx(0, 1.5f, 1, TimeType.UI, () => dotween = null).OnCompleted(() => activeGameObject.SetActive(true));
        scoreText.text = InGameLoader.Game.Score.ToString();
    }

    private void Update()
    {
        if (dotween == null)
            return;

        dotween.OnUpdate();

        background.color = background.color.Alpha(dotween.Value);
    }

    public void Retry()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.InGame);
    }

    public void GoTitle()
    {
        SceneManagerEx.LoadingAndNextScene(SceneType.Title);
    }
}
