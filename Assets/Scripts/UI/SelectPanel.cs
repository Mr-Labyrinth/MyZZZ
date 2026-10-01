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
    private Button exitButton;

    private void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();

        pages = new GameObject[uiConfig.uiConfigDatas.Length];

        exitButton = GetComponentInChildren<Button>();
        exitButton.onClick.AddListener(OnExitButtonClicked);

        // 根据UIConfig中的配置加载页面预制体并实例化
        for (int i = 0; i < uiConfig.uiConfigDatas.Length; i++)
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
                Debug.LogError("资源加载失败："+ name + " :\n " + path);
            }
        }
    }

    private void Start()
    {
        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        for(int i = 0; i < toggles.Length; i++)
        {
            int index = i;// 防止闭包问题
            // 为每个Toggle添加监听器，当Toggle被选中时切换到对应的页面
            toggles[index].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    switchPage(index);
                }
            });
        }
        // 默认选中第一个Toggle并显示对应的页面
        if (toggles.Length > 0)
        {
            toggles[0].isOn = true;
            switchPage(0);
        }
    }

    // 切换页面的方法
    private void switchPage(int index)
    {
        for(int i = 0; i < pages.Length; i++)
        {
            pages[i]?.SetActive(i == index);
        }
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
