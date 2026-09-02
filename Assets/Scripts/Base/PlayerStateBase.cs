using UnityEngine;

public enum PlayerState
{
    Idle, Idle_AFK,
    Walk, Run, RunEnd, TurnBack,
    Evade_Front, Evade_Back, Evade_Front_End, Evade_Back_End,
    NormalAttack, NormalAttack_End,
    BigSkillStart, BigSkill, BigSkill_End,
    SwitchInNormal
}

public class PlayerStateBase : StateBase
{
    //玩家控制器
    protected PlayerController playerController;
    // 玩家模型
    protected PlayerModel playerModel;
    //动画信息
    private AnimatorStateInfo stateInfo;
    //记录当前状态进入的时间
    protected float statePlayTime = 0;

    public override void Init(IStateMachineOwner owner)
    {
        playerController = (PlayerController)owner;
        playerModel = playerController.playerModel;
    }

    public override void Enter()
    {
        statePlayTime = 0;
    }

    public override void Exit()
    {
    }

    public override void FixedUpdate()
    {
        //施加重力影响
        playerModel.characterController.Move(new Vector3(0, playerModel.gravity * Time.fixedDeltaTime, 0));
        //刷新动画状态
        stateInfo = playerModel.animator.GetCurrentAnimatorStateInfo(0);
    }

    public override void LateUpdate()
    {
    }

    public override void UnInit()
    {
    }

    public override void Update()
    {

    }
}
