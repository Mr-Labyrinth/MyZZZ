using UnityEngine;

public class RunBehaviour : MonoBehaviour
{
    public BehaviourTree tree;
    void Start()
    {
        tree = tree.Clone();
    }

    void Update()
    {
        tree.Update();
    }
}
