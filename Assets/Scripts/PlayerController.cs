using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public UIController UIController;

    private static PlayerController instance = null;
    public GameObject cmVcam;
    CinemachineConfiner confiner;
    string currentScene;
    string updateScene;

    int maxHealth = 5;
    public float speed = 50f;
    public Vector2 jumpHeight;
    public Vector2 dashDistance;

    public Vector2 knockBack;
    bool isKnockBack = false;
    public float timeKnockBack = 0.4f;
    float knockBackTimer;

    bool isChild;
    public float timeInvincible = 2.0f;
    int currentHealth;

    public int health
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }
    // Dash Var
    bool beginDash = false;
    bool isDash = false;
    float dashTimer;
    public float timeDash = 1f;
    bool isStuned;
    float stunedTimer;
    
    bool isGround;
    bool facingRight = true;
    bool isDead;
    bool isInvincible;
    float invincibleTimer;

    bool isJumped = false;
    Rigidbody2D rigidbody2d;
    float horizontal;
    private SpriteRenderer mySpriteRenderer;
    public Color baseColor;
    public Color changeColor;
    public Color stunedColor;

    //Hit var
    bool beginHit = false;
    bool isHit = false;
    float hitTimer;
    public float timeHit = 1f;
    public GameObject hitTriggerLeft;  // 
    public GameObject hitTriggerRight;

    Vector2 lookDirection = new Vector2(1, 0);

    Animator animator;

    //Item
    ItemInfo item;
    public ItemInfo getItem() {return item;}
    //menu
    public GameObject menu;
    GameObject menuObject;
    //Player
    PlayerInfo playerInfo;
    public PlayerInfo getPlayerInfo() {return playerInfo;}
    int lv;
    int maxMP;
    public int currentMP{get; set;}
    int Atk;
    int Def;
    public int gold{get; set;}

    public int scroll { get; set; }
    public int hpPotion { get; set; }
    public int mpPotion { get; set; }
    public int book { get; set; }
    public int gem { get; set; }

    public GameObject lvupeffect;
    float timeLvup = 0;

    //public GameObject projectilePrefab;
    //public float projectileForce = 300f;

    //AudioSource audioSource;
    //public AudioClip cogShotClip;
    //public AudioClip playerHitClip;
    // Start is called before the first frame update

    void Start()
    {
        isChild = false;
        isDead = false;
        isStuned = false;
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        // audioSource = GetComponent<AudioSource>();

        playerInfo = new PlayerInfo();
        item = new ItemInfo();
        gold = 0;
        scroll = 2;
        hpPotion = 2;
        mpPotion = 2;
        book = 2;
        gem = 2;
        lv = playerInfo.getLV();
        maxHealth = playerInfo.getHP()+ (item.getArmor() + item.getBoot() + item.getNeck() + item.getRing())*10;
        maxMP = playerInfo.getMP();
        currentHealth = maxHealth;
        currentMP = 0;
        UIController.setMaxMana(maxMP);
        UIController.setMaxHealth(maxHealth);

        hitTriggerLeft.SetActive(false);
        hitTriggerRight.SetActive(false);
        knockBackTimer = -1;
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
        currentScene = SceneManager.GetActiveScene().name;
        FindConfiner();
        if (PlayerPrefs.HasKey("Loadmode"))
        {
            if(PlayerPrefs.GetInt("Loadmode")==1)
            {
                LoadGame();
                PlayerPrefs.SetInt("Loadmode", 0);
	            PlayerPrefs.Save();
            }
        }
    }

    private void Awake()
    {
        GetInstance();
    }

    public PlayerController GetInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }

        return instance;
    }

    // Update is called once per frame
    void Update()
    {
        //Player Info
        if (lv != playerInfo.getLV())
        {
            maxHealth = playerInfo.getHP()+ (item.getArmor() + item.getBoot() + item.getNeck() + item.getRing())*10;
            currentHealth = maxHealth;
            lvupeffect.SetActive(true);
            timeLvup = 0.6f;
        }
        if (timeLvup > 0){
            timeLvup -= Time.deltaTime;
        } else 
        {
            lvupeffect.SetActive(false);
        }
        Atk = playerInfo.getATK()+ item.getSword()*5;
        Def = playerInfo.getDEF()+ item.getArmor() + item.getBoot() + item.getNeck() + item.getRing();
        maxHealth = playerInfo.getHP()+ (item.getArmor() + item.getBoot() + item.getNeck() + item.getRing())*10;
        lv = playerInfo.getLV();
        UIController.setLevel(playerInfo.getLV());
        UIController.setMaxHealth(maxHealth);
        UIController.setHealth(currentHealth);
       
        updateScene = SceneManager.GetActiveScene().name;
        if (updateScene != currentScene)
        {
            currentScene = updateScene;
            FindConfiner();
        }

        if (isDead)
        {
            animator.SetTrigger("Dead");
            animator.SetBool("Run", false);
            animator.SetBool("Jumped", false);
            animator.SetBool("Dash", false);
            animator.SetBool("SwordHit", false);
            animator.SetBool("Fall", false);
            hitTriggerLeft.SetActive(false);
            hitTriggerRight.SetActive(false);
            mySpriteRenderer.color = baseColor;
            return;
        }
        if (currentHealth <= 0)
        {
            isDead = true;
        }
        
        isGround = IsGround();
        horizontal = Input.GetAxis("Horizontal");
        //vertical = Input.GetAxis("Vertical");
        //Debug.Log("horizontal " + horizontal);
        if (!isDash && !isHit)
        {
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
        if (horizontal != 0 && isGround && !isStuned)
            animator.SetBool("Run", true);
        else
            animator.SetBool("Run", false);


        if (isGround)
        {
            animator.SetBool("Jumped", false);
            animator.SetBool("Fall", false);
        }

        if (isGround==false)
        {
            animator.SetBool("Run", false);
            animator.SetBool("Fall", true);
        }

        if (isStuned)
        {
            stunedTimer -= Time.deltaTime;
            if (stunedTimer < 0)
            {
                isStuned = false;
                mySpriteRenderer.color = baseColor;
            }
        }

        //animator.SetFloat("Look X", lookDirection.x);
        //animator.SetFloat("Look Y", lookDirection.y);
        if (isDead == false)
        {
            if (Input.GetButtonDown("Jump")) //makes player jump
            {
                if (isHit == false)
                    isJumped = true;
                
            }
            if (Input.GetButtonDown("Dash"))
            {
                if (!isDash && !isHit && !isStuned && currentMP >= 5)
                {
                    beginDash = true;
                    isDash = true;
                    dashTimer = timeDash;
                    ChangeMana(-5);
                    
                    animator.SetBool("Run", false);
                }
               
            }

            // Sword Hit action
            if (Input.GetButtonDown("Fire1") && !isInvincible)
            {
                beginHit = true;
                //isHit = true;
                hitTimer = timeHit;
            }
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
                gameObject.layer = 8; // Dua ve layer "Player"
                if (isStuned)
                    mySpriteRenderer.color = stunedColor;
                else mySpriteRenderer.color = baseColor;
            }
        }
    
        
        if (isDash)
        {
            animator.SetBool("Run", false);
            dashTimer -= Time.deltaTime;
            if (dashTimer<0)
            {
                isDash = false;
                animator.SetBool("Dash", false);
          
            }
        }

        if (beginHit)
        {
            beginHit = false;
            SwordHit();
        }

        if (isHit)
        {

            if (facingRight)
                hitTriggerRight.SetActive(true);
            else
                hitTriggerLeft.SetActive(true);

            hitTimer -= Time.deltaTime;
            if (hitTimer<0)
            {
                isHit = false;
                animator.SetBool("SwordHit", false);
               
                hitTriggerRight.SetActive(false);
                hitTriggerLeft.SetActive(false);
            }

            

        }

        if (knockBackTimer >= 0)
        {
            knockBackTimer -= Time.deltaTime;
            if (knockBackTimer < 0)
            {
                isKnockBack = false;
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (menuObject == null) 
            {
                menuObject = Instantiate(menu);
                UIController.IsActive(false);
            }
            else 
            {
                Destroy(menuObject);
                menuObject = null;
            }
        }

        if (menuObject == null) 
        {
            UIController.IsActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            useScroll();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            useHPP();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            useMPP();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            useBook();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            useGem();
        }

        /*
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                Debug.Log("1");
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }*/

    }

    private void FixedUpdate()
    {
        if (isDead)
            return;

        if (beginDash)
        {
            animator.SetBool("Dash", true);
            animator.SetBool("Jumped", false);
            
            beginDash = false;
            Dash();
        }

        if (isKnockBack)
        {
            rigidbody2d.AddForce(knockBack, ForceMode2D.Impulse);
            isKnockBack = false;
        }
        else
        if (knockBackTimer < 0 && horizontal != 0 && isDash == false && (isHit == false || isGround == false) && !isStuned)
        {

         
            //Vector2 position = rigidbody2d.position;
            //position.x = position.x + speed * horizontal * Time.deltaTime;
            //position.y = position.y + speed * vertical * Time.deltaTime;
            //rigidbody2d.MovePosition(position);
         
            float moveBy = horizontal * speed*Time.deltaTime;
            rigidbody2d.velocity = new Vector2(moveBy, rigidbody2d.velocity.y);

        }

        if (isJumped && !isStuned)
        {
            isJumped = false;
            Jump();
        }
        
        
    }

    public void ChangeHealth(int amount)
    {
        int a = amount;
        if (amount < 0)
        {
            //animator.SetTrigger("Hit");
            if (isInvincible)
                return;

            mySpriteRenderer.color = changeColor;
            isInvincible = true;
            gameObject.layer = 13;
            invincibleTimer = timeInvincible;
           
            isKnockBack = true;
            knockBackTimer = timeKnockBack;
            //PlaySound(playerHitClip);
            a = Mathf.Clamp(Def + amount, amount, -1);
        }
        currentHealth = Mathf.Clamp(currentHealth + a, 0, maxHealth);
        Debug.Log("Player Health: " + currentHealth);
        //UiHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

    }

    public void ChangeMana(int amount)
    {
        currentMP = Mathf.Clamp(currentMP + amount, 0, maxMP);
    }

    private bool IsGround()
    {
        return transform.Find("GroundCheck").GetComponent<GroundCheck>().isGround;
    }

    void Jump()
    {
        Debug.Log("Jumped Ground " + isGround);
        if (isGround)
        {
            animator.SetBool("Jumped", true);
            animator.SetBool("Run", false);
            rigidbody2d.AddForce(jumpHeight, ForceMode2D.Impulse);
        }
    }

    void Dash()
    {
        Debug.Log("Dash");
        if ((facingRight && dashDistance.x<0) || (facingRight == false && dashDistance.x>0))
        {
            dashDistance.x *= -1;
        }
   
        rigidbody2d.velocity = new Vector2(dashDistance.x * Time.deltaTime, rigidbody2d.velocity.y);
        //rigidbody2d.AddForce(dashDistance, ForceMode2D.Impulse);
    }

    void SwordHit()
    {
        hitTimer = timeHit;
        isHit = true;
        animator.SetBool("SwordHit", true);
        animator.SetBool("Dash", false);
        animator.SetBool("Jumped", false);
        animator.SetBool("Run", false);
        

    }

    void Flip()
    {
        facingRight = !facingRight;
        
        mySpriteRenderer.flipX = !mySpriteRenderer.flipX;
        Debug.Log("Flip " + mySpriteRenderer.flipX);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Boss01Controller boss01 = collision.gameObject.GetComponent<Boss01Controller>();
        if (boss01 != null)
        {
            boss01.ChangeHealth(-Atk);
        }
        
        EnemyController slime = collision.gameObject.GetComponent<EnemyController>();
        if (slime != null)
        {
            slime.ChangeHealth(-Atk);
        }

        Boss02Controller boss02 = collision.gameObject.GetComponent<Boss02Controller>();
        if (boss02 != null)
        {
            boss02.ChangeHealth(-Atk);
        }
        BossBody bossBody = collision.gameObject.GetComponent<BossBody>();
        if (bossBody != null)
        {
            bossBody.ChangeHealth(-Atk);
        }
        Boss03Controller boss03 = collision.gameObject.GetComponent<Boss03Controller>();
        if (boss03 != null)
        {
            boss03.ChangeHealth(-Atk);
        }
    }
        
    void FindConfiner()
    {
            confiner = cmVcam.GetComponent<CinemachineConfiner>();
        if(confiner != null)
        {
            confiner.InvalidatePathCache();
            confiner.m_BoundingShape2D = GameObject.FindGameObjectWithTag("Bound").GetComponent<Collider2D>();
        }
           
        
    }

    public void useHPP()
    {
        if (currentHealth < maxHealth && hpPotion > 0)
        {
            hpPotion -= 1;
            ChangeHealth(100);
        }
    }

    public void useMPP()
    {
        if (currentMP < maxMP && mpPotion > 0)
        {
            mpPotion -= 1;
            ChangeMana(20);
        }
    }

    public void useScroll()
    {
        if (scroll > 0)
        {
            scroll -= 1;
            // Return villa
        }
    }

    public void useBook()
    {
        if (book > 0)
        {
            book -= 1;
            playerInfo.setLV(playerInfo.getLV()+1,0);
        }
    }

    public void useGem()
    {
        if (gem > 0)
        {
            gem -= 1;
            gold = gold * 2;
        }
    }

    public void Stuned(float timeStuned)
    {
        isStuned = true;
        stunedTimer = timeStuned;
        Debug.Log("Player Stuned");
        mySpriteRenderer.color = stunedColor;

    }

    public void fullHealth()
    {
        currentHealth = maxHealth;
    }

    public void SaveGame()
    {
        PlayerPrefs.DeleteAll();
	    PlayerPrefs.SetInt("playerLV", playerInfo.getLV());
	    PlayerPrefs.SetFloat("exp", playerInfo.getEXP());
        PlayerPrefs.SetInt("gold", gold);
        PlayerPrefs.SetInt("scroll", scroll);
        PlayerPrefs.SetInt("hpp", hpPotion);
        PlayerPrefs.SetInt("mpp", mpPotion);
        PlayerPrefs.SetInt("book", book);
        PlayerPrefs.SetInt("gem", gem);
        PlayerPrefs.SetInt("neck", item.getNeck());
        PlayerPrefs.SetInt("armor", item.getArmor());
        PlayerPrefs.SetInt("ring", item.getRing());
        PlayerPrefs.SetInt("boot", item.getBoot());
        PlayerPrefs.SetInt("sword", item.getSword());
        PlayerPrefs.SetInt("currentHealth", currentHealth);
        PlayerPrefs.SetInt("currentMP", currentMP);
	    PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("playerLV"))
        {
            int playerLV = PlayerPrefs.GetInt("playerLV");
            float exp = PlayerPrefs.GetFloat("exp");
            playerInfo.setLV(playerLV, exp);
            Debug.Log(playerInfo.getLV());
            gold = PlayerPrefs.GetInt("gold");
            scroll = PlayerPrefs.GetInt("scroll");
            hpPotion = PlayerPrefs.GetInt("hpp");
            mpPotion = PlayerPrefs.GetInt("mpp");
            book = PlayerPrefs.GetInt("book");
            gem = PlayerPrefs.GetInt("gem");
            currentHealth = PlayerPrefs.GetInt("currentHealth");
            currentMP = PlayerPrefs.GetInt("currentMP");
            item.setNeck(PlayerPrefs.GetInt("neck"));
            item.setArmor(PlayerPrefs.GetInt("armor"));
            item.setRing(PlayerPrefs.GetInt("ring"));
            item.setBoot(PlayerPrefs.GetInt("boot"));
            item.setSword(PlayerPrefs.GetInt("sword"));
        }
    }


    /* void Launch()
     {
         //Tao projectile
         GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

         Projectile projectile = projectileObject.GetComponent<Projectile>();
         projectile.Launch(lookDirection, projectileForce);

         animator.SetTrigger("Launch");
         PlaySound(cogShotClip);
     }
     */

    /* public void PlaySound(AudioClip clip)
     {
         audioSource.PlayOneShot(clip);
     }
     */
}
