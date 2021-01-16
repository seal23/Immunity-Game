using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public Button newGameBT, continueBT, optionBT, exitBT;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(GameObject.Find("Player"));
        newGameBT.onClick.AddListener(newGame);
        continueBT.onClick.AddListener(continueGame);
        optionBT.onClick.AddListener(optionGame);
        exitBT.onClick.AddListener(exitGame);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void newGame(){
        var parameters = new LoadSceneParameters(LoadSceneMode.Single);
        SceneManager.LoadScene("Village v0.1");
    }

    void continueGame(){
         Debug.Log("Button continue click");
    }

    void optionGame(){
         Debug.Log("Button option click");
    }

    void exitGame(){
        Debug.Log("exit");
         Application.Quit();
    }
}
