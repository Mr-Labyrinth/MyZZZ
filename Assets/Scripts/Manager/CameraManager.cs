using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// 相机管理器
/// </summary>
public class CameraManager : SingleMomoBase<CameraManager>
{
    // CM的大脑组件
    public CinemachineBrain cm_brain;
    // 自由相机
    public GameObject freeLookCamera;
    // 自由相机的组件
    public CinemachineFreeLook freeLook;

    public void ResetFreeLookCamera()
    {
        freeLook.m_YAxis.Value = 0.5f;
        freeLook.m_XAxis.Value = PlayerController.INSTANCE.transform.eulerAngles.y;
    }
}
