using System;
using UnityEngine;

public class ConditionNode : ActionNode
{
    public Type type;

    public GameObject target;

    public override void Bind(GameObject gameObject)
    {
        target = gameObject;
    }
    protected override void OnStart()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnStop()
    {
        throw new System.NotImplementedException();
    }

    protected override State OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}
