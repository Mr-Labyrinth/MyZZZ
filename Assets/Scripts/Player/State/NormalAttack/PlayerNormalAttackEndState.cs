using UnityEngine;

public class PlayerNormalAttackEndState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        playerController.
            PlayAnimation($"Attack_Normal_{playerModel.skillConfig.currentNormalAttackIndex}_End", 1.0f);
    }

    public override void Update()
    {
        base.Update();

        #region 检测大招
        if (playerController.inputSystem.Player.BigSkill.triggered)
        {
            //切换到进入大招状态
            playerController.SwitchState(PlayerState.BigSkillStart);
            return;
        }
        #endregion

        #region 检测普通攻击
        if (playerController.inputSystem.Player.Fire.triggered)
        {
            //攻击段数累加
            playerModel.skillConfig.currentNormalAttackIndex++;
            if (playerModel.skillConfig.currentNormalAttackIndex > playerModel.skillConfig.normalAttackDamageMulitple.Length)
            {
                //如果超过了最大段数，则重置为第一段
                playerModel.skillConfig.currentNormalAttackIndex = 1;
            }
            //切换到普通攻击状态
            playerController.SwitchState(PlayerState.NormalAttack);
            return;
        }
        #endregion

        #region 检测移动
        if (playerController.inputMoveVec2 != Vector2.zero)
        {
            //普攻段数归零
            playerModel.skillConfig.currentNormalAttackIndex = 1;
            //切换到移动状态
            playerController.SwitchState(PlayerState.Walk);
            return;
        }
        #endregion

        #region 检测闪避
        if (playerController.inputSystem.Player.Evade.triggered)
        {
            //普攻段数归零
            playerModel.skillConfig.currentNormalAttackIndex = 1;
            //切换到闪避状态
            playerController.SwitchState(PlayerState.Evade_Back);
            return;
        }
        #endregion

        #region 动画是否播放完毕
        if(IsAnimationEnd())
        {
            //切换到待机状态
            playerController.SwitchState(PlayerState.Idle);
            //当前机动段数归零
            playerModel.skillConfig.currentNormalAttackIndex = 1;
            return;
        }
        #endregion
    }
}
