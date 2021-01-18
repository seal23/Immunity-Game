using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBody : MonoBehaviour
{
    public GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeHealth(int amount)
    {
        Boss02Controller boss02 = boss.GetComponent<Boss02Controller>();
        if (boss02 != null)
        {
            boss02.ChangeHealth(amount);
        }
    }
    
}
