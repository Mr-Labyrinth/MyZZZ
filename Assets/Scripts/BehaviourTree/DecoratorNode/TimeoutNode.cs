using UnityEngine;

public class TimeoutNode : DecoratorNode
{
    public float timeout;

    public float currentTime;

    protected override void OnStart()
    {
        currentTime = 0f;
    }

    protected override void OnStop()
    {
        currentTime = 0f;
    }

    protected override State OnUpdate()
    {
        if(currentTime >= timeout)
        {
            return State.Failure;
        }

        var state = child.Update();

        currentTime += Time.deltaTime;

        return state;
    }
}
