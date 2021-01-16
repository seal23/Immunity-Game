using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    private int neck;
    private int armor;
    private int ring;
    private int sword;
    private int boot;

    public int getNeck() {return neck;}
    public void setNeck(int n) {neck = n;}

    public int getArmor() {return armor;}
    public void setArmor(int n) {armor = n;}

    public int getRing() {return ring;}
    public void setRing(int n) {ring = n;}

    public int getSword() {return sword;}
    public void setSword(int n) {sword = n;}

    public int getBoot() {return boot;}
    public void setBoot(int n) {boot = n;}

    public ItemInfo()
    {
        neck = 1;
        armor = 1;
        sword = 1;
        boot = 1;
        ring = 1;
    }

    public void Set(int n1, int n2, int n3, int n4, int n5)
    {
        neck = n1;
        armor = n2;
        sword = n3;
        boot = n4;
        ring = n5;
    }

    void Start()
    {
        
    }

    void Update()
    {

    }
}
