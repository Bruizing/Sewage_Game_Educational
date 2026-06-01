using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputProvider : MonoBehaviour
{
    ActionSystem inputActions;
    private float direction = 0f;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    
    Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new ActionSystem();
        inputActions.Enable();

        inputActions.Player.Movement.performed += ctx =>
        {
            direction = ctx.ReadValue<float>();
            rb.velocity = new Vector2(direction * speed * Time.deltaTime, rb.velocity.y);
        };

        inputActions.Player.Jump.performed += ctx => Jump();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(direction * speed * Time.deltaTime, rb.velocity.y);
    }

    void Jump()
    {
       if(isGrounded())
       {
           rb.velocity = new Vector2(rb.velocity.x, jumpForce);
       }
    }

    bool isGrounded()
    {
            return Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
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
