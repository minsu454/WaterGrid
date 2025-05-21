using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Common.Hexagon;

public class HexMapEditorWindow : CustomWindow<HexMapEditorWindow>
{
    private float tileSize = 0.5f;          //타일 사이즈
    private int gridWidth = 10;             //가로 갯수
    private int gridHeight = 10;            //세로 갯수

    private Dictionary<Vector2Int, float> _hexDict = new();
    private float fillSlider = 100f;

    [MenuItem("Tools/MapEditor/Create", priority = 1)]
    static void Init()
    {
        CreateComstomWindow("Hex Map Editor", new Vector2(800f, 580f), new Vector2(800f, 580f));
    }

    protected override void Run()
    {
        base.Run();
        controller.leftMouseDownEvent += OnLeftClick;
    }

    protected override void Stop()
    {
        controller.leftMouseDownEvent -= OnLeftClick;
        base.Stop();
    }

    private void OnGUI()
    {
        GUILayout.Label("Hex Grid Settings", EditorStyles.boldLabel);
        gridWidth = EditorGUILayout.IntField("Width", gridWidth);
        gridHeight = EditorGUILayout.IntField("Height", gridHeight);

        GUILayout.Space(10);
        GUILayout.Label("Tile Percentage Fill", EditorStyles.boldLabel);
        fillSlider = EditorGUILayout.Slider("Fill %", fillSlider, 0f, 100f);
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
                //HexUtility.DrawHex2D(worldPos, tileSize, coord, );
            }
        }
    }

    private void OnLeftClick(Vector3 screenPos)
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
                if (HexUtility.PointInHex(worldPos, world, tileSize))
                {
                    _hexDict[new Vector2Int(qr, rr)] = fillSlider;
                    return;
                }
            }
        }
    }
}
