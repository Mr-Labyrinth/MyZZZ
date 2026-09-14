using NUnit.Framework;
using UnityEngine;

public struct UIConfigData
{
    public string uiName;
    public string uiPath;
    public GameObject ui;
}

public class UIConfig : ScriptableObject
{
    public UIConfigData[] uIConfigDatas;
}
