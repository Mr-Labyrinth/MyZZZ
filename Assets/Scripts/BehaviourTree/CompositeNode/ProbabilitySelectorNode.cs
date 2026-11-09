using System.Collections.Generic;
using UnityEngine;

public class ProbabilitySelectorNode : CompositeNode
{
    [Tooltip("与子节点一一对应的权重，权重越大被选中概率越高;")]
    public List<float> weights = new List<float>();

    private int selectedIndex = -1;

    protected override void OnStart()
    {
        
        selectedIndex = PickIndex();  // ★ 进入时选一次
    }

    protected override void OnStop()
    {
        for (int i = 0; i < children.Count; i++)
            children[i]?.Abort();     // 中断所有子节点（只有选中的在跑，其余 Abort 是空转）
        selectedIndex = -1;
    }

    protected override State OnUpdate()
    {
        if (selectedIndex < 0 || selectedIndex >= children.Count)
            return State.Failure;     // 所有权重为 0 / 无子节点

        return children[selectedIndex].Update();   // 只 tick 选中的
    }

    public override Node Clone()
    {
        var clone = base.Clone() as ProbabilitySelectorNode;
        clone.weights = new List<float>(weights);  // 复制权重，避免共享
        clone.selectedIndex = -1;
        return clone;
    }

    private int PickIndex()
    {
        float total = 0f;
        for (int i = 0; i < children.Count; i++)
            if (weights[i] > 0f) total += weights[i];

        if (total <= 0f)
        {
            Debug.LogWarning($"{name}: 所有子节点权重为 0，本次返回 Failure");
            return -1;
        }

        float r = Random.value * total;
        for (int i = 0; i < children.Count; i++)
        {
            if (weights[i] <= 0f) continue;
            r -= weights[i];
            if (r <= 0f) return i;
        }

        // 浮点误差兜底：返回最后一个权重 > 0 的
        for (int i = children.Count - 1; i >= 0; i--)
            if (weights[i] > 0f) return i;
        return -1;
    }
}