using UnityEngine;


public class LoopNode : DecoratorNode
{

    public int loopCount;

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if( loopCount == 0)
        {
            return State.Success;
        }

        child.Update();

        loopCount--;

        return State.Running;
    }
}
