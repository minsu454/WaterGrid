using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourcesSO", menuName = "ScriptableObject/AddressableLoader", order = 1)]
public class ResourcesLoaderSO : ScriptableObject
{
    public List<LoadAddressableData> loadDataList;
}

