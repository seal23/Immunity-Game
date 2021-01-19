using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01Controller : MonoBehaviour
{
    public GameObject nextLevelDoor;
    float horizontal;
    GameObject target;
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

    int flag;
    public GameObject drop1;
    public GameObject drop2;
    public GameObject drop3;
    public GameObject drop4;
    public GameObject drop5;

    //Skill01 config
    float skill01Timer;
    bool usedSkill01 = false;
    public float timeSkill01 = 2.0f;
    public float timeUsedSkill01Min = 4.0f;
    public float timeUsedSkill01Max = 7.0f;
    float usedSkillTimer01;
    public float skill01Speed = 150.0f;

    bool usedSkill02 = false;
    public float timeUsedSkill02 = 20.0f;
    float usedSkill02Timer;
    public float mulSkill02Speed = 1.8f;
    public GameObject projectilePrefab;

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
        target = GameObject.Find("Player");
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
            if (target.transform.position.x > position.x)
                horizontal = 1;
            else if (target.transform.position.x < position.x)
                horizontal = -1;
            else horizontal = 0;
        }

        if (!usedSkill02 && currentHealth <= maxHealth*0.5 && status == 2)
        {
            usedSkill02 = true;
            baseSpeed *= mulSkill02Speed;
            speed = baseSpeed;
            usedSkill02Timer = timeUsedSkill02;
            Launch();
        }

        if (usedSkill02)
        {
            usedSkill02Timer -= Time.deltaTime;
            if (usedSkill02Timer < 0)
            {
                usedSkill02 = false;
                baseSpeed /= mulSkill02Speed;
            }
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
                    int randrace = Random.Range(0, 101);
                    if (randrace < 10)
                    {
                        int randitem = Random.Range(1, 3);
                        switch (randitem)
                        {
                            case 1: Instantiate(drop4, rigidbody2d.position, Quaternion.identity);
                                break;
                            case 2: Instantiate(drop5, rigidbody2d.position, Quaternion.identity);
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

        EnemyController slime = collision.gameObject.GetComponent<EnemyController>();
        if (slime != null)
        {
            ChangeHealth((int)(maxHealth * 0.1));
            Destroy(collision.gameObject);
        }
    }

    public void UpdateStatus(int s)
    {
        status = s;
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void Launch()
    {
        //Tao projectile
        GameObject projectileObject1 = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.left * 1.5f + Vector2.up * 2.5f, Quaternion.identity);
        //GameObject projectileObject2 = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.left * 2f + Vector2.up * 2.5f, Quaternion.identity);
        GameObject projectileObject3 = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.right * 1.5f+ Vector2.up * 2.5f, Quaternion.identity);

        EnemyController projectile1 = projectileObject1.GetComponent<EnemyController>();
        //SlimeController projectile2 = projectileObject2.GetComponent<SlimeController>();
        EnemyController projectile3 = projectileObject3.GetComponent<EnemyController>();
        Vector2 direction1 = new Vector2(-1f, 0.5f);
        //Vector2 direction2 = new Vector2(-0.6f, 0.5f);
        Vector2 direction3 = new Vector2(1f, 0.5f);
        projectile1.Launch(direction1);
        //projectile2.Launch(direction2);
        projectile3.Launch(direction3);

        //animator.SetTrigger("Launch");
        
    }
}
