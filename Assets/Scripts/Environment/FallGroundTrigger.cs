using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallGroundTrigger : MonoBehaviour
{

    public List<GameObject> fallGround;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            for (int i=0; i < fallGround.Count; i++)
            {
                fallGround[i].GetComponent<FallGround>().Fall();
            }
        }
    }
}
