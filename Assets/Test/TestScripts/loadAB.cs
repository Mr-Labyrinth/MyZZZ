using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadAB : MonoBehaviour
{
    [SerializeField]
    private AssetReference lumine;

    private GameObject lumineObj;
    private AsyncOperationHandle<GameObject> handle;

    void Start()
    {
        handle = lumine.InstantiateAsync();
        handle.Completed += OnLumineLoaded;
    }

    private void OnLumineLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            lumineObj = handle.Result;
            Renderer[] renderers = lumineObj.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.materials;
                foreach(Material material in materials)
                {
                    material.shader = Shader.Find(material.shader.name);
                }
            }
        }
        else
        {
            Debug.LogError("º”‘ÿ Lumine  ß∞‹");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnDestroy()
    {
        if (handle.IsValid())
            Addressables.Release(handle);
    }
}