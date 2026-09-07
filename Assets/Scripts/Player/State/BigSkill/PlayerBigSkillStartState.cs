using UnityEngine;
using Unity.Cinemachine;

/// <summary>
/// 玩家开始放大招状态
/// </summary>
public class PlayerBigSkillStartState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        // 切换镜头
        CameraManager.INSTANCE.cm_brain.DefaultBlend =
            new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.Cut, 0f);
        CameraManager.INSTANCE.freeLookCamera.SetActive(false);
        playerModel.bigSkillStartShot.SetActive(true);

        //播放动画
        playerController.PlayAnimation("BigSkill_Start", 0.1f);
    }

    public override void Update()
    {
        base.Update();

        #region 动画是否播放结束
        if (IsAnimationEnd())
        {
            //切换到大招状态
            playerController.SwitchState(PlayerState.BigSkill);
            return;
        }
        #endregion
    }


}
