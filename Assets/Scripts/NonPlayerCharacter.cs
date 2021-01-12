using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NonPlayerCharacter : MonoBehaviour
{
    public GameObject ui;
    public 

    void Start()
    {

    }

    void Update()
    {
        if (triggerStay)
        {
            if (Input.GetKeyDown(KeyCode.X))
                Instantiate(ui);
        }
        else
        {
            
        }
    }

    private bool triggerStay = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered");
        if (collision.gameObject.CompareTag("Player"))
        {
            triggerStay = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exited");
        if (collision.gameObject.CompareTag("Player"))
        {
            triggerStay = false;
        }
    }
}
