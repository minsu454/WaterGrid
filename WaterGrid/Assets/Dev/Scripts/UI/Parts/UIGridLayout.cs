using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

[RequireComponent(typeof(GridLayoutGroup))]
public class UIGridLayout : MonoBehaviour
{
    [Header("Grid Layout Group")]
    private GridLayoutGroup gridLayout;

    [SerializeField] private PaddingData Padding;
    [SerializeField] private Vector2 CellSize;
    [SerializeField] private Vector2 Spacing;
    [SerializeField] private Axis axis;

    [Header("Button")]
    [SerializeField] private List<UIButton> _btnList = new();

    public void Init()
    {
        gridLayout = GetComponent<GridLayoutGroup>();

        gridLayout.padding = Padding.ToRectOffset();
        gridLayout.cellSize = CellSize;
        gridLayout.spacing = Spacing;
        gridLayout.startAxis = axis;
    }
}