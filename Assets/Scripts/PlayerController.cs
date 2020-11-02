using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 5;
    public float speed = 3.0f;

    public float timeInvincible = 2.0f;

    int currentHealth;

    public int health
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    bool facingRight = true;
    
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
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

        if (horizontal != 0)
            animator.SetBool("Run", true);
        else
            animator.SetBool("Run", false);

        Vector2 move = new Vector2(horizontal, vertical);

        //animator.SetFloat("Look X", lookDirection.x);
        //animator.SetFloat("Look Y", lookDirection.y);
       
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
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
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        //position.y = position.y + speed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(position);
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
