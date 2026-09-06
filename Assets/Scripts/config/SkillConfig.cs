using UnityEngine;

[CreateAssetMenu(menuName ="Config/Skill")]
public class SkillConfig : ScriptableObject
{
    // ÆÕ¹¥¶ÎÊý
    [HideInInspector]public int currentNormalAttackIndex = 1;
    // ÆÕ¹¥ÉËº¦±¶ÂÊ
    public float[] normalAttackDamageMulitple;
}
