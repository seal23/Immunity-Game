using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;
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
        SceneManager.LoadScene(LevelManager.VillageSceneName);
    }

    void continueGame(){
        var parameters = new LoadSceneParameters(LoadSceneMode.Single);
        SceneManager.LoadScene(LevelManager.VillageSceneName);
        PlayerPrefs.SetInt("Loadmode", 1);
	    PlayerPrefs.Save();
        
    }

    void optionGame(){
        Debug.Log("Button option click");
    }

    void exitGame(){
        Debug.Log("exit");
         Application.Quit();
    }
}
