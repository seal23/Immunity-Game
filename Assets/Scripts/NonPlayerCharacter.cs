using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NonPlayerCharacter : MonoBehaviour
{
    public GameObject gameObject;
    public Button exitDia, upgradeBT;

    void Start()
    {
        gameObject.SetActive(false);
        exitDia.onClick.AddListener(exitDialog);
        upgradeBT.onClick.AddListener(upgradeItem);
    }

    void Update()
    {
        if (triggerStay)
        {
            if (Input.GetKeyDown(KeyCode.X))
                isEnter = true;
        }
        else
        {
            
        }
    }
    
    void upgradeItem()
    {
        
    }

    void exitDialog()
    {
        gameObject.SetActive(false);
    }

    public void DisplayDialog()
    {
        gameObject.SetActive(true);
    }

    private bool isEnter = false;
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
            isEnter = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
       
        if (isEnter && collision.GetComponent<PlayerController>() != null)
        {
            DisplayDialog();
            isEnter = false;
        }

    }
}
