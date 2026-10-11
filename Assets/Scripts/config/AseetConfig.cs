using System;
using UnityEngine;

[Serializable]
public struct AssetConfigData
{
    public string assetName;
    public string assetPath;
}

[CreateAssetMenu(menuName = "Config/Asset Config")]
public class AseetConfig : ScriptableObject
{
    [SerializeField]
    public AssetConfigData[] assetConfigs;

    public AssetConfigData[] AssetConfigs => assetConfigs;
}
