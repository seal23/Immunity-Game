using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    [SerializeField] private LayerMask platformLayerMask;
    public bool isGround;
    private void OnTriggerStay2D(Collider2D collision)
    {
        isGround = collision != null; // && (((1 << collision.gameObject.layer) & platformLayerMask) != 0);
        if (collision != null)
        {
            //Debug.Log("GroundCheck Collision with " + collision.gameObject);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGround = false;
    }

}
