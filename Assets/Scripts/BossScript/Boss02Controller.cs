using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss02Controller : MonoBehaviour
{
    public GameObject nextLevelDoor;
    float horizontal;
    float vertical;
    GameObject target;
    public float baseSpeed = 60.0f;
    float speed;
    Rigidbody2D rigidbody2d;
    public int atk = 10;
    public int maxHealth = 100;
    Animator animator;

    public float timeBorn = 4.0f;
    float bornTimer;

    public float timeDead = 2f;
    float deadTimer;

    public Vector2 knockBack;
    bool isKnockBack;

    int flag;
    public GameObject drop1;
    public GameObject drop2;
    public GameObject drop3;

    //Skill01 config
    float skill01Timer;
    bool usedSkill01 = false;
    public float timeSkill01 = 2.0f;
    public float timeUsedSkill01Min = 4.0f;
    public float timeUsedSkill01Max = 7.0f;
    float usedSkillTimer01;
    public float skill01Speed = 150.0f;

    //Skill02 cofig
    bool usedSkill02 = false;
    int healthSkill;
    int maxHealthSkill;
    public float mulHealthSkill = 0.2f;
    public float mulAtkSkill = 0.2f;
    float usedSkill02Timer;
    public float timeUsedSkill02 = 1f;
    public float timeStuned = 2f;
    private SpriteRenderer mySpriteRenderer;
    public Color baseColor;
    public Color changeColor;

    Vector2 position;
    Vector2 targetPosition;
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
        flag = 0;
        maxHealthSkill = (int)(maxHealth * mulHealthSkill);
        healthSkill = maxHealthSkill;
        target = GameObject.Find("Player");
        gameObject.layer = 20; // layer "BossGhost"
        speed = baseSpeed;
        
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        status = 0;
        bornTimer = timeBorn;
        deadTimer = timeDead;

        mySpriteRenderer = GetComponent<SpriteRenderer>();
       
    }

    // Update is called once per frame
    void Update()
    {
       
        if (status ==2)
        {
            if (!usedSkill01 && !usedSkill02)
            {
                usedSkillTimer01 -= Time.deltaTime;
                if (usedSkillTimer01 < 0)
                {
                    usedSkill01 = true;
                    skill01Timer = timeSkill01;
                    speed = skill01Speed;

                    targetPosition = target.transform.position;
                    UpdateTargetPosition(targetPosition);

                }
            }

            if (usedSkill01)
            {
                UpdateTargetPosition(targetPosition);
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



            if (!usedSkill01)
            {
                position = rigidbody2d.position;
                if (target.transform.position.x > position.x)
                    horizontal = 1;
                else if (target.transform.position.x < position.x)
                    horizontal = -1;
                else horizontal = 0;
                if (target.transform.position.y > position.y)
                    vertical = 1;
                else if (target.transform.position.y < position.y)
                    vertical = -1;
                else vertical = 0;

            }

            //Skill 02 when lose amount of health (20%)
            if (healthSkill <= 0 && currentHealth > 0)
            {
                healthSkill = maxHealthSkill;
                usedSkill02 = true;
                usedSkill02Timer = timeUsedSkill02;
                mySpriteRenderer.color = changeColor;
            }

            if (usedSkill02)
            {
                usedSkill02Timer -= Time.deltaTime;
                if (usedSkill02Timer < 0)
                {
                    usedSkill02 = false;
                    mySpriteRenderer.color = baseColor;
                    target.GetComponent<PlayerController>().Stuned(timeStuned);
                }
            }
        }
        

        //Born Time
        if (status == 1)
        {
            animator.Play("Born");
            bornTimer -= Time.deltaTime;
            if (bornTimer < 0)
            {
                gameObject.layer = 18; // layer "FlyingBoss"
                UpdateStatus(2);
                animator.Play("Moving");
            }
        }

        //Update Death status
        if (currentHealth <= 0)
        {
            if(flag == 0) 
            {
                for (int i = 0; i < 5; i++)
                {
                    int rand = Random.Range(0, 101);
                    if (rand < 60)
                    {
                        int randitem = Random.Range(1, 4);
                        switch (randitem)
                        {
                            case 1: Instantiate(drop1, rigidbody2d.position + new Vector2(randitem*0.2f,randitem*0.3f), Quaternion.identity);
                                break;
                            case 2: Instantiate(drop2, rigidbody2d.position + new Vector2(randitem*0.2f,randitem*0.3f), Quaternion.identity);
                                break;
                            case 3: Instantiate(drop3, rigidbody2d.position + new Vector2(randitem*0.2f,randitem*0.3f), Quaternion.identity);
                                break;
                            default: break;
                        }
                    }
                }
                PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();
                player.getPlayerInfo().addExp(maxHealth*2);
                player.gold = player.gold+(atk/2);
                player.ChangeMana(10);
                flag = 1;
            }

            nextLevelDoor.SetActive(false);
            UpdateStatus(3);
        }
        if (status == 3)
        {
            animator.Play("Dead");
            gameObject.layer = 20; // layer "BossGhost"
            Debug.Log("Slime King Dead");
           
           
            if (deadTimer < 0)
            {
                rigidbody2d.gravityScale = 1f;
                rigidbody2d.constraints = RigidbodyConstraints2D.None;
            }
            else
            {
                rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
                deadTimer -= Time.deltaTime;
            }
        }

    }

    private void FixedUpdate()
    {

        if (status == 2 && !usedSkill02)
        {

            float moveX = horizontal * speed * Time.deltaTime;
            float moveY = vertical * speed * Time.deltaTime;
            if (!isKnockBack)
                rigidbody2d.velocity = new Vector2(moveX, moveY);
            //rigidbody2d.velocity = new Vector2(moveX, rigidbody2d.velocity.y);
            else
            {
                rigidbody2d.AddForce(knockBack, ForceMode2D.Impulse);
                isKnockBack = false;
            }
        }
    }

    public void ChangeHealth(int amount)
    {
        if (status != 2 || usedSkill02)
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
            if ((knockBack.y < 0 && vertical < 0) || (knockBack.y > 0 && vertical > 0))
            {
                knockBack.y = -knockBack.y;
            }
            if (!usedSkill01)
            {
                 isKnockBack = true;

                 Debug.Log("Knockback " + knockBack.x);


                 //PlaySound(playerHitClip);
            }
            isKnockBack = true;
            Debug.Log("Knockback " + knockBack.x);
            animator.Play("Hurt");
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        healthSkill += amount;
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

    void UpdateTargetPosition(Vector2 targetPosition)
    {
        position = rigidbody2d.position;
        if (targetPosition.x > position.x)
            horizontal = 1;
        else if (targetPosition.x < position.x)
            horizontal = -1;
        else horizontal = 0;
        if (targetPosition.y > position.y)
            vertical = 1;
        else if (targetPosition.y < position.y)
            vertical = -1;
        else vertical = 0;
    }

  
}
