using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ParallelNode : CompositeNode
{
    public enum ParallelMode
    {
        FistFailure,
        FirstSuccess,
        FistSuccessOrFailure
    }

    public ParallelMode parallelMode = ParallelMode.FistFailure;


    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        foreach (var child in children)
        {
            child.Abort();
        }
    }

    protected override State OnUpdate()
    {
        State state = State.Running;
        State result = State.Running;
        int targetCount = 0;
        switch (parallelMode)
        {
            case ParallelMode.FirstSuccess:
                foreach (var child in children)
                {
                    state = child.Update();
                    if (state is State.Success)
                        result = State.Success;
                    else if (state is State.Failure)
                        targetCount++;
                }
                if (targetCount == children.Count) // 如果所有节点都失败了，返回失败
                    return State.Failure;
                else if (result is State.Success) // 如果有节点成功，返回成功
                    return State.Success;
                else // 否则返回运行中
                    return State.Running;

            case ParallelMode.FistFailure:
                foreach (var child in children)
                {
                    state = child.Update();
                    if (state is State.Failure)
                        result = State.Failure;
                    else if (state is State.Success)
                        targetCount++;
                }
                if (targetCount == children.Count) // 如果所有节点都成功了，返回成功
                    return State.Success;
                else if (result is State.Failure) // 如果有节点失败，返回失败
                    return State.Failure;
                else // 否则返回运行中
                    return State.Running;

            case ParallelMode.FistSuccessOrFailure:
                bool flag = true;
                foreach (var child in children)
                {
                    state = child.Update();
                    if ((state is not State.Running) && flag)// 如果有节点成功或失败，返回状态为该节点状态
                    {
                        result = state;
                        flag = false;
                    }
                }
                return result;
            default:
                return State.Running;
        }
    }
}
