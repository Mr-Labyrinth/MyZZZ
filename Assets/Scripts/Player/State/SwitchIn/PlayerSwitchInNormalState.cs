using UnityEngine;

/// <summary>
/// ½ÇÉ«ÆÕÍ¨Èë³¡×´Ì¬
/// </summary>
public class PlayerSwitchInNormalState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        playerController.PlayAnimation("SwitchIn_Normal", 0f);
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

        #region ¼ì²é¹¥»÷
        if (playerController.inputSystem.Player.Fire.triggered)
        {
            //ÇÐ»»µ½¹¥»÷×´Ì¬
            playerController.SwitchState(PlayerState.NormalAttack);
            return;
        }
        #endregion

        #region ¼ì²âÒÆ¶¯
        if (playerController.inputMoveVec2 != Vector2.zero)
        {
            //ÇÐ»»µ½ÒÆ¶¯×´Ì¬
            playerController.SwitchState(PlayerState.Walk);
            return;
        }
        #endregion

        #region ¼ì²éÉÁ±Ü
        if (playerController.inputSystem.Player.Evade.triggered)
        {
            //ÇÐ»»µ½ÉÁ±Ü×´Ì¬
            playerController.SwitchState(PlayerState.Evade_Back);
            return;
        }
        #endregion

        #region ¶¯»­²¥·ÅÍê±ÏºóÇÐ»»µ½¾²Ö¹×´Ì¬
        if (IsAnimationEnd())
        {
            //ÇÐ»»µ½¾²Ö¹×´Ì¬
            playerController.SwitchState(PlayerState.Idle);
            return;
        }
        #endregion
    }
}
