using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class PlayerInfomation : MonoBehaviour
{
    // Start is called before the first frame update
    private static PlayerInfomation instance = null;

    public float Health;

    void Start()
    {
        if(instance == null)
        {
            instance = new PlayerInfomation();
        }
    }

    private void Awake()
    {
        GetInstance();

        DontDestroyOnLoad(gameObject);
    }
    public PlayerInfomation GetInstance()
    {
        if(instance == null)
        {
            instance = new PlayerInfomation();
        }
        
        return instance;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    
}
