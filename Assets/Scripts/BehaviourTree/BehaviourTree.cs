using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    public Node rootNode;
    public Node.State treeState = Node.State.Running;
    public List<Node> nodes = new List<Node>();

    public Node.State Update()
    {
        if(rootNode.state == Node.State.Running)
        {
            treeState = rootNode.Update();
        }
        return treeState;
    }

    public Node CreateNode(System.Type type)
    {
        Node node = CreateInstance(type) as Node;
        node.name = type.Name;
        node.guid = System.Guid.NewGuid().ToString();
        nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;
    }

    public void DeleteNode(Node node)
    {
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(Node parent, Node child)
    {
        DecoratorNode decoratorNode = parent as DecoratorNode;
        if(decoratorNode)
        {
            decoratorNode.child = child;
        }

        CompositeNode compositeNode = parent as CompositeNode;
        if (compositeNode)
        {
            compositeNode.Children.Add(child);
        }
    }

    public void RemoveChild(Node parent, Node child)
    {
        DecoratorNode decoratorNode = parent as DecoratorNode;
        if (decoratorNode)
        {
            decoratorNode.child = null;
        }

        CompositeNode compositeNode = parent as CompositeNode;
        if (compositeNode)
        {
            compositeNode.Children.Remove(child);
        }
    }

    public List<Node> GetChildren(Node parent, Node child)
    {
        List<Node> children = new List<Node>();

        DecoratorNode decoratorNode = parent as DecoratorNode;
        if (decoratorNode && decoratorNode.child is not null)
        {
            children.Add(decoratorNode.child);        
        }

        CompositeNode compositeNode = parent as CompositeNode;
        if (compositeNode)
        {
            return compositeNode.Children;
        }

        return children;
    }
}
