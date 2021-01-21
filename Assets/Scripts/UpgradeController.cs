using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradeController : MonoBehaviour
{
    
    public Button upgradeBT, neckBT, bootBT, ringBT, armorBT, swordBT, exitBT;

    public Text text;

    public GameObject itemImage;

    public GameObject costUpg;

    public Text costText;

    private static UpgradeController instance = null;

    PlayerController player;

    int temp = 0;

    int cost;

    private void Awake()
    {
        GetInstance();
    }

    public UpgradeController GetInstance()
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

    void Start()
	{
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		upgradeBT.onClick.AddListener(upgradeItem);
        neckBT.onClick.AddListener(() => itemShow(neckBT, 1));
        bootBT.onClick.AddListener(() => itemShow(bootBT, 2));
        ringBT.onClick.AddListener(() => itemShow(ringBT, 3));
        armorBT.onClick.AddListener(() => itemShow(armorBT, 4));
        swordBT.onClick.AddListener(() => itemShow(swordBT, 5));
        exitBT.onClick.AddListener(exitDialog);
        itemImage.SetActive(false);
        costUpg.SetActive(false);
	}

    void Initiate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Destroy(this.gameObject);
        }
        
        int lev=0;
        string str=null;
        switch (temp)
        {
            case 1:
                lev = player.getItem().getNeck();
                str = "Level:\t\t"+lev+"\t\t->\t\t"+(lev+1)+"\nHP:\t\t\t"+(10*lev)+"\t\t->\t\t"+(10*lev+10)+"\nDEF:\t\t"+1*lev+"\t\t->\t\t"+(1*lev+1);
                text.text = str;
                cost = lev*10 + (lev-1)*7;
                if (cost > player.gold) costText.color = Color.red;
                    else costText.color = Color.yellow;
                costText.text = "" + cost;
                costUpg.SetActive(true);
                break;
            case 2: 
                lev = player.getItem().getBoot();
                str = "Level:\t\t"+lev+"\t\t->\t\t"+(lev+1)+"\nHP:\t\t\t"+(10*lev)+"\t\t->\t\t"+(10*lev+10)+"\nDEF:\t\t"+1*lev+"\t\t->\t\t"+(1*lev+1);
                text.text = str;
                cost = lev*10 + (lev-1)*7;
                if (cost > player.gold) costText.color = Color.red;
                    else costText.color = Color.yellow;
                costText.text = "" + cost;
                costUpg.SetActive(true);
                break;
            case 3: 
                lev = player.getItem().getRing();
                str = "Level:\t\t"+lev+"\t\t->\t\t"+(lev+1)+"\nHP:\t\t\t"+(10*lev)+"\t\t->\t\t"+(10*lev+10)+"\nDEF:\t\t"+1*lev+"\t\t->\t\t"+(1*lev+1);
                text.text = str;
                cost = lev*10 + (lev-1)*7;
                if (cost > player.gold) costText.color = Color.red;
                    else costText.color = Color.yellow;
                costText.text = "" + cost;
                costUpg.SetActive(true);
                break;
            case 4:
                lev = player.getItem().getArmor();
                str = "Level:\t\t"+lev+"\t\t->\t\t"+(lev+1)+"\nHP:\t\t\t"+(10*lev)+"\t\t->\t\t"+(10*lev+10)+"\nDEF:\t\t"+1*lev+"\t\t->\t\t"+(1*lev+1);
                text.text = str;
                cost = lev*10 + (lev-1)*7;
                if (cost > player.gold) costText.color = Color.red;
                    else costText.color = Color.yellow;
                costText.text = "" + cost;
                costUpg.SetActive(true);
                break;
            case 5:
                lev = player.getItem().getSword();
                str = "Level:\t\t"+lev+"\t\t->\t\t"+(lev+1)+"\nATK:\t\t\t"+(5*lev)+"\t\t->\t\t"+(5*lev+5);
                text.text = str;
                cost = lev*10 + (lev-1)*20;
                if (cost > player.gold) costText.color = Color.red;
                    else costText.color = Color.yellow;
                costText.text = "" + cost;
                costUpg.SetActive(true);
                break;
            default: text.text = null; 
                itemImage.SetActive(false);
                costUpg.SetActive(false);
                break;
        }
    }

    void upgradeItem()
    {
        int lev=0;
        if (cost <= player.gold)
        {
            switch (temp)
            {
                case 1:
                    lev = player.getItem().getNeck();
                    player.getItem().setNeck(lev+1);
                    player.health = player.health+10;
                    break;
                case 2: 
                    lev = player.getItem().getBoot();
                    player.getItem().setBoot(lev+1);
                    player.health = player.health+10;
                    break;

                case 3: 
                    lev = player.getItem().getRing();
                    player.getItem().setRing(lev+1);
                    player.health = player.health+10;
                    break;
                case 4:
                    lev = player.getItem().getArmor();
                    player.getItem().setArmor(lev+1);
                    player.health = player.health+10;
                    break;
                case 5:
                    lev = player.getItem().getSword();
                    player.getItem().setSword(lev+1);
                    break;
                default: break;
            }
            player.gold = player.gold - cost;
        }
    }

    void itemShow(Button bt, int n)
    {
        itemImage.GetComponent<Image>().sprite = bt.GetComponent<Image>().sprite;
        itemImage.SetActive(true);
        temp = n;
    }

    void exitDialog()
    {
        Debug.Log("de");
        Destroy(this.gameObject);
    }
}
