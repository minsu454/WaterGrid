using UnityEditor;
using Common.SceneEx;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class StartScene
{
    static StartScene()
    {
        EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>($"{EditorPath.StartScenePath}");
    }
}