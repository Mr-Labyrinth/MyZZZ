using UnityEngine;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    public Node rootNode;
    public Node.State treeState = Node.State.Runing;

    public Node.State Update()
    {
        if(rootNode.state == Node.State.Runing)
        {
            treeState = rootNode.Update();
        }
        return treeState;
    }
}
