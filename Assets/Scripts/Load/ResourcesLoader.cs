using UnityEngine;

public class ResourcesLoader : IAssetLoader
{
    public bool LoadSync<T>(string path, out T asset) where T : Object
    {
        asset = Resources.Load<T>(path);
        return asset != null;
    }

    public void LoadAsync<T>(string path, System.Action<T> onLoaded) where T : Object
    {
        var request = Resources.LoadAsync<T>(path);
        request.completed += _ => onLoaded(request.asset as T);
    }

    public void Release(Object asset)
    {
        // Resources 用 Resources.UnloadUnusedAssets() 统一释放
    }
}