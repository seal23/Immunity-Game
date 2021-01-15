using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private int LV;
    private int HP;
    private int MP;
    private int ATK;
    private int DEF;
    private float  EXP;

    public void setLV(int lv, float exp)
    {
        LV = lv;
        HP = 100 + 50*LV;
        MP = 100 + 10*LV;
        ATK = 10 + LV*5;
        DEF = 1 + LV*1;
        EXP = exp;
    }

    public PlayerInfo()
    {
        LV = 1;
        HP = 100;
        MP = 100;
        ATK = 10;
        DEF = 1;
        EXP = 0;
    }

    public void addExp(float exp)
    {
        EXP += exp;
        float MaxEXP= 100 * Mathf.Pow(2, LV);
        
        if (EXP >= MaxEXP)
        {
            float n = EXP - MaxEXP;
            setLV(LV+1, n);
        }
    }

    public int getLV() {return LV;}
    public int getHP() {return HP;}
    public int getMP() {return MP;}
    public int getATK() {return ATK;}
    public int getDEF() {return DEF;}
    public float getEXP() {return EXP;}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
