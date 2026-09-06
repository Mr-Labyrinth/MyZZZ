using UnityEngine;

/// <summary>
/// Íæ¼Ò¼±Í£×´Ì¬
/// </summary>
public class PlayerRunEndState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        #region ÅÐ¶Ï×óÓÒ½Å
        switch (playerModel.foot)
        {
            case ModelFoot.Right:
                playerController.PlayAnimation("Run_End_R", 0.1f);
                break;
            case ModelFoot.Left:
                playerController.PlayAnimation("Run_End_L", 0.1f);
                break;  
        }
        #endregion
    }

    public override void Update()
    {
        base.Update();

        #region ¼ì²é´óÕÐ
        if (playerController.inputSystem.Player.BigSkill.triggered)
        {
            //ÇÐ»»µ½½øÈë´óÕÐ×´Ì¬
            playerController.SwitchState(PlayerState.BigSkillStart);
            return;
        }
        #endregion

        #region ¼ì²éÆÕÍ¨¹¥»÷
        if (playerController.inputSystem.Player.Fire.triggered)
        {
            // ÇÐ»»µ½ÆÕÍ¨¹¥»÷×´Ì¬
            playerController.SwitchState(PlayerState.NormalAttack);
            return;
        }
        #endregion

        #region ¼ì²éÉÁ±Ü
        if (playerController.inputSystem.Player.Evade.IsPressed())
        {
            // ÇÐ»»µ½ÉÁ±Ü×´Ì¬
            playerController.SwitchState(PlayerState.Evade_Back);
            return;
        }
        #endregion

        #region ¼ì²éÒÆ¶¯×´Ì¬
        if(playerController.inputMoveVec2 != Vector2.zero)
        {
            playerController.SwitchState(PlayerState.Walk);
            return;
        }
        #endregion

        #region ¼ì²é¶¯»­²¥·ÅÍê±Ï
        if (IsAnimationEnd())
        {
            playerController.SwitchState(PlayerState.Idle);
            return;
        }
        #endregion
    }


}
