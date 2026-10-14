using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "Configs/Character Config")]
public class CharacterConfig : ScriptableObject
{
    public string characterId;
    public string displayName;
    public string prefabPath;
    public int baseHp;
    public int baseAtk;
}
