using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float idleTime = 2f;
    public float walkTime = 2f;
    public float moveSpeed = 3f;
    public float jumpForce = 5f;
    public float playerDetectionRadius = 5f;
    public Animator slime;
    private float timer;
    private bool isWalking;
    private Vector3 direction;
    private Transform playerTransform;
    public AudioSource Idle;
    public AudioSource Move;
    private float changeDirectionCooldown = 0.5f; // Cooldown time in seconds before changing direction again
    private float lastDirectionChangeTime = -1f; // Time since last direction change

    void Start()
    {
        timer = idleTime;
        isWalking = false;
        direction = Vector3.zero;
        Move.Stop();
        Idle.Play();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // Check for player presence within detection radius
        bool playerDetected = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, playerDetectionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                playerTransform = collider.transform;
                playerDetected = true;
                break;
            }
        }

        if (timer <= 0f)
        {
            if (!isWalking)
            {
                isWalking = true;
                // If player is detected, set direction towards the player
                if (playerDetected)
                {
                    Vector3 playerDirection = playerTransform.position - transform.position;
                    direction = playerDirection.normalized;
                }
                else // Otherwise, choose a random direction
                {
                    direction = Quaternion.Euler(0, Random.Range(0, 360), 0) * Vector3.forward;
                }

                timer = walkTime;
                slime.Play("slimeMoving");
                Move.Play();
                Idle.Stop();
            }
            else
            {
                isWalking = false;
                timer = idleTime;
                slime.Play("slimeIdle");
                Move.Stop();
                Idle.Play();
            }
        }

        if (isWalking)
        {
            // Calculate the rotation angle towards the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Change direction if collided with something
        direction = Quaternion.Euler(0, Random.Range(0, 360), 0) * Vector3.forward;

        // Check if something is blocking the path
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 1f))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                // Jump over the obstacle after a delay
                StartCoroutine(JumpAfterDelay());
            }
        }
    }

    private IEnumerator JumpAfterDelay()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            slime.Play("Slime_Jump");
            yield return new WaitForSeconds(0.6f); // Adjust the delay time as needed
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
