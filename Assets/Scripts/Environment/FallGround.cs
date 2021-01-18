using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallGround : MonoBehaviour
{
    public float minGravityScales = 1.6f;
    public float maxGravityScales = 4f;

    public float timeDestroy = 4f;
    float destroyTimer;
    bool isFall;

    public float timeFade = 2f;
    float fadeTimer;
    bool isFade;
    public float timeChangeColor = 0.4f;
    float changeColorTimer;

    Rigidbody2D rigidbody2d;

    private SpriteRenderer mySpriteRenderer;
    public Color baseColor;
    public Color changeColor;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2d.bodyType = RigidbodyType2D.Static;
        isFade = false;
        isFall = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (isFall)
        {
            destroyTimer -= Time.deltaTime;
            if (destroyTimer < 0)
            {
                fadeTimer = timeFade;
                isFade = true;
                isFall = false;
                changeColorTimer = timeChangeColor;
            }
        }

        if (isFade)
        {
            fadeTimer -= Time.deltaTime;
            changeColorTimer -= Time.deltaTime;
            if (changeColorTimer < 0)
            {
                if (mySpriteRenderer.color == baseColor)
                {
                    mySpriteRenderer.color = changeColor;
                }
                else
                {
                    mySpriteRenderer.color = baseColor;
                }
                changeColorTimer = timeChangeColor;
            }

            if (fadeTimer < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Fall()
    {
        rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
        //rigidbody2d.angularDrag = 2f;
        rigidbody2d.constraints = RigidbodyConstraints2D.FreezePositionX;
        rigidbody2d.gravityScale = Random.Range(minGravityScales, maxGravityScales);
        
        destroyTimer = timeDestroy;
        isFall = true;
    }
}
