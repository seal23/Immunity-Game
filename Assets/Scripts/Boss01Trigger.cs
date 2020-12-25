using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01Trigger : MonoBehaviour
{

    public Boss01Controller Boss01;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Boss01.UpdateStatus(1);
            Destroy(gameObject);
        }
    }
}
