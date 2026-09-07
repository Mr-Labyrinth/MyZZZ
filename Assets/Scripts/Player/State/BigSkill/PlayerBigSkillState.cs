using UnityEngine;

public class PlayerBigSkillState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        // 切换镜头
        playerModel.bigSkillStartShot.SetActive(false);
        playerModel.bigSkillShot.SetActive(true);

        // 播放动画
        playerController.PlayAnimation("BigSkill", 0.1f);
    }

    public override void Update()
    {
        base.Update();

        #region 动画是否播放结束
        if (IsAnimationEnd())
        {
            // 切换到大招结束状态
            playerController.SwitchState(PlayerState.BigSkillEnd);
            return;
        }
        #endregion
    }
}
