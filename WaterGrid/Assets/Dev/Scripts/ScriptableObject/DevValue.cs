using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "DevSO", menuName = "ScriptableObject/Dev", order = 0)]
public class DevValue : ScriptableObject
{
    [Header("Time")]
    public float monthDuration = 10;
    [Header("Day")]
    public MonthType MonthType = MonthType.Jan;
    [Header("Item")]
    public int StartHammerCount = 2;
    public int StartPumpCount = 1;
    public int GetHammerCount = 2;
    public int GetPumpCount = 1;
    [Header("Upgrade")]
    public int HammerUpgradeCount = 3;
    [Header("Warning")]
    public float warningCount = 14;
}
