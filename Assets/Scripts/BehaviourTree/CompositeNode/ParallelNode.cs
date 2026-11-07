using UnityEngine;

public class ParallelNode : CompositeNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        State state;
        foreach (var child in children)
        {
            state = child.Update();
            if(state is (State.Failure or State.Success))
                return state;
            
        }
        return State.Running;
    }
}
