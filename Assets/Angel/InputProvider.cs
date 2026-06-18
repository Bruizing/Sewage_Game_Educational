using System;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class InputProvider : MonoBehaviour
{
    ActionSystem inputActions;
    public float DirX = 0f;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [SerializeField] private float XVel;
    [SerializeField] private float YVel;

    [Header("Player Components References")]
     public Rigidbody2D rb;
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
        float YFloat = YVel;
        
        #region XDir Animation Parameters
        if(XInt < 0)
        {
            anim.SetInteger("XDir", XInt);
        }
        else if(XInt > 0)
        {
            anim.SetInteger("XDir", XInt);
        }
        else if(XInt == 0)
        {
            anim.SetInteger("XDir", 0);
        }
        #endregion

        #region YDir Animation Parameters
        if(!isGrounded())
        {
            anim.SetInteger("YDir", 1);
        }
        else if(isGrounded())
        {
            anim.SetInteger("YDir", 0);
        }
        #endregion
        
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
