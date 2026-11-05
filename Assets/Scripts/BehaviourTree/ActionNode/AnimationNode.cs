using UnityEditor;
using UnityEngine;

public class AnimationNode : ActionNode
{
    public string animationName;

    public GameObject targetGameObject;

    public Animator animator;

    public override void Bind(GameObject gameObject)
    {
        targetGameObject = gameObject;
        if (targetGameObject == null)
        {
            targetGameObject = gameObject;
        }
        animator = targetGameObject.GetComponentInChildren<Animator>();
    }

    protected override void OnStart()
    {
        animator.CrossFadeInFixedTime(animationName, 0.25f);
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(IsAnimationEnd())
        {
            return State.Success;
        }
        return State.Running;
    }

    public override void Abort()
    {
        animator.Play(animationName, 0, 1.0f);
        base.Abort();
    }

    public bool IsAnimationEnd()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0);
    }
}
