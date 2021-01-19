using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    bool facingRight = true;

    float horizontal;
    public GameObject target;
    public float baseSpeed = 60.0f;
    float speed;
    Rigidbody2D rigidbody2d;
    public int atk = 5;
    public int maxHealth = 3;
    Animator animator;

    public Vector2 knockBack;
    bool isKnockBack;

    public bool isLaunch = false;
    public float timeLaunch = 1.0f;
    float launchTimer;
    public Vector2 direction;

    SpriteRenderer mySpriteRenderer;

    int currentHealth;
    public int health
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    public float timeInvincible = 0.4f;
    bool isInvincible;
    float invincibleTimer;

    int status;
    int flag;


    // Start is called before the first frame update
    void Start()
    {
        flag = 0;
        currentHealth = maxHealth;
        speed = baseSpeed;
        status = 0;
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Status: " +status);
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }

        Vector2 position = rigidbody2d.position;
        if (isLaunch)
        {
            launchTimer -= Time.deltaTime;
            if (launchTimer < 0)
            {
                isLaunch = false;
            }
        }

        else
        {
            if (Mathf.Abs(target.transform.position.x - position.x) >= 10)
            {
                status = 0;
            }
            if (Mathf.Abs(target.transform.position.x - position.x) < 10 && (status != 2))
            {
                status = 1;
                Debug.Log("Status 1");
            }
            if (currentHealth <= 0)
            {
                status = 2;

                if(flag == 0) 
                {
                    PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();
                    player.getPlayerInfo().addExp(100);
                    player.gold = player.gold+3;
                    player.currentMP = player.currentMP+5;
                    flag = 1;
                }

                animator.SetTrigger("Dead");
                gameObject.layer = 14; // layer "EnemyGhost"
                Debug.Log("Slime Dead");
            }


            if (status == 1)
            {
                if (target.transform.position.x > position.x)
                    horizontal = 1;
                else if (target.transform.position.x < position.x)
                    horizontal = -1;
                else horizontal = 0;
            }

            if (horizontal > 0)
            {
                if (!facingRight)
                    Flip();
            }
            else if (horizontal < 0)
            {
                if (facingRight)
                    Flip();
            }
        }
        

    }

    private void FixedUpdate()
    {
        if (status == 0 && isLaunch)
        {
            rigidbody2d.AddForce(direction, ForceMode2D.Impulse);
        }

        if (status == 1)
        {
            float moveBy = horizontal * speed * Time.deltaTime;
            if (isLaunch)
            {
                rigidbody2d.AddForce(direction, ForceMode2D.Impulse);
            }
            else if (!isKnockBack)
                rigidbody2d.velocity = new Vector2(moveBy, rigidbody2d.velocity.y);
            else
            {
                rigidbody2d.AddForce(knockBack, ForceMode2D.Impulse);
                isKnockBack = false;
            }
        }
    }

    public void ChangeHealth(int amount)
    {
        if (status > 1)
            return;
        if (amount < 0)
        {

            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            if ((knockBack.x < 0 && horizontal < 0) || (knockBack.x > 0 && horizontal > 0))
            {
                knockBack.x = -knockBack.x;
            }
          
            isKnockBack = true;
            Debug.Log("Knockback " + knockBack.x);
            //PlaySound(playerHitClip);
            animator.SetTrigger("Hurt");
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log("Slime Health: " + currentHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            if ((player.knockBack.x > 0 && horizontal <= 0) || (player.knockBack.x < 0 && horizontal > 0))
            {
                player.knockBack.x = -player.knockBack.x;
            }
            player.ChangeHealth(-atk);
        }
    }

    public void Launch(Vector2 d)
    {
        direction = d;
        isLaunch = true;
        launchTimer = timeLaunch;
    }

    void Flip()
    {
        facingRight = !facingRight;

        mySpriteRenderer.flipX = !mySpriteRenderer.flipX;
        Debug.Log("Flip " + mySpriteRenderer.flipX);
    }
}
