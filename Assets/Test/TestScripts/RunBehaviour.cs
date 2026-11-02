using UnityEngine;

public class RunBehaviour : MonoBehaviour
{
    public BehaviourTree tree;
    void Start()
    {
        tree = tree.Clone();
        tree.Bind(gameObject);
    }

    void Update()
    {
        tree.Update();
    }
}
