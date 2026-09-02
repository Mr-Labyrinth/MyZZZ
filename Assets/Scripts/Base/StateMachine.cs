using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 宿主标记
/// </summary>
public interface IStateMachineOwner { }

/// <summary>
/// 状态机
/// </summary>
public class StateMachine
{
    // 当前状态
    private StateBase currentState;

    // 是否包含当前状态
    public bool HasState { get => currentState != null; }

    // 宿主
    private IStateMachineOwner owner;

    private Dictionary<Type, StateBase> stateDic = new Dictionary<Type, StateBase>();

    public StateMachine(IStateMachineOwner owner)
    {
        Init(owner);
    }

    public void Init(IStateMachineOwner owner)
    {
        this.owner = owner;
    }

    /// <summary>
    /// 进入状态
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="reLoadState"></param>
    public void EnterState<T>(bool reLoadState = false) where T : StateBase, new()
    {
        if (HasState && currentState.GetType() == typeof(T) && !reLoadState)
        {
            return;
        }

        #region 结束当前状态
        if (HasState)
        {
            ExitCurrentState();
        }
        #endregion

        #region 进入新状态
        currentState = LoadState<T>();
        EnterCurrentState();
        #endregion
    }

    private StateBase LoadState<T>() where T : StateBase, new()
    {
        // 获取状态类型
        Type statetype = typeof(T);

        // 如果状态不存在，则创建并初始化
        if (!stateDic.TryGetValue(statetype, out StateBase state))
        {
            state = new T();
            state.Init(owner);
            stateDic.Add(statetype, state);
        }

        return state;
    }

    /// <summary>
    /// 进入当前状态
    /// </summary>
    private void EnterCurrentState()
    {
        currentState.Enter();
        MonoManager.INSTANCE.AddUpdateAction(currentState.Update);
        MonoManager.INSTANCE.AddFixedUpdateAction(currentState.FixedUpdate);
        MonoManager.INSTANCE.AddLateUpdateAction(currentState.LateUpdate);
    }

    /// <summary>
    /// 退出当前状态
    /// </summary>
    private void ExitCurrentState()
    {
        currentState.Exit();
        MonoManager.INSTANCE.RemoveUpdateAction(currentState.Update);
        MonoManager.INSTANCE.RemoveFixedUpdateAction(currentState.FixedUpdate);
        MonoManager.INSTANCE.RemoveLateUpdateAction(currentState.LateUpdate);
    }

    /// <summary>
    /// 刷新状态机，清除所有状态
    /// </summary>
    public void Clear()
    {
        ExitCurrentState();
        currentState = null;
        foreach (var state in stateDic.Values)
        {
            state.UnInit();
        }
        stateDic.Clear();
    }
}
