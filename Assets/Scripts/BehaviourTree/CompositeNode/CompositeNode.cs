using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode : Node
{
    [HideInInspector] public List<Node> children  = new List<Node>();

    public override Node Clone()
    {
        CompositeNode compositeNodeClone = Instantiate(this);
        compositeNodeClone.children = children.ConvertAll(child => child.Clone());
        return compositeNodeClone;
    }
}
