using UnityEngine;

public class SingleMomoBase<T> : MonoBehaviour where T : SingleMomoBase<T>
{
    public static T INSTANCE;

    protected virtual void Awake()
    {
        if( INSTANCE != null)
        {
            Debug.LogError(this + "²»·ûºÏµ¥Àý");
        }
        INSTANCE = (T)this;
    }

    protected virtual void OnDestroy()
    {
        Destroy();
    }

    public void Destroy()
    {
        INSTANCE = null;
    }

}
