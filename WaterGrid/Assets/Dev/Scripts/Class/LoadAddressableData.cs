using System;
using UnityEngine;

/// <summary>
/// Load 데이터
/// </summary>
[Serializable]
public class LoadAddressableData
{
    [field: SerializeField] public string path { get; private set; }
    [field: SerializeField] public UnityEngine.Object setObject { get; private set; }
}
