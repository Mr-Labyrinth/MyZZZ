using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string PartyPath =>
        Path.Combine(Application.persistentDataPath, "party.json");

    public static void SaveParty(PartySaveData data)
    {
        string josn = JsonUtility.ToJson(data);
        File.WriteAllText(PartyPath, josn);
    }

    public static PartySaveData LoadParty()
    {
        if (!File.Exists(PartyPath))
        {
            return new PartySaveData();
        }

        string json = File.ReadAllText(PartyPath);
        return JsonUtility.FromJson<PartySaveData>(json);
    }
}
