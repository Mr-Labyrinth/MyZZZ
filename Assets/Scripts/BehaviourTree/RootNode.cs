using UnityEngine;

public class RootNode : Node
{
    public Node child;
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        return child.Update();
    }

    public override Node Clone()
    {
        RootNode rootNodeClone = Instantiate(this);
        if(child != null)
        {
            rootNodeClone.child = child.Clone();
        }
        return rootNodeClone;
    }
}
