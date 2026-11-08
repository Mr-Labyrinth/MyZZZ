using UnityEngine;

public class ParallelNode : CompositeNode
{
    public enum ParallelMode
    {
        FirstFailure,
        FirstSuccess,
        FirstSuccessOrFailure
    }

    public ParallelMode parallelMode = ParallelMode.FirstFailure;

    [HideInInspector] private bool[] isDone;
    private int targetCount;

    protected override void OnStart()
    {
        isDone = new bool[children.Count];
        targetCount = 0;                          // ★ 必须归零
    }

    protected override void OnStop()
    {
        if (isDone == null) return;
        for (int i = 0; i < children.Count; i++)
            if (!isDone[i]) children[i]?.Abort(); // ★ 只中断未完成的
        isDone = null;
    }

    protected override State OnUpdate()
    {
        switch (parallelMode)
        {
            case ParallelMode.FirstSuccess: return UpdateFirstSuccess();
            case ParallelMode.FirstFailure: return UpdateFirstFailure();
            case ParallelMode.FirstSuccessOrFailure: return UpdateRace();
            default: return State.Running;
        }
    }

    private State UpdateFirstSuccess()
    {
        for (int i = 0; i < children.Count; i++)
        {
            if (isDone[i]) continue;
            var s = children[i].Update();
            if (s == State.Running) continue;
            isDone[i] = true;
            if (s == State.Success) return State.Success;
            targetCount++;                        // 失败计数
        }
        return targetCount == children.Count ? State.Failure : State.Running;
    }

    private State UpdateFirstFailure()
    {
        for (int i = 0; i < children.Count; i++)
        {
            if (isDone[i]) continue;
            var s = children[i].Update();
            if (s == State.Running) continue;
            isDone[i] = true;
            if (s == State.Failure) return State.Failure;
            targetCount++;                        // 成功计数
        }
        return targetCount == children.Count ? State.Success : State.Running;
    }

    private State UpdateRace()
    {
        for (int i = 0; i < children.Count; i++)
        {
            var s = children[i].Update();
            if (s != State.Running) return s;     // ★ 第一个终态立即返回
        }
        return State.Running;
    }
}