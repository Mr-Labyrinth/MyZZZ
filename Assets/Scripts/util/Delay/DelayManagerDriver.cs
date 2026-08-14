using UnityEngine;

public class DelayManagerDriver : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void AutoInit()
    {
        if(FindAnyObjectByType<DelayManagerDriver>() == null)
        {
            var go = new GameObject("[DelayManagerDriver]");
            go.AddComponent<DelayManagerDriver>();
            DontDestroyOnLoad(go);
        }
    }

    
    void Update()
    {
        DelayManager.Instance.Update();
    }
}
