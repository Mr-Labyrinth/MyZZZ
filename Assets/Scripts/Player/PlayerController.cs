using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : SingleMomoBase<PlayerController>,IStateMachineOwner
{
    //输入系统
    [HideInInspector]public InputSystem inputSystem;
    //玩家移动输入
    public Vector2 inputMoveVec2;
    
    //玩家模型
    public PlayerModel playerModel;

    //转向速度
    public float rotationSpeed = 8f;

    //闪避计时器
    private float evadeTimer = 1;

    //状态机
    private StateMachine stateMachine;

    //玩家配置信息
    public PlayerConfig playerConfig;

    //配队
    private List<PlayerModel> controllableModels;

    //当前角色的下标
    private int currentModelIndex;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new StateMachine(this);
        inputSystem = new InputSystem();

        controllableModels = new List<PlayerModel>();

        #region 生成角色模型
        for(int i=0; i < playerConfig.models.Length; i++)
        {
            GameObject modle = Instantiate(playerConfig.models[i], transform);
            controllableModels.Add(modle.GetComponent<PlayerModel>());
            controllableModels[i].gameObject.SetActive(false);
        }
        #endregion

        #region 控制第一个角色
        currentModelIndex = 0;
        controllableModels[currentModelIndex].gameObject.SetActive(true);
        playerModel = controllableModels[currentModelIndex];
        #endregion
    }

    private void Start()
    {
        //锁定光标
        LockMouse();
        //切换到待机状态
        SwitchState(PlayerState.Idle);
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="playerState">状态</param>
    public void SwitchState(PlayerState playerState)
    {
        playerModel.currentState = playerState;
        switch (playerState)
        {
            //    case PlayerState.Idle:
            //    case PlayerState.Idle_AFK:
            //        stateMachine.EnterState<>
        }
    }

    private void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }
    private void OnDisable()
    {
        inputSystem.Disable();
    }
}
