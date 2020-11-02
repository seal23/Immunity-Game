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

    bool beginDash = false;
    bool isDash = false;
    bool isGround;
    bool facingRight = true;
    bool isDead;
    bool isInvincible;
    float invincibleTimer;
    float dashTimer;
    public float timeDash = 1f;
    bool isJumped = false;
    Rigidbody2D rigidbody2d;
    float horizontal;
    private SpriteRenderer mySpriteRenderer;
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
            animator.SetBool("Jumped", false);
        if (isGround==false)
        {
            animator.SetBool("Run", false);
        }


        //animator.SetFloat("Look X", lookDirection.x);
        //animator.SetFloat("Look Y", lookDirection.y);
        if (isDead == false)
        {
            if (Input.GetButtonDown("Jump")) //makes player jump
            {
                isJumped = true;
                
            }
            if (Input.GetButtonDown("Dash"))
            {   if (!isDash)
                {
                    beginDash = true;
                    isDash = true;
                    dashTimer = timeDash;
                 
                    
                    animator.SetBool("Run", false);
                }
               
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
            animator.SetBool("Dash", true);
            animator.SetBool("Jumped", false);
            animator.SetBool("Run", false);
            dashTimer -= Time.deltaTime;
            if (dashTimer<0)
            {
                isDash = false;
                animator.SetBool("Dash", false);
          
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
            beginDash = false;
            Dash();
        }

        if (horizontal != 0 && isDash == false)
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
