using UnityEngine;

public class PlayerNormalAttackState : PlayerStateBase
{
    private bool enterNextAttack;

    public override void Enter()
    {
        base.Enter();
        enterNextAttack = false;
        // 播放普通攻击动画
        playerController.PlayAnimation("Attack_Normal_" + playerModel.skillConfig.currentNormalAttackIndex, 0.1f);
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

        // 检测普通攻击连击
        if(NormalizedTime() >= 0.5 && playerController.inputSystem.Player.Fire.triggered)
        {
            enterNextAttack = true;
        }

        #region 动画是否播放完毕
        if (IsAnimationEnd())
        {
            if (enterNextAttack)
            {
                // 切换到下一次普通攻击
                // 普攻段数累加
                playerModel.skillConfig.currentNormalAttackIndex++;
                if(playerModel.skillConfig.currentNormalAttackIndex > playerModel.skillConfig.normalAttackDamageMulitple.Length)
                {
                    // 如果超过了最大段数，则重置为第一段
                    playerModel.skillConfig.currentNormalAttackIndex = 1;
                }
                playerController.SwitchState(PlayerState.NormalAttack);
                return;
            }
            else
            {
                // 切换普攻后摇
                playerController.SwitchState(PlayerState.NormalAttack_End);
                return;
            }
        }
        #endregion
    }
}
