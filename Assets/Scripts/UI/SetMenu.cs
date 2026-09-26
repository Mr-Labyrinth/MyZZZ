using UnityEngine;
using UnityEngine.InputSystem;

public class SetMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject setMenu;
    private InputSystem inputActions;
    private bool isPaused = false;

    private void Awake()
    {
        inputActions = InputManager.Instance.inputActions;
    }

    private void Start()
    {
        setMenu.SetActive(false);
    }

    private void OnEnable()
    {

        inputActions.UI.Enable();
        inputActions.UI.Esc.performed += OnMenu;
    }
    private void OnDisable()
    {
        inputActions.UI.Esc.performed -= OnMenu;
        inputActions.UI.Disable();
    }

    private void OnMenu(InputAction.CallbackContext context)
    {
        if (isPaused)
            closeMenu();
        else
            openMenu();

    }
    private void openMenu()
    {
        isPaused = true;
        setMenu.SetActive(true);
        Time.timeScale = 0f;

        // …Ë÷√ ‰»Î”≥…‰
        inputActions.Player.Disable();
        InputManager.Instance.UnLockMouse();
    }
    private void closeMenu()
    {
        isPaused = false;
        setMenu.SetActive(false);
        Time.timeScale = 1;

        // ª÷∏¥Ω«…´øÿ÷∆
        inputActions.Player.Enable();
        InputManager.Instance.LockMouse();
    }

    
}
