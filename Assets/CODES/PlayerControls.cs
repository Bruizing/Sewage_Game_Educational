using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{   //Movement
    public float moveSpeed = 4f;
    public Animator Anime;
    public AudioSource JumpSFX;
    public Shooting GunPoint;
    public Rigidbody2D chara;
    private Vector2 lastMovement;
    //Jumping
    public float JumpForce = 9f;
    public bool isGrounded;
    private Vector2 movement;
    private bool onlyOnce = false;
    // store horizontal momentum for air
    private float airVelX;
    public GameObject GameOverBool;
    public bool hasMove = false;//bool for Death

    void Start()
    {
        GunPoint.enabled = true;
        chara = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        // Lock only horizontal momentum; let physics control vertical fully.
        Vector2 v = chara.velocity;
        v.x = airVelX;
        chara.velocity = v;
    }
    // movement code vvvvv
    void Update()
    {
        //Movement code vvvvv
        float horizontalInput = Input.GetAxis("Horizontal");
        if (isGrounded)
        {
            airVelX = horizontalInput * moveSpeed;
        }
        
        // for animations that relied on movement (not used for physics anymore)
        movement = new Vector2(isGrounded ? airVelX : airVelX, 0f) * Time.fixedDeltaTime;
        if (movement != Vector2.zero)
        {
            lastMovement = movement;
        }

        if (GameOverBool.activeSelf == false)//allows jumping to happen when not gameover
        {
            // Jump
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
            {
                hasMove = true;
                JumpSFX.pitch = Random.Range(0.9f, 1.8f);
                JumpSFX.Play();
                isGrounded = false;
                // snapshot current horizontal at takeoff
                airVelX = horizontalInput * moveSpeed;
                chara.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            }
            else if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                hasMove = true;
                JumpSFX.pitch = Random.Range(0.9f, 1.8f);
                JumpSFX.Play();
                isGrounded = false;
                // snapshot current horizontal at takeoff
                airVelX = horizontalInput * moveSpeed;
                chara.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            }
        }

        // Animations
        if (horizontalInput > 0 || chara.velocity.x > 0)
        {
            hasMove = true;
            if (isGrounded)
            {
                Anime.Play("Run_Right");
            }
            else if (isGrounded == false)
            {
                Anime.Play("Jump_Right");//either jumping or from walking off
            }
        }
        else if (horizontalInput < 0 || chara.velocity.x < 0)
        {
            hasMove = true;
            if (isGrounded)
            {
                Anime.Play("Run_Left");
            }
            else if (isGrounded == false)
            {
                Anime.Play("Jump_Left");//either jumping of from walking off
            }
        }
        else if (horizontalInput == 0 && isGrounded == false)//When jump goes only up
        {
            hasMove = true;
            RandomFacing();
        }
        else
        {
            Anime.Play("Idle_Right");
            hasMove = false;
        }
        // GunPoint enable logic. For moments of no shoot vvv
        if (isGrounded == true && Mathf.Abs(horizontalInput) > 0)
        {
            GunPoint.isEnabled = false;
        }
        else
        {
            GunPoint.isEnabled = true;
        }
        
    }
    void RandomFacing()//only happen once
    {
        if (onlyOnce == false)
        {
            onlyOnce = true;

            int randomNum = Random.Range(1, 5);// Generate a random num between 1-4

            if (randomNum % 2 == 0)//even
            {
                Anime.Play("Jump_Right");
            }
            else if (randomNum % 2 == 1)//odd
            {
                Anime.Play("Jump_Left");
            }
        }
        else
        {
            return;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (gameObject != null)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                StartCoroutine(GroundTimeTouch());
            }
        } 
    } 
    private void OnCollisionExit2D(Collision2D other)
    {
        if (gameObject != null)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
                StartCoroutine(GroundTimeLeft());
            }
        }
    }
    IEnumerator GroundTimeTouch()
    {
        yield return new WaitForSeconds(0.01f);
        isGrounded = true;
        onlyOnce = false;
    }
    IEnumerator GroundTimeLeft() // for making one animation play when falling
    {
        yield return new WaitForSeconds(0.1f);
        // keep script enabled so physics/velocity continue to update
    }
}
