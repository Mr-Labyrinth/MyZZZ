using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HUDBtn : MonoBehaviour
{
    [SerializeField]
    private RectTransform AttackBtn;
    [SerializeField]
    private RectTransform EvadeBtn;
    [SerializeField]
    private RectTransform SPAttackBtn;
    [SerializeField]
    private RectTransform SwitchBtn;
    [SerializeField]
    private RectTransform BigSkillBtn;

    private InputSystem inputSystem;

    private void Awake()
    {
        inputSystem = InputManager.Instance.inputActions;
    }

    private void Update()
    {
        if (inputSystem.Player.Fire.triggered)
        {
            OnAttack();
        }
        if (inputSystem.Player.Evade.triggered)
        {
            OnEvade();
        }
        //if (inputSystem.Player.SPAttack.triggered)
        //{
        //    OnSPAttack();
        //}
        if (inputSystem.Player.SwitchDown.triggered)
        {
            OnSwitch();
        }
        if (inputSystem.Player.BigSkill.triggered)
        {
            OnBigSkill();
        }
    }

    private void OnAttack()
    {
        DoPunchAnimation(AttackBtn);
    }

    private void OnEvade()
    {
        DoPunchAnimation(EvadeBtn);
    }

    private void OnSPAttack()
    {
        
    }
    private void OnSwitch()
    {
        DoPunchAnimation(SwitchBtn);
    }
    private void OnBigSkill()
    {
        DoPunchAnimation(BigSkillBtn);
    }

    private void DoPunchAnimation(RectTransform rectTransform, float punchAmout = 0.2f, float duration = 0.3f, int vibrato = 1, float elasticity = 1f)
    {
        rectTransform.DOKill();
        rectTransform.localScale = Vector3.one;
        rectTransform.DOPunchScale(Vector3.one *punchAmout,
            duration,
            vibrato,
            elasticity).SetLink(rectTransform.gameObject);
    }
}
