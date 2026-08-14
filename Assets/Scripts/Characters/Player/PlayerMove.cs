using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float BaseSpeed = 5;

    [SerializeField]
    private float BaseJumpPower = 2;

    [SerializeField]
    private float BaseDashPower = 1;

    private Camera camMain;

    private InputActions action;

    private Vector2 moveInput;
    
    private bool jumpTriggered;


    private enum MoveState
    {
        Ground,
        AirBorne,
        Idel,
        Dash,
        Walk,
        Run,
        Sprint,
        Jump,
        LightLanding
    }
    private enum PlayerAction
    {
        FixedUpdate,
        Walk,
        Run,
        Sprint,
        Jump,
        Dash
    }

    private void Awake()
    {
        action = new InputActions();
        rb = GetComponent<Rigidbody>();
        action.Player.Jump.performed += JumpInput;
        action.Player.Dash.performed += DashInput;
        action.Player.Move.performed += MoveInput;
        action.Player.Move.canceled += UnMoveInput;
    }

    

    void Start()
    {
        action.Enable();
        camMain = Camera.main;


    }

    void MoveInput(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    void UnMoveInput(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

    void JumpInput(InputAction.CallbackContext ctx)
    {
        jumpTriggered = true;
    }

    void DashInput(InputAction.CallbackContext obj)
    {
        
    }

    Vector3 getRawMoveDir()
    {
        Vector3 camForward = camMain.transform.forward;
        Vector3 camRight = camMain.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward = camForward.normalized;
        camRight = camRight.normalized;

        // 将输入混合相机方向，得到世界坐标的移动方向。
        Vector3 rawMoveDir = camForward * moveInput.y + camRight * moveInput.x;

        return rawMoveDir;
    }

    void move(float speed)
    {
        Vector3 rawMoveDir = getRawMoveDir();

        // 移动赋值（即使原地不动也要保留竖直速度，不影响跳跃下落)
        if (rawMoveDir.sqrMagnitude > 0.001f)
        {
            Vector3 moveDir = rawMoveDir.normalized;
            rb.linearVelocity = new Vector3(moveDir.x * speed, rb.linearVelocity.y, moveDir.z * speed);

            // 只有存在有效移动方向时才转向
            transform.rotation = Quaternion.LookRotation(moveDir);
        }
        else
        {
            // 原地不动：保留Y速度，清除水平速度，防止人物持续滑行
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (jumpTriggered)
        {
            rb.AddForce(Vector3.up * BaseJumpPower, ForceMode.Impulse);
            jumpTriggered = false;
        }

    }

    private void OnDestroy()
    {
        action.Player.Jump.performed -= JumpInput;
        action.Player.Dash.performed -= DashInput;
        action.Player.Move.performed -= MoveInput;
        action.Player.Move.canceled -= UnMoveInput;

        action.Disable();
    }
}
