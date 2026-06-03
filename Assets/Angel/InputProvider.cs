using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class InputProvider : MonoBehaviour
{
    ActionSystem inputActions;

    private float DirX = 0f;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [SerializeField] private float XVel;
    [SerializeField] private float YVel;

    [Header("Player Components References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    [Header("Ground Check")]
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform groundCheck;

    private void Awake()
    {
        inputActions = new ActionSystem();
        inputActions.Enable();

        inputActions.Player.Movement.performed += ctx =>
        {
            DirX = ctx.ReadValue<float>();
        };

        inputActions.Player.Jump.performed += ctx => Jump();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(DirX * speed * Time.deltaTime, rb.velocity.y);
        XVel = rb.velocity.x;
        YVel = rb.velocity.y;
        int XInt = Mathf.RoundToInt(XVel);
        int YInt = Mathf.RoundToInt(YVel)   ;

        if(XInt < 0)
        {
            anim.SetInteger("XDir", XInt);
        }
        else if(DirX > 0)
        {
            anim.SetInteger("XDir", XInt);
        }
        if(YInt < 0)
        {
            anim.SetInteger("YDir", YInt);
        }
    }

    void Jump()
    {
    if(inputActions.Player.Jump.triggered && isGrounded())
       {
           rb.velocity = new Vector2(rb.velocity.x, jumpForce);
       }
    }

    bool isGrounded()
    {
            return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.4f, 0.1f), CapsuleDirection2D.Horizontal, 0, layerMask);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }
     
    private void OnDisable()
    {
        inputActions.Disable();
    }







    /*
    [SerializeField] private InputActionReference playerInput;
    [SerializeField] private float speed;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {   
        Vector2 MoveDirection = playerInput.action.ReadValue<Vector2>();
        transform.Translate(MoveDirection.x * speed * Time.deltaTime,0,0);
    }
    */
}
