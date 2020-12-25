using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01Controller : MonoBehaviour
{
    float horizontal;
    public Transform target;
    public float baseSpeed = 80.0f;
    float speed;
    Rigidbody2D rigidbody2d;
    public int atk = 10;
    public int maxHealth = 10;
    Animator animator;

    public float timeBorn = 4.0f;
    float bornTimer;


    public Vector2 knockBack;
    bool isKnockBack;



    //Skill01 config
    float skill01Timer;
    bool usedSkill01 = false;
    public float timeSkill01 = 2.0f;
    public float timeUsedSkill01Min = 4.0f;
    public float timeUsedSkill01Max = 7.0f;
    float usedSkillTimer01;
    public float skill01Speed = 150.0f;

    bool usedSkill02 = false;
    public float mulSkill02Speed = 1.8f;

   
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
    // Start is called before the first frame update
    void Start()
    {

        gameObject.layer = 14; // layer "EnemyGhost"
        speed = baseSpeed;
        usedSkillTimer01 = Random.Range(timeUsedSkill01Min, timeUsedSkill01Max); 
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        status = 0;
        bornTimer = timeBorn;
    }

    // Update is called once per frame
    void Update()
    {
        if (!usedSkill01)
        {
            usedSkillTimer01 -= Time.deltaTime;
            if (usedSkillTimer01 < 0)
            {
                usedSkill01 = true;
                skill01Timer = timeSkill01;
                speed = skill01Speed;
            }
        }
        
        if (usedSkill01)
        {
            skill01Timer -= Time.deltaTime;
            if (skill01Timer < 0)
            {
                usedSkill01 = false;
                usedSkillTimer01 = Random.Range(timeUsedSkill01Min, timeUsedSkill01Max);
                speed = baseSpeed;
            }
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;

            }
        }

        Vector2 position = rigidbody2d.position;

        if (!usedSkill01)
        {
            if (target.position.x > position.x)
                horizontal = 1;
            else if (target.position.x < position.x)
                horizontal = -1;
            else horizontal = 0;
        }

        if (!usedSkill02 && currentHealth <= maxHealth*0.5)
        {
            usedSkill02 = true;
            baseSpeed *= mulSkill02Speed;
            speed = baseSpeed;
        }

        //Born Time
        if (status == 1)
        {
            animator.SetTrigger("Trigger");
            bornTimer -= Time.deltaTime;
            if (bornTimer < 0)
            {
                gameObject.layer = 9; // layer "Enemy"
                UpdateStatus(2);
                animator.SetTrigger("Idle");
            }
        }

        //Update Death status
        if (currentHealth <= 0)
        {
            UpdateStatus(3);
        }

        if (status == 3)
        {
            animator.SetTrigger("Dead");
            gameObject.layer = 14; // layer "EnemyGhost"
            Debug.Log("Slime King Dead");
        }
      
    }

    private void FixedUpdate()
    {

        if (status == 2)
        {



            float moveBy = horizontal * speed * Time.deltaTime;
            if (!isKnockBack)
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
        if (status != 2)
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
            if (!usedSkill01)
            {
                isKnockBack = true;

                Debug.Log("Knockback " + knockBack.x);

                
                //PlaySound(playerHitClip);
            }
            animator.SetTrigger("Hurt");
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log("Boss Health: " + currentHealth);
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

    public void UpdateStatus(int s)
    {
        status = s;
    }
}
