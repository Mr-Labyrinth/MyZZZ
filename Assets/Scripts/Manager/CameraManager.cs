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
    public CinemachineCamera freeLook;

    public void ResetFreeLookCamera()
    {
        if (freeLookCamera != null)
        {
            // 1. 获取相机上的 CinemachineOrbitalFollow 组件
            var orbitalFollow = freeLookCamera.GetComponent<CinemachineOrbitalFollow>();
            if (orbitalFollow != null)
            {
                // 2. 通过该组件的 HorizontalAxis 和 VerticalAxis 来调整角度
                // 垂直角度 (Y轴): 范围通常是 -90 到 90[reference:2]
                orbitalFollow.VerticalAxis.Value = 0f;

                // 水平角度 (X轴): 范围通常是 -180 到 180[reference:3]
                orbitalFollow.HorizontalAxis.Value = PlayerController.INSTANCE.transform.eulerAngles.y;
            }
        }
    }
}
