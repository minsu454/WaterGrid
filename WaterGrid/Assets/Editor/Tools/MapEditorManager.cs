using UnityEngine;

public sealed class MapEditorManager
{
    public bool IsCreateData { get; private set; } = false;     //데이터 생성했는지 확인하는 bool 변수

    /// <summary>
    /// 맵 에디터 열기 함수
    /// </summary>
    public void LoadMapEditor()
    {
        SceneEditorManager.OpenTempScene(EditorPath.UseScenePath);
        IsCreateData = true;
    }

    /// <summary>
    /// 맵 에디터 삭제 함수
    /// </summary>
    public void LeaveMapEditor()
    {
        IsCreateData = false;
        SceneEditorManager.CloseTempScene();
    }

    /// <summary>
    /// 맵 에디터 열기 함수
    /// </summary>
    public void LoadMapEditor<T>(ref T builder) where T : Component
    {
        if (builder != null)
        {
            Debug.LogWarning("map has already been created.");
            return;
        }

        SceneEditorManager.OpenTempScene(EditorPath.UseScenePath);

        GameObject go = new GameObject("Map");
        builder = go.AddComponent<T>();

        IsCreateData = true;

        Debug.Log("Create Completed");
    }

    /// <summary>
    /// 맵 에디터 닫기 함수
    /// </summary>
    public void LeaveMapEditor<T>(ref T builder) where T : Component
    {
        if (builder == null)
        {
            Debug.LogWarning("map has already been deleted.");
            return;
        }

        Object.DestroyImmediate(builder);
        IsCreateData = false;

        SceneEditorManager.CloseTempScene();
        Debug.Log("Delete Completed");
    }
}


