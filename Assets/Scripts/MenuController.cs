using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  

public class MenuController : MonoBehaviour
{
    private static MenuController instance = null;

    private void Awake()
    {
        GetInstance();
    }

    public MenuController GetInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        return instance;
    }

    public Button menuBT, continueBT, optionBT, exitBT, saveBT;
    // Start is called before the first frame update
    void Start()
    {
        menuBT.onClick.AddListener(menuGame);
        saveBT.onClick.AddListener(saveGame);
        continueBT.onClick.AddListener(continueGame);
        optionBT.onClick.AddListener(optionGame);
        exitBT.onClick.AddListener(exitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void menuGame(){
        var parameters = new LoadSceneParameters(LoadSceneMode.Single);
        SceneManager.LoadScene("StartMenu");
    }

    void saveGame(){
        GameObject.Find("Player").GetComponent<PlayerController>().SaveGame();
        Destroy(this.gameObject);
    }

    void continueGame(){
         Destroy(this.gameObject);
    }

    void optionGame(){
         Debug.Log("Button option click");
    }

    void exitGame(){
        Debug.Log("exit");
         Application.Quit();
    }
}
