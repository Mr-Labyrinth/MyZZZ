using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine;

[UxmlElement]
public partial class BehaviourTreeView : GraphView
{
    public Action<NodeView> OnNodeSelected;
    BehaviourTree tree;

    public BehaviourTreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var sylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
        styleSheets.Add(sylesheet);
    }

    NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    // 填充图形视图，将行为树中的节点创建为节点视图并添加到图形视图中
    internal void PopulateView(BehaviourTree tree)
    {
        this.tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        if(tree.rootNode is null)
        {
            tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }

        // 创建节点视图并添加到图形视图中
        tree.nodes.ForEach(n => CreateNodeView(n));

        tree.nodes.ForEach(n =>
        {
            var Children = tree.GetChildren(n);
            NodeView parentView = FindNodeView(n);
            Children.ForEach(c =>
            {
                NodeView childView = FindNodeView(c);
                Edge edge = parentView.output.ConnectTo(childView.input);
                AddElement(edge);
            });
        });

        schedule.Execute(RefreshCompositeEdgeOrders).ExecuteLater(0);
    }

    // 获取兼容的端口，确保连接的端口方向不同且不属于同一个节点
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort => 
            endPort.direction != startPort.direction && 
            endPort.node != startPort.node).ToList();
    }

    // 当图形视图发生变化时调用的回调函数，用于处理节点和边的添加和删除
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if(graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                NodeView nodeView = elem as NodeView;
                if(nodeView != null)
                {
                    tree.DeleteNode(nodeView.node);
                }

                Edge edge = elem as Edge;
                if(edge is not null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    tree.RemoveChild(parentView.node, childView.node);
                }
            });

            schedule.Execute(RefreshCompositeEdgeOrders).ExecuteLater(0);
        }

        if(graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                tree.AddChild(parentView.node, childView.node);
            });

            schedule.Execute(RefreshCompositeEdgeOrders).ExecuteLater(0);
        }


        return graphViewChange;
    }

    // 在右键菜单中添加创建节点的选项
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] /{type.Name}",
                    (a) => CreateNode(type));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] /{type.Name}",
                    (a) => CreateNode(type));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] /{type.Name}",
                    (a) => CreateNode(type));
            }
        }
    }

    public void RefreshCompositeEdgeOrders()
    {
        foreach(var e in nodes)
        {
            if(e is not NodeView nodeView) continue;
            if(nodeView.node is not CompositeNode compositeNode) continue;
            if(nodeView.output == null) continue;

            int total = compositeNode.children.Count;
            if(total == 0) continue;

            for(int i = 0; i < total; i++)
            {
                var child = compositeNode.children[i];
                var childView = FindNodeView(child);

                Edge edge = nodeView.output.connections.FirstOrDefault(c => c.input.node == childView);

                if (edge is null) continue;

                edge.edgeControl.Q<Label>("order-label")?.RemoveFromHierarchy();
                var label = new Label(i.ToString());
                label.name = "order-label";
                label.pickingMode = PickingMode.Ignore;
                label.style.position = Position.Absolute;
                label.style.unityFontStyleAndWeight = UnityEngine.FontStyle.Bold;
                label.style.fontSize = 18;
                label.style.left = Length.Percent(50);
                label.style.top = Length.Percent(50);
                label.style.color = new StyleColor(Color.green);
                label.style.translate = new Translate(Length.Percent(-50), Length.Percent(-50));
                edge.edgeControl.Add(label);
            }
        }
    }

    // 创建一个新的节点并将其添加到行为树中
    void CreateNode(System.Type type)
    {
        Node node = tree.CreateNode(type);
        CreateNodeView(node);
        EditorUtility.SetDirty(tree);
    }

    // 创建一个节点视图并将其添加到图形视图中
    void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }
}
