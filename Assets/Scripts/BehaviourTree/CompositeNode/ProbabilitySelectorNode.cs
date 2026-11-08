using System.Collections.Generic;
using UnityEngine;

public class ProbabilitySelectorNode : CompositeNode
{
    public List<float> probabilities = new List<float>();

    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        return State.Running;
    }
}
