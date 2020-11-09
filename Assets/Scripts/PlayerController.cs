using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 5;
    public float speed = 50f;
    public Vector2 jumpHeight;
    public Vector2 dashDistance;
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


    bool isGround;
    bool facingRight = true;
    bool isDead;
    bool isInvincible;
    float invincibleTimer;

    bool isJumped = false;
    Rigidbody2D rigidbody2d;
    float horizontal;
    private SpriteRenderer mySpriteRenderer;

    //Hit var
    bool beginHit = false;
    bool isHit = false;
    float hitTimer;
    public float timeHit = 1f;
    public GameObject hitTriggerLeft;  // 
    public GameObject hitTriggerRight;

    //Vector2 lookDirection = new Vector2(1, 0);

    Animator animator;

    //public GameObject projectilePrefab;
    //public float projectileForce = 300f;

    //AudioSource audioSource;
    //public AudioClip cogShotClip;
    //public AudioClip playerHitClip;
    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        // audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        hitTriggerLeft.SetActive(false);
        hitTriggerRight.SetActive(false);
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
    }

    // Update is called once per frame
    void Update()
    {
        isGround = IsGround();
        horizontal = Input.GetAxis("Horizontal");
        //vertical = Input.GetAxis("Vertical");
        //Debug.Log("horizontal " + horizontal);
        if (horizontal > 0)
        {
            if (facingRight == false)
                Flip();
        }
        else if (horizontal < 0)
        {
            if (facingRight)
                Flip();
        }

        if (horizontal != 0 && isGround)
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
            {   if (!isDash && !isHit)
                {
                    beginDash = true;
                    isDash = true;
                    dashTimer = timeDash;
                 
                    
                    animator.SetBool("Run", false);
                }
               
            }

            // Sword Hit action
            if (Input.GetButtonDown("Fire1"))
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

        /*if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("fire1");
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
        */
    }

    private void FixedUpdate()
    {

        if (beginDash)
        {
            animator.SetBool("Dash", true);
            animator.SetBool("Jumped", false);
            
            beginDash = false;
            Dash();
        }

        if (horizontal != 0 && isDash == false && (isHit ==false || isGround==false))
        {

         
            //Vector2 position = rigidbody2d.position;
            //position.x = position.x + speed * horizontal * Time.deltaTime;
            //position.y = position.y + speed * vertical * Time.deltaTime;
            //rigidbody2d.MovePosition(position);
         
            float moveBy = horizontal * speed*Time.deltaTime;
            rigidbody2d.velocity = new Vector2(moveBy, rigidbody2d.velocity.y);

        }
      
        if (isJumped)
        {
            isJumped = false;
            Jump();
        }
        
        
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            //animator.SetTrigger("Hit");
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            //PlaySound(playerHitClip);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        //UiHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

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
