using UnityEditor;
using UnityEngine;

public abstract class CustomWindow<T> : EditorWindow where T : EditorWindow
{
    protected static T window;
    

    private void OnEnable()
    {
        Run();
    }

    /// <summary>
    /// 프레임마다 주기적으로 Scene UI 업데이트 해야하는 것들 함수
    /// </summary>
    protected abstract void OnSceneGUI(SceneView sceneView);

    /// <summary>
    /// OnSceneGUI 실행시켜주는 함수
    /// </summary>
    protected virtual void Run()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    /// <summary>
    /// OnSceneGUI 멈추는 함수
    /// </summary>
    protected virtual void Stop()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    /// <summary>
    /// ComstomWindow 생성 함수
    /// </summary>
    protected static void CreateComstomWindow(string name, Vector2 minSize, Vector2 maxSize)
    {
        if (window != null)
        {
            return;
        }

        window = GetWindow<T>(name);

        //// 최소, 최대 크기 지정
        window.minSize = minSize;
        window.maxSize = maxSize;

        window.Show();
    }

    private void OnDisable()
    {
        Stop();
    }
}
