using UnityEngine;
using DG.Tweening;

public class GameManager : SingleMomoBase<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
        Object.DontDestroyOnLoad(this.gameObject);
        DOTween.Init(true,true,LogBehaviour.Default).SetCapacity(500,100);
    }
}
