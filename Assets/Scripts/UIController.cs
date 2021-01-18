﻿using System.Collections;
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

    public void setMaxHealth(int health) 
    {
        sliderHP.maxValue = health;
        sliderHP.value = health;
    }

    public void setHealth(int health) 
    {
        sliderHP.value = health;
    }

    public void setMaxMana(int mana) 
    {
        sliderMP.maxValue = mana;
        sliderMP.value = mana;
    }

    public void setMana(int mana) 
    {
        sliderMP.value = mana;
    }

    public void setLevel(int lv) 
    {
        textLv.text = "LV "+ lv;
    }

    public void setGold(int g) 
    {
        textGold.text = "" + g;
    }

    public void IsActive(bool n)
    {
        gameObject.SetActive(n);
    }
}
