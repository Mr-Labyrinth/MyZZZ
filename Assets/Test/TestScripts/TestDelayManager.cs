using System;
using UnityEngine;

public class TestDelayManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    DelayManager delayManager = DelayManager.Instance;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            delayManager.AddScaledDelay(1, () => { Debug.Log("缩放延时输出1"); });
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            delayManager.AddUnScaledDelay(3, () => { Debug.Log("无缩放延时输出3"); });
        }
    }
}
