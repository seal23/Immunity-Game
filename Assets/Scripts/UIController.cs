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
    public Text textBook;
    public Text textGem;

    public Image scrima;
    public Image hpp;
    public Image mpp;
    public Image bookima;
    public Image gemima;

    public Button scrBT, hpBT, mpBT, bookBT, gemBT;

    PlayerController player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        scrBT.onClick.AddListener(scrBTClick);
        hpBT.onClick.AddListener(hpBTClick);
        mpBT.onClick.AddListener(mpBTClick);
        bookBT.onClick.AddListener(bookBTClick);
        gemBT.onClick.AddListener(gemBTClick);
    }

    void Update()
    {
        checkColor(player.scroll,scrima);
        checkColor(player.hpPotion,hpp);
        checkColor(player.mpPotion,mpp);
        checkColor(player.book,bookima);
        checkColor(player.gem,gemima);

        setMana(player.currentMP);
        
        SetNum(player.gold, textGold);
        SetNum(player.scroll, textScroll);
        SetNum(player.hpPotion, textHPP);
        SetNum(player.mpPotion, textMPP);
        SetNum(player.book, textBook);
        SetNum(player.gem, textGem);
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

    public void SetNum(int n, Text t) 
    {
        t.text = "" + n;
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

    void scrBTClick()
    {
        player.useScroll();
    }

    void hpBTClick()
    {
        player.useHPP();
    }

    void mpBTClick()
    {
        player.useMPP();
    }

    void bookBTClick()
    {
        player.useBook();
    }

    void gemBTClick()
    {
        player.useGem();
    }
}
