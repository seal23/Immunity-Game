using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss03Controller : MonoBehaviour
{
    public GameObject nextLevelDoor;
    bool isChild;
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
    public GameObject drop4;
    public GameObject drop5;

    public GameObject projectilePrefab;
    public GameObject projectilePrefab2;
    //GameObject projectileObject3;

    //Skill01 config
    float skill01Timer;
    bool usedSkill01 = false;
    int typeSkill01;
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
        typeSkill01 = 0;
        isChild = false;
        flag = 0;
        maxHealthSkill = (int)(maxHealth * mulHealthSkill);
        healthSkill = maxHealthSkill;
        target = GameObject.FindGameObjectWithTag("Player");
        gameObject.layer = 21; // layer "BossGhost"
        speed = baseSpeed;

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        status = 1;
        bornTimer = timeBorn;
        deadTimer = timeDead;
        targetPosition = target.transform.position;
        mySpriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

        if (status == 2)
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
                    typeSkill01 = Random.Range(0, 2);
                    Debug.Log("TypeSkill: " + typeSkill01);
                    if (typeSkill01 == 1)
                        Launch2();

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
                    typeSkill01 = 0;
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
                targetPosition = target.transform.position;
                speed = skill01Speed;
                gameObject.layer = 21; // layer BossGhost
            }

            if (usedSkill02)
            {
                
                UpdateTargetPosition(targetPosition);
                usedSkill02Timer -= Time.deltaTime;
                if (usedSkill02Timer < 0)
                {
                    usedSkill02 = false;
                    mySpriteRenderer.color = baseColor;
                    speed = baseSpeed;
                    gameObject.layer = 19; // set layer FlyingBoss
                    //target.GetComponent<PlayerController>().Stuned(timeStuned);
                }
            }
        }


        //Born Time
        if (status == 1)
        {
            //animator.Play("Born");
            bornTimer -= Time.deltaTime;
            if (bornTimer < 0)
            {
                gameObject.layer = 19; // layer "FlyingBoss"
                UpdateStatus(2);
                animator.Play("Moving");
            }
        }

        //Update Death status
        if (currentHealth <= 0)
        {
            if (flag == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    int rand = Random.Range(1, 101);
                    if (rand < 60)
                    {
                        int randitem = Random.Range(1, 4);
                        switch (randitem)
                        {
                            case 1:
                                Instantiate(drop1, rigidbody2d.position + new Vector2(randitem * 0.2f, randitem * 0.3f), Quaternion.identity);
                                break;
                            case 2:
                                Instantiate(drop2, rigidbody2d.position + new Vector2(randitem * 0.2f, randitem * 0.3f), Quaternion.identity);
                                break;
                            case 3:
                                Instantiate(drop3, rigidbody2d.position + new Vector2(randitem * 0.2f, randitem * 0.3f), Quaternion.identity);
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
                            case 1:
                                Instantiate(drop4, rigidbody2d.position, Quaternion.identity);
                                break;
                            case 2:
                                Instantiate(drop5, rigidbody2d.position, Quaternion.identity);
                                break;
                            default: break;
                        }
                    }
                }
                PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();
                player.getPlayerInfo().addExp(maxHealth * 2);
                player.gold = player.gold + (atk / 2);
                player.ChangeMana(10);
                flag = 1;
            }

            //nextLevelDoor.SetActive(false);
            UpdateStatus(3);
        }
        if (status == 3)
        {
            animator.Play("Dead");
            gameObject.layer = 21; // layer "BossGhost"
            Debug.Log("Slime King Dead");
            target.GetComponent<PlayerController>().NextBoss(3);

            if (deadTimer < 0)
            {
                rigidbody2d.gravityScale = 1f;
                rigidbody2d.constraints = RigidbodyConstraints2D.None;
                if (!isChild)
                {
                    Launch();
                    isChild = true;
                }
                   
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

        if (status == 2)
        {

            float moveX = horizontal * speed * Time.deltaTime;
            float moveY = vertical * speed * Time.deltaTime;
            if (!isKnockBack && typeSkill01 != 1)
                rigidbody2d.velocity = new Vector2(moveX, moveY);
            //rigidbody2d.velocity = new Vector2(moveX, rigidbody2d.velocity.y);
            else if (isKnockBack)
            {
                if (typeSkill01 != 1)
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
        if (player != null && !usedSkill02)
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

    void SetChild(int health)
    {
        isChild = true;
        maxHealth = health;
        currentHealth = maxHealth;
    }
    void Launch()
    {
        //Tao projectile
        GameObject projectileObject1 = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.left * 1.5f + Vector2.up * 1f, Quaternion.identity);
        GameObject projectileObject2 = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.left * 0f + Vector2.up * 0f, Quaternion.identity);
        GameObject projectileObject3 = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.right * 1.5f + Vector2.up * 1f, Quaternion.identity);

        Boss03Controller projectile1 = projectileObject1.GetComponent<Boss03Controller>();
        Boss03Controller projectile2 = projectileObject2.GetComponent<Boss03Controller>();
        Boss03Controller projectile3 = projectileObject3.GetComponent<Boss03Controller>();
        //Vector2 direction1 = new Vector2(-1f, 0.5f);
        //Vector2 direction2 = new Vector2(-0.6f, 0.5f);
        //Vector2 direction3 = new Vector2(1f, 0.5f);
        projectile1.SetChild((int)(maxHealth * 0.7f));
        projectile2.SetChild((int)(maxHealth * 0.7f));
        projectile3.SetChild((int)(maxHealth * 0.7f));

        //animator.SetTrigger("Launch");
    }

    void Launch2()
    {
        //Tao projectile

        float xSpread = Random.Range(-1, 2);
        
        float ySpread = Random.Range(-1, 1);
        Vector2 direction1;
        List <GameObject> projectileObject = new List<GameObject>();
        for (int i =0; i<10; i++)
        {
            GameObject newProjectileObject = Instantiate(projectilePrefab2, rigidbody2d.position + Vector2.left * 0f + Vector2.up * 0f, Quaternion.identity);

            projectileObject.Add(newProjectileObject);
            xSpread = Random.Range(-1, 2);
            ySpread = Random.Range(-1, 2);
            if (xSpread<=0)
            {
                xSpread = Random.Range(-1.5f, -0.5f);
            }
            else if (xSpread > 0)
            {
                xSpread = Random.Range(0.5f, 1.5f);
            }
            if (ySpread <=0)
            {
                ySpread = Random.Range(-1.5f, -0.5f);
            }
            else if (ySpread > 0 || (ySpread == 0 && xSpread == 0))
            {
                ySpread = Random.Range(0.5f, 1.5f);
            }

            Debug.Log("xSpread: " + xSpread + " ySpread: " + ySpread);

            direction1 = new Vector2(xSpread, ySpread);
            BossBullet projectile1 = newProjectileObject.GetComponent<BossBullet>();
            projectile1.SetDirection(direction1);
        }
        //animator.SetTrigger("Launch");
    }

}
