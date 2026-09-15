using System;
using UnityEngine;

[Serializable]
public struct UIConfigData
{
    public string uiName;
    public string uiPath;
}

[CreateAssetMenu(fileName ="UIConfig", menuName = "Config/UI Config")]
public class UIConfig : ScriptableObject
{
    public UIConfigData[] uiConfigDatas;

    public bool TryGet(string uiName, out UIConfigData data)
    {
        foreach (var item in uiConfigDatas)
        {
            if (item.uiName == uiName)
            {
                data = item;
                return true;
            }
        }
        data = default;
        return false;
    }
}
