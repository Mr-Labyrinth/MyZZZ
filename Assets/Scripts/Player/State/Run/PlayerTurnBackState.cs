using UnityEngine;

/// <summary>
/// 玩家180度转身状态
/// </summary>
public class PlayerTurnBackState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        playerController.PlayAnimation("TurnBack", 0.1f);
    }

    public override void Update()
    {
        base.Update();

        #region 检查大招
        if (playerController.inputSystem.Player.BigSkill.triggered)
        {
            //切换到进入大招状态
            playerController.SwitchState(PlayerState.BigSkillStart);
            return;
        }
        #endregion

        #region 动画播放完毕后切换到跑步状态
        if (IsAnimationEnd())
        {
            playerController.SwitchState(PlayerState.Run);
            return;
        }
        #endregion
    }
}
