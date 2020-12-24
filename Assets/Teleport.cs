using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    // Start is called before the first frame update
    public string PositionName = "";
    public string DestinationSceneName= "";
    public string DestinationTeleportName = "";

    public static Dictionary<string, Teleport> listTeleports = new Dictionary<string, Teleport>();
    void Start()
    {
        //if (!listTeleports.ContainsKey(PositionName))
        //{
        //    listTeleports.Add(PositionName, this);
        //    Debug.Log("start");

        //    Debug.Log(PositionName);
        //}
    }

    private void Awake()
    {
        if (!listTeleports.ContainsKey(PositionName))
        {
            listTeleports.Add(PositionName, this);
            Debug.Log("awake");

            Debug.Log(PositionName);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();
        var destinationScene = SceneManager.GetSceneByName(DestinationSceneName);
        if(player!= null && destinationScene != null)
        {
            SceneManager.LoadScene(DestinationSceneName);
            //var gameObj = destinationScene.GetRootGameObjects();
            if (listTeleports.ContainsKey(DestinationTeleportName))
            {
                player.transform.position = listTeleports[DestinationTeleportName].transform.position;
            }
        }
    }
}
