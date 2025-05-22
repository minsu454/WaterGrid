using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class SceneEditorManager
{
    private static string brforeScenePath; // 현재 씬 저장용
    private static Scene brforeScene;     // 임시 씬

    public static void OpenTempScene(string path)
    {
        if (brforeScene.IsValid())
            return;

        brforeScenePath = EditorSceneManager.GetActiveScene().path;
        brforeScene = EditorSceneManager.OpenScene(path);
    }

    public static void CloseTempScene()
    {
        if (!brforeScene.IsValid())
            return;

        EditorSceneManager.OpenScene(brforeScenePath);
    }
}