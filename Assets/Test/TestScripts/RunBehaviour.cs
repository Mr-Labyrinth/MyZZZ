using UnityEngine;

public class RunBehaviour : MonoBehaviour
{
    BehaviourTree tree;
    void Start()
    {
        tree = ScriptableObject.CreateInstance<BehaviourTree>();
        var log = ScriptableObject.CreateInstance<DebugLogNode>();
        log.message = "HEEELLLLOOOOOO YOU";
        tree.rootNode = log;
    }

    void Update()
    {
        tree.Update();
    }
}
