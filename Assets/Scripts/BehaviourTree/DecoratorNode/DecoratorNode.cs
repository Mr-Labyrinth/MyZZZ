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

    public override void Bind(GameObject gameObject)
    {
        if (child != null)
        {
            child.Bind(gameObject);
        }
    }

    public override void Abort()
    {
        base.Abort();

        child.Abort();
    }
}
