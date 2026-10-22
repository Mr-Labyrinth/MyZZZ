using System.Collections.Generic;
using UnityEngine;

public class AssetManager : SingleMomoBase<AssetManager>
{
    private Dictionary<string, UnityEngine.Object> loadedAssets = new();

    [SerializeField]
    private AseetConfig initAssetConfig;

    protected override void Awake()
    {
        base.Awake();
    }

    private void InitLoadAsset()
    {
        foreach (var assetConfig in initAssetConfig.AssetConfigs)
        {
            AssetLoader.Load<UnityEngine.Object>(assetConfig.assetPath, out var asset);
            loadedAssets[assetConfig.assetName] = asset;
        }
    }
}
