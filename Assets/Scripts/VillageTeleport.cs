using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;
public class VillageTeleport : MonoBehaviour
{
    public int level = 1;
    public string PositionName = "Gate01";
    // Start is called before the first frame update
    private string levelSceneName = "";
    private string TeleportGameObjName = "Gate01";

    void Start()
    {
        var playerGameObj = GameObject.Find("Player");
        if (playerGameObj != null)
        {
            levelSceneName = LevelManager.getSceneNameByLevel(level);
            //levelSceneName = LevelManager.getSceneNameByLevel(playerGameObj.GetComponent<PlayerController>().getLevel());
        }
    }

    // Update is called once per frame
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
            playerGameObj = collision.gameObject;
            var parameters = new LoadSceneParameters(LoadSceneMode.Single);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(levelSceneName);
        }

    }

    private GameObject playerGameObj;
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        var listObj = arg0.GetRootGameObjects();
        Debug.Log(listObj.Length);

        foreach (var gameObj in listObj)
        {

            //if (gameObj.name.Equals(TeleportGameObjName))
            //{
            //    player.transform.position = gameObj.transform.position;
            //}
            var tele = gameObj.GetComponent<Teleport>();
            if (tele != null && tele.PositionName.Equals(TeleportGameObjName))
            {
                Debug.Log(tele.PositionName);
                if (playerGameObj!=null) {
                    var rigidbody2d = playerGameObj.GetComponent<Rigidbody2D>();
                    rigidbody2d.MovePosition(gameObj.transform.position);
                    Debug.Log("tele: " + gameObj.transform.position);
                    Debug.Log("player: " + rigidbody2d.transform.position);
                }

                break;
            }
        }
    }

    
}
