using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss02Trigger : MonoBehaviour
{
    public Boss02Controller Boss02;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Boss02.UpdateStatus(1);
            Destroy(gameObject);
        }
    }
}
