using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemCollection : MonoBehaviour
{
    public int item; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            Destroy(this.gameObject);
            switch (item)
            {
                case 1: player.scroll = player.scroll + 1;
                    break;
                case 2: player.hpPotion = player.hpPotion + 1;
                    break;
                case 3: player.mpPotion = player.mpPotion + 1;
                    break;
                case 4: player.book = player.book + 1;
                    break;
                case 5: player.gem = player.gem + 1;
                    break;
                default: break;
            }
            
        }
    }
}
