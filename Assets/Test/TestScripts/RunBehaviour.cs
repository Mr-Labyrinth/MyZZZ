using UnityEngine;

public class RunBehaviour : MonoBehaviour
{
    BehaviourTree tree;
    void Start()
    {
        tree = ScriptableObject.CreateInstance<BehaviourTree>();

        var log1 = ScriptableObject.CreateInstance<DebugLogNode>();
        log1.message = "HEEELLLLOOOOOO YOU 111";

        var pause1 = ScriptableObject.CreateInstance<WaitNode>();

        var log2 = ScriptableObject.CreateInstance<DebugLogNode>();
        log2.message = "HEEELLLLOOOOOO YOU 222";

        var pause2 = ScriptableObject.CreateInstance<WaitNode>();

        var log3 = ScriptableObject.CreateInstance<DebugLogNode>();
        log3.message = "HEEELLLLOOOOOO YOU 333";

        var pause3 = ScriptableObject.CreateInstance<WaitNode>();

        var sequencer = ScriptableObject.CreateInstance<SequencerNode>();
        sequencer.Children.Add(log1);
        sequencer.Children.Add(pause1);
        sequencer.Children.Add(log2);
        sequencer.Children.Add(pause2);
        sequencer.Children.Add(log3);
        sequencer.Children.Add(pause3);

        var loop = ScriptableObject.CreateInstance<RepeatNode>();
        loop.child = sequencer;

        tree.rootNode = loop;
    }

    void Update()
    {
        tree.Update();
    }
}
