using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider sliderHP;
    public Slider sliderMP;
    public Text textLv;
    public Text textGold;
    public GameObject gameObject;
    public Text textScroll;
    public Text textHPP;
    public Text textMPP;

    public Image scrima;
    public Image hpp;
    public Image mpp;

    public Button hpBT;

    PlayerController player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        hpBT.onClick.AddListener(hpBTClick);
    }

    void Update()
    {
        checkColor(player.scroll,scrima);
        checkColor(player.hpPotion,hpp);
        checkColor(player.mpPotion,mpp);
        setMana(player.currentMP);
        setGold(player.gold);
        setScroll(player.scroll);
        setHPP(player.hpPotion);
        setMPP(player.mpPotion);
    }

    public void setMaxHealth(int n) 
    {
        sliderHP.maxValue = n;
        sliderHP.value = n;
    }

    public void setHealth(int n) 
    {
        sliderHP.value = n;
    }

    public void setMaxMana(int n) 
    {
        sliderMP.maxValue = n;
        sliderMP.value = n;
    }

    public void setMana(int n) 
    {
        sliderMP.value = n;
    }

    public void setLevel(int n) 
    {
        textLv.text = "LV "+ n;
    }

    public void setGold(int n) 
    {
        textGold.text = "" + n;
    }

    public void setScroll(int n) 
    {
        textScroll.text = "" + n;
    }

    public void setHPP(int n) 
    {
        textHPP.text = "" + n;
    }

    public void setMPP(int n) 
    {
        textMPP.text = "" + n;
    }

    public void IsActive(bool n)
    {
        gameObject.SetActive(n);
    }

    public void setColor(Image i, byte r, byte g, byte b, byte a)
    {
        i.color = new Color32(r,g,b,a);
    }

    public void checkColor(int n, Image i)
    {
        if (n == 0)
        {
            setColor(i,55,55,55,255);
        }
        else 
        { 
            setColor(i,255,255,255,255);
        }
    }

    void hpBTClick()
    {
        player.useHPP();
    }
}
