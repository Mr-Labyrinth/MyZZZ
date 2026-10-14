using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using YooAsset;

public class YooAssetLoader : IAssetLoader
{
    private readonly string _packageName;
    private readonly Dictionary<UnityEngine.Object, AssetHandle> _handles = new();

    public YooAssetLoader(string packageName = "DefaultPackage")
    {
        _packageName = packageName;
    }

    private ResourcePackage Package => YooAssets.GetPackage(_packageName);
    public void LoadAsync<T>(string path, Action<T> onLoaded) where T : UnityEngine.Object
    {
        var handle = Package.LoadAssetAsync<T>(path);
        handle.Completed += h =>
        {
            if (h.Status == EOperationStatus.Succeeded)
            {
                var asset = h.AssetObject as T;
                _handles[asset] = handle;
                onLoaded?.Invoke(asset);
            }
            Debug.LogError($"LoadAsync 失败： {path},{h.Error}");
            onLoaded?.Invoke(null);
        };
    }

    public bool LoadSync<T>(string path, out T asset) where T : UnityEngine.Object
    {
        var handle = Package.LoadAssetSync<T>(path);
        if(handle.Status == EOperationStatus.Succeeded)
        {
            asset = handle.AssetObject as T;
            _handles[asset] = handle;
            return true;
        }

        Debug.LogError($"LoadSync 失败： {path},{handle.Error}");
        asset = null;
        return false;
    }

    public void Release(UnityEngine.Object asset)
    {
        if (asset is null) return;
        if(_handles.TryGetValue(asset, out var handle))
        {
            handle.Release();
            _handles.Remove(asset);
        }
        else
        {
            Debug.LogWarning($"Release 失败： {asset.name} 没有找到对应的 AssetHandle");
        }
    }
}
