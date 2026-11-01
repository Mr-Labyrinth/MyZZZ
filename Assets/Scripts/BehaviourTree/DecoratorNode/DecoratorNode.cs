using UnityEngine;

public abstract class DecoratorNode : Node
{
    [HideInInspector] public Node child;

    public override Node Clone()
    {
        DecoratorNode decoratorNodeClone = Instantiate(this);
        if (child != null)
        {
            decoratorNodeClone.child = child.Clone();
        }
        return decoratorNodeClone;
    }
}
