using UnityEngine;

public interface IAssetLoader
{
    bool LoadSync<T>(string path, out T asset) where T : Object;
    void LoadAsync<T>(string path, System.Action<T> onLoaded) where T : Object;
    void Release(Object asset);
}