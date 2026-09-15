using NUnit.Framework;
using System;
using UnityEngine;

[Serializable]
public struct UIConfigData
{
    public string uiName;
    public string uiPath;
    public GameObject ui;
}

[CreateAssetMenu(menuName = "Config/UI Config")]
public class UIConfig : ScriptableObject
{
    public UIConfigData[] uIConfigDatas;
}
