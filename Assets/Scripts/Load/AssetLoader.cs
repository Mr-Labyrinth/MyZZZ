using System;

public class AssetLoader
{
    private static readonly Lazy<AssetLoader> _instance = 
        new Lazy<AssetLoader>(() => new AssetLoader());
    public static AssetLoader Instance => _instance.Value;

    private readonly Lazy<IAssetLoader> _lazyLoader = 
        new Lazy<IAssetLoader>(() => new ResourcesLoader());

    private IAssetLoader _current;

    public IAssetLoader Current
    {
        get => _current ??= _lazyLoader.Value;
        set => _current = value;
    }


    private AssetLoader() {}

    public bool Load<T>(string path, out T asset) where T : UnityEngine.Object
        => Current.LoadSync(path, out asset);

    public void LoadAsync<T>(string path, System.Action<T> onLoaded) where T : UnityEngine.Object
        => Current.LoadAsync(path, onLoaded);

    public void Release(UnityEngine.Object asset) => Current.Release(asset);
}