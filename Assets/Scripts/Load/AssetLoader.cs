using UnityEngine;
using UnityEngine.Rendering;

public static class AssetLoader
{
    private static IAssetLoader _current;

    public static IAssetLoader Current
    {
        get => _current ??= new ResourcesLoader();
        set => _current = value;
    }

    public static bool Load<T>(string path, out T asset) where T : Object
        => Current.LoadSync(path, out asset);

    public static void LoadAsync<T>(string path, System.Action<T> onLoaded) where T : Object
        => Current.LoadAsync(path, onLoaded);

    public static void Release(Object asset) => Current.Release(asset);
}