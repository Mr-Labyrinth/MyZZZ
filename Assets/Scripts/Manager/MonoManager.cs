using System;
using UnityEngine;

public class MonoManager : SingleMomoBase<MonoManager>
{
    // update 任务
    public Action updateAction;
    // fixedUpdate 任务
    public Action fixedUpdateAction;
    // lateUpdate 任务
    public Action lateUpdateAction;

    /// <summary>
    /// 添加update任务
    /// </summary>
    /// <param name="task"></param>
    public void AddUpdateAction(Action task)
    {
        updateAction += task;
    }

    /// <summary>
    /// 移除update任务
    /// </summary>
    /// <param name="task"></param>
    public void RemoveUpdateAction(Action task)
    {
        updateAction -= task;
    }

    /// <summary>
    /// 添加fixedUpdate任务
    /// </summary>
    /// <param name="task"></param>
    public void AddFixedUpdateAction(Action task)
    {
        fixedUpdateAction += task;
    }

    /// <summary>
    /// 移除fixedUpdate任务
    /// </summary>
    /// <param name="task"></param>
    public void RemoveFixedUpdateAction(Action task)
    {
        fixedUpdateAction -= task;
    }

    /// <summary>
    /// 添加lateUpdate任务
    /// </summary>
    /// <param name="task"></param>
    public void AddLateUpdateAction(Action task)
    {
        lateUpdateAction += task;
    }

    /// <summary>
    /// 移除lateUpdate任务
    /// </summary>
    /// <param name="task"></param>
    public void RemoveLateUpdateAction(Action task)
    {
        lateUpdateAction -= task;
    }

    void Update()
    {
        updateAction?.Invoke();
    }

    void FixedUpdate()
    {
        fixedUpdateAction?.Invoke();
    }

    void LateUpdate()
    {
        lateUpdateAction?.Invoke();
    }
}
