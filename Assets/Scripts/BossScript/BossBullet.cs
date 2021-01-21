using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public int atk = 10;
    public Vector2 direction;
    Rigidbody2D rigidbody2d;
    Animator animator;
    public float timeBullet = 6f;
    float bulletTimer;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bulletTimer = timeBullet;
    }

    private void Update()
    {
        bulletTimer -= Time.deltaTime;
        if (bulletTimer < 0)
        {
            animator.Play("BulletDestroy");
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody2d.AddForce(direction*Time.deltaTime, ForceMode2D.Impulse);
    }

    public void SetDirection(Vector2 d)
    {
        direction = d;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            
            player.ChangeHealth(-atk);
            animator.Play("BulletDestroy");
            Destroy(gameObject);
        }

    }
}
