using System;

public static class AssetLoader
{
    private static IAssetLoader _current = new ResourcesLoader();

    public static IAssetLoader Current
    {
        get => _current;
    }

    public static bool Load<T>(string path, out T asset) where T : UnityEngine.Object
        => Current.LoadSync(path, out asset);

    public static void LoadAsync<T>(string path, System.Action<T> onLoaded) where T : UnityEngine.Object
        => Current.LoadAsync(path, onLoaded);

    public static void Release(UnityEngine.Object asset) => Current.Release(asset);
}