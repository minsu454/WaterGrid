using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using static Codice.Client.BaseCommands.BranchExplorer.ExplorerData.BrExTreeBuilder.BrExFilter;
using Object = UnityEngine.Object;

public static class GUIParts
{
    /// <summary>
    /// UI들 가로로 만들어주는 함수
    /// </summary>
    public static void CreateHorizontal(params Action[] content)
    {
        GUILayout.BeginHorizontal();

        for (int i = 0; i < content.Length; i++)
            content[i].Invoke();

        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 에리어 만들어주는 함수
    /// </summary>
    public static void CreateArea(Rect rect, Color color,  params Action<Rect>[] content)
    {
        GUI.color = color; 
        GUI.Box(rect, GUIContent.none);

        GUI.color = Color.white; // 기본 색상 복원

        GUILayout.BeginArea(rect, GUIStyle.none);

        for (int i = 0; i < content.Length; i++)
            content[i].Invoke(rect);

        GUILayout.EndArea();
    }

    /// <summary>
    /// 경로에 있는 모든 파일들 반환하는 함수
    /// </summary>
    public static void LoadAllInFolder<T, U>(string folderPath, out Dictionary<T, U> dict) where T : Enum where U : Object
    {
        string[] guidArr = AssetDatabase.FindAssets($"t:{typeof(U).Name}", new[] { folderPath });
        Dictionary<T, U> returnDict = new Dictionary<T, U>();

        foreach (string guid in guidArr)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            U u = AssetDatabase.LoadAssetAtPath<U>(assetPath);

            string assetName = Path.GetFileNameWithoutExtension(assetPath);
            if (!Enum.IsDefined(typeof(T), assetName)) continue;

            T t = (T)Enum.Parse(typeof(T), assetName, true);

            returnDict.Add(t, u);
        }

        dict = returnDict;
    }

    public static void DrawIcon(Vector2 center, Texture icon)
    {
        Handles.BeginGUI();
        Vector2 screen = HandleUtility.WorldToGUIPoint(center);
        float iconSize = 24f;
        GUI.DrawTexture(
            new Rect(
                screen.x - iconSize / 2f,
                screen.y - iconSize / 2f,
                iconSize,
                iconSize
            ),
            icon
        );
        Handles.EndGUI();
    }

}