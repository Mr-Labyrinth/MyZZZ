using UnityEngine;

public class RetrySuccessNode : DecoratorNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(child.Update() is State.Success)
        {
            return State.Success;
        }
        return State.Running;
    }
}
