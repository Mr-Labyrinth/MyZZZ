using UnityEngine;
using UnityEngine.UI;

public class SelectPanel : MonoBehaviour
{
    private ToggleGroup toggleGroup;
    [SerializeField]
    private UIConfig uiConfig;
    private GameObject[] pages;
    [SerializeField]
    private Transform pageRoot;

    private void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();

        pages = new GameObject[uiConfig.uiConfigDatas.Length];

        for(int i = 0; i < uiConfig.uiConfigDatas.Length; i++)
        {
            GameObject page;
            string name = uiConfig.uiConfigDatas[i].uiName;
            string path = uiConfig.uiConfigDatas[i].uiPath;
            if (AssetLoader.Load<GameObject>(uiConfig.uiConfigDatas[i].uiPath, out page))
            {
                pages[i] = Instantiate(page, pageRoot);
                pages[i].SetActive(false);
            }
            else
            {
                Debug.LogError("◊ ‘¥º”‘ÿ ß∞‹£∫"+ name + " :\n " + path);
            }
        }
    }

    private void Start()
    {
        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        for(int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[index].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    switchPage(index);
                }
            });
        }
        if(toggles.Length > 0)
        {
            toggles[0].isOn = true;
            switchPage(0);
        }
    }

    private void switchPage(int index)
    {
        for(int i = 0; i < pages.Length; i++)
        {
            pages[i]?.SetActive(i == index);
        }
    }
}
