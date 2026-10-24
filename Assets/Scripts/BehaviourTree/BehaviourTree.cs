using UnityEngine;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    public Node rootNode;
    public Node.State treeState = Node.State.Runing;

    public Node.State Update()
    {
        return rootNode.Update();
    }
}
