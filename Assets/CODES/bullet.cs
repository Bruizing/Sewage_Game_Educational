using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 20f;
    private Rigidbody2D rb;
    public GameObject TheBullet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = transform.right * speed;
        StartCoroutine(BulletTime());
    }

    IEnumerator BulletTime()
    {
        yield return new WaitForSeconds(2f);
        Destroy(TheBullet);
    }
}
