using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Common.Hexagon;
using System;
using System.IO;

public class HexMapEditorWindow : CustomWindow<HexMapEditorWindow>
{
    private float tileSize = 0.5f;          //타일 사이즈
    private int gridWidth = 10;             //가로 갯수
    private int gridHeight = 10;            //세로 갯수

    private int curWeight = 0;              //현재 가중치
    private int maxOneWeight = 1000;        //한 칸에 최대 가중치

    private readonly Dictionary<Vector2Int, (int weight, TileType type)> _hexDict = new();
    private int weightSlider = 1;

    private int houseCount = 0;

    private enum ToolMode { Fill, Brush, Erase }
    private ToolMode toolMode = ToolMode.Fill;
    
    private TileType tileType = TileType.None;
    private static LoadMapData loadMapData;
    private Dictionary<TileType, Texture2D> _texture2DArr = new();

    [MenuItem("Tools/MapEditor/Create", priority = 1)]
    static void Create()
    {
        CreateComstomWindow("Hex Map Editor", new Vector2(600f, 580f), new Vector2(600f, 580f));
    }

    [MenuItem("Tools/MapEditor/Load", priority = 2)]
    static void Load()
    {
        string path = EditorUtility.OpenFilePanel("Open File", "", "json");

        if (SaveManager.Load(path, out loadMapData) is false)
        {
            EditorUtility.DisplayDialog(
            "Load Failed",                          
            "Failed to load JSON from selected file.", 
            "OK"                                    
            );
            return;
        }

        CreateComstomWindow("Hex Map Editor", new Vector2(600f, 580f), new Vector2(600f, 580f));
    }

    protected override void Run()
    {
        base.Run();

        controller.leftMouseDownEvent += OnLeftDrag;
        controller.leftMouseDragEvent += OnLeftDrag;
        controller.rightMouseDownEvent += OnRightDrag;
        controller.rightMouseDragEvent += OnRightDrag;

        curWeight = 0;
        LoadData();

        GUIParts.LoadAllInFolder(EditorPath.TexturePath, out _texture2DArr);
    }

    protected override void Stop()
    {
        controller.leftMouseDownEvent -= OnLeftDrag;
        controller.leftMouseDragEvent -= OnLeftDrag;
        controller.rightMouseDownEvent -= OnRightDrag;
        controller.rightMouseDragEvent -= OnRightDrag;

        _texture2DArr = null;

        base.Stop();
    }

    #region GUI

    private void OnGUI()
    {
        GUILayout.Label("Tilemap Size", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Width", GUILayout.Width(50));
        gridWidth = EditorGUILayout.IntField(gridWidth, GUILayout.Width(60));
        GUILayout.Label("Height", GUILayout.Width(50));
        gridHeight = EditorGUILayout.IntField(gridHeight, GUILayout.Width(60));
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Save", GUILayout.Width(100), GUILayout.Height(20)))
        {
            string initialFilename = "Map_" + DateTime.Now.ToString(("MM_dd_HH_mm")) + ".json";
            string path = EditorUtility.SaveFilePanel("Save File", "", initialFilename, "json");
            SaveData(path);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.Label("Setting", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();

        GUILayout.Label("Tool", GUILayout.Width(40));

        if (GUILayout.Toggle(toolMode == ToolMode.Fill, EditorGUIUtility.IconContent("d_Grid.FillTool"), "Button", GUILayout.Width(30)))
        {
            toolMode = ToolMode.Fill;
            tileType = TileType.None;
        }

        if (GUILayout.Toggle(toolMode == ToolMode.Brush, EditorGUIUtility.IconContent("d_editicon.sml"), "Button", GUILayout.Width(30)))
            toolMode = ToolMode.Brush;

        if (GUILayout.Toggle(toolMode == ToolMode.Erase, EditorGUIUtility.IconContent("d_Grid.EraserTool"), "Button", GUILayout.Width(30)))
        {
            toolMode = ToolMode.Erase;
            tileType = TileType.None;
        }

        GUILayout.Space(20);

        if (toolMode == ToolMode.Brush)
        {
            GUILayout.Label("Tile Type", GUILayout.Width(60));
            tileType = (TileType)EditorGUILayout.EnumPopup(tileType, GUILayout.Width(100));

            if (tileType == TileType.House)
            {
                GUILayout.Label("Count", GUILayout.Width(45));
                houseCount = EditorGUILayout.IntField(houseCount, GUILayout.Width(50));
            }
        }

        if (toolMode == ToolMode.Fill)
        {
            GUILayout.Label("Weight", GUILayout.Width(50));
            weightSlider = EditorGUILayout.IntSlider(weightSlider, 1, maxOneWeight, GUILayout.Width(230));
        }

        GUILayout.EndHorizontal();
    }

    protected override void OnSceneGUI(SceneView sceneView)
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        Event e = controller.GetEvent();
        controller.InputMouse(e);

        int qOffset = -gridWidth / 2;
        int rOffset = -gridHeight / 2;

        for (int r = 0; r < gridHeight; r++)
        {
            for (int q = 0; q < gridWidth; q++)
            {
                int qr = q + qOffset;
                int rr = r + rOffset;

                Vector2 worldPos = HexUtility.HexToWorld2D(qr, rr, tileSize);
                Vector2Int coord = new Vector2Int(qr, rr);
                bool hasValue = _hexDict.ContainsKey(coord);
                int weight = hasValue ? _hexDict[coord].weight : -1;
                float percent = hasValue ? (float)weight / curWeight * 100f : 0;
                //Debug.Log($"percent {(float)weight / curWeight * 100f}");

                float alpha = Mathf.Clamp01(percent / 100f);
                Color color = new Color(0f, 1f, 0f, alpha);

                HexUtility.DrawHex2D(worldPos, tileSize, hasValue, percent, color);

                if (hasValue && _hexDict[coord].type != TileType.None)
                    GUIParts.DrawIcon(worldPos, _texture2DArr[_hexDict[coord].type]);
            }
        }
    }

    #endregion

    #region Save

    /// <summary>
    /// 저장 함수
    /// </summary>
    private void SaveData(string path)
    {
        if (loadMapData == null)
            loadMapData = new LoadMapData();

        List<TileData> tileDataList = new();

        foreach (var pair in _hexDict)
        {
            TileData data = new TileData(pair);
            tileDataList.Add(data);
        }

        loadMapData.TileDataList = tileDataList;
        loadMapData.Name = Path.GetFileNameWithoutExtension(path);
        loadMapData.Width = gridWidth;
        loadMapData.Height = gridHeight;
        loadMapData.Weight = curWeight;
        string json = JsonUtility.ToJson(loadMapData);

        SaveManager.Save(path, json);
    }

    /// <summary>
    /// 로드 함수
    /// </summary>
    private void LoadData()
    {
        if (loadMapData == null)
            return;

        foreach (var data in loadMapData.TileDataList)
        {
            curWeight += data.Weight;
            _hexDict.Add(data.Position, new(data.Weight, data.TileType));
        }
        
        gridWidth = loadMapData.Width;
        gridHeight = loadMapData.Height;
    }

    #endregion

    #region ClickEvent

    /// <summary>
    /// 좌클릭 시 이벤트 함수
    /// </summary>
    private void OnLeftDrag(Vector3 screenPos)
    {
        switch (toolMode)
        {
            case ToolMode.Fill:
                DrawHexToWorld(screenPos);
                return;
            case ToolMode.Erase:
                EraseToWorld(screenPos);
                return;
        }

        switch (tileType)
        {
            case TileType.None:
                EraseTextureToWorld(screenPos);
                break;
            default:
                DrawTextureToWorld(screenPos);
                break;
        }
    }

    private void OnRightDrag(Vector3 screenPos)
    {
        FindHexagon(screenPos, (posInt) =>
        {
            if (_hexDict.TryGetValue(posInt, out var value))
            {
                _hexDict.Remove(posInt);
            }
        });
    }

    /// <summary>
    /// 클릭한 부분 색칠하는 함수
    /// </summary>
    private void DrawHexToWorld(Vector3 screenPos)
    {
        FindHexagon(screenPos, (posInt) =>
        {
            if (_hexDict.TryGetValue(posInt, out var value) is false)
            {
                _hexDict[posInt] = new(weightSlider, TileType.None);
                curWeight += weightSlider;
                return;
            }

            curWeight -= value.weight;
            value.weight = weightSlider;
            curWeight += weightSlider;
            _hexDict[posInt] = value;
        });
    }

    /// <summary>
    /// 클릭한 부분 색 지우는 함수
    /// </summary>
    private void EraseToWorld(Vector3 screenPos)
    {
        FindHexagon(screenPos, (posInt) =>
        {
            if (_hexDict.TryGetValue(posInt, out var value) && value.type == TileType.None)
            {
                curWeight -= value.weight;
                _hexDict.Remove(posInt);
            }
        });
    }

    /// <summary>
    /// 클릭한 부분 텍스쳐 그리는 함수
    /// </summary>
    private void DrawTextureToWorld(Vector3 screenPos)
    {
        FindHexagon(screenPos, (posInt) =>
        {
            if (_hexDict.TryGetValue(posInt, out var value) is false)
            {
                _hexDict[posInt] = new(0, tileType);
                return;
            }

            value.type = tileType;
            _hexDict[posInt] = value;
        });
    }

    /// <summary>
    /// 클릭한 부분 텍스쳐 지우는 함수
    /// </summary>
    private void EraseTextureToWorld(Vector3 screenPos)
    {
        FindHexagon(screenPos, (posInt) =>
        {
            if (_hexDict.TryGetValue(posInt, out var value))
            {
                if (value.weight == 0)
                    _hexDict.Remove(posInt);
                else
                {
                    value.type = TileType.None;
                    _hexDict[posInt] = value;
                }
            }
        });
    }

    /// <summary>
    /// 마우스 위치 헥사곤 찾는 함수
    /// </summary>
    private void FindHexagon(Vector3 screenPos, Action<Vector2Int> completed)
    {
        Vector2 mousePos = screenPos;
        mousePos.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePos.y;
        Vector3 world = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));

        int qOffset = -gridWidth / 2;
        int rOffset = -gridHeight / 2;

        for (int r = 0; r < gridHeight; r++)
        {
            for (int q = 0; q < gridWidth; q++)
            {
                int qr = q + qOffset;
                int rr = r + rOffset;

                Vector2 worldPos = HexUtility.HexToWorld2D(qr, rr, tileSize);
                if (HexUtility.ContainHex(worldPos, world, tileSize))
                {
                    completed?.Invoke(new Vector2Int(qr, rr));
                    return;
                }
            }
        }
    }

    #endregion
}
