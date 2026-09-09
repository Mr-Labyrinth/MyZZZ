using Unity.Cinemachine;
using UnityEngine;

public class PlayerBigSkillEndState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        // ÇÐ»»¾µÍ·
        playerModel.bigSkillShot.SetActive(false);
        CameraManager.INSTANCE.cm_brain.DefaultBlend =
            new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.EaseInOut, 1f);
        CameraManager.INSTANCE.freeLookCamera.SetActive(true);
        CameraManager.INSTANCE.ResetFreeLookCamera();

        // ²¥·Å¹¥»÷ºóÒ¡¶¯»­
        playerController.PlayAnimation("BigSkill_End", 0.0f);
    }
    public override void Update()
    {
        base.Update();

        #region ¼ì²é¹¥»÷
        if (playerController.inputSystem.Player.Fire.triggered)
        {
            //ÇÐ»»µ½ÆÕÍ¨¹¥»÷×´Ì¬
            playerController.SwitchState(PlayerState.NormalAttack);
            return;
        }
        #endregion

        #region ¼ì²âÒÆ¶¯
        if (playerController.inputMoveVec2 != Vector2.zero)
        {
            // ÇÐ»»µ½ÒÆ¶¯×´Ì¬
            playerController.SwitchState(PlayerState.Walk);
            return;
        }
        #endregion

        #region ¼ì²éÉÁ±Ü
        if (playerController.inputSystem.Player.Evade.triggered)
        {
            // ÇÐ»»µ½ÉÁ±Ü×´Ì¬
            playerController.SwitchState(PlayerState.Evade_Back);
            return;
        }
        #endregion

        #region ¼ì²â¶¯»­ÊÇ·ñ²¥·ÅÍê±Ï
        if (playerModel.IsAnimationEnd())
        {
            // ÇÐ»»µ½´óÕÐ×´Ì¬
            playerController.SwitchState(PlayerState.Idle);
            return;
        }
        #endregion
    }
}
