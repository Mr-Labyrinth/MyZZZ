using UnityEngine;

public class PlayerRunState : PlayerStateBase
{
    private Camera mainCamera;

    public override void Enter()
    {
        base.Enter();

        mainCamera = Camera.main;

        //判读移动状态
        switch (playerModel.currentState)
        {
            case PlayerState.Walk:
                AnimateWalkStep("Walk");
                break;
            case PlayerState.Run:
                AnimateWalkStep("Run");
                break;
        }
    }

    private void AnimateWalkStep(string animation)
    {
        #region 迈出左右脚的判断
        switch (playerModel.foot)
        {
            case ModelFoot.Left:
                playerController.PlayAnimation(animation, 0.125f, 0.5f);
                playerModel.SetOutRightFoot();
                break;
            case ModelFoot.Right:
                playerController.PlayAnimation(animation, 0.125f, 0.0f);
                playerModel.SetOutLeftFoot();
                break;
        }
        #endregion
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

        #region 检查普通攻击
        if (playerController.inputSystem.Player.Fire.triggered)
        {
            // 切换到普通攻击状态
            playerController.SwitchState(PlayerState.NormalAttack);
            return;
        }
        #endregion

        #region 检查闪避
        if (playerController.inputSystem.Player.Evade.triggered)
        {
            // 切换到闪避状态
            playerController.SwitchState(PlayerState.Evade_Front);
            return;
        }
        #endregion

        #region 检查待机
        if(playerController.inputMoveVec2 == Vector2.zero)
        {
            // 切换到待机状态
            playerController.SwitchState(PlayerState.Idle);
            return;
        }
        #endregion
        else
        {
            #region 检查移动
            Vector3 inputMoveVec3 = new Vector3(playerController.inputMoveVec2.x
                , 0,
                playerController.inputMoveVec2.y);
            // 获取相机的旋转轴Y
            float camerAxisY = mainCamera.transform.eulerAngles.y;
            // 四元数 x 向量
            Vector3 targetDic = Quaternion.Euler(0, camerAxisY, 0) * inputMoveVec3;
            Quaternion targetQua = Quaternion.LookRotation(targetDic);
            //计算旋转角度
            float angle = 
                Mathf.Abs(targetQua.eulerAngles.y - playerModel.transform.eulerAngles.y);

            if(angle > 145f && angle < 215f && playerModel.currentState == PlayerState.Run)
            {
                // 切换到转身状态
                playerController.SwitchState(PlayerState.TurnBack);
            }
            else
            {
                playerModel.transform.rotation = 
                    Quaternion.Slerp(
                        playerModel.transform.rotation, 
                        targetQua, 
                        Time.deltaTime * playerController.rotationSpeed);
            }
            #endregion
        }

        #region 检查漫步升级
        if(playerModel.currentState == PlayerState.Walk && statePlayTime > 3f)
        {
            //切换到跑步状态
            playerController.SwitchState(PlayerState.Run);
            return;
        }
        #endregion
    }
}
