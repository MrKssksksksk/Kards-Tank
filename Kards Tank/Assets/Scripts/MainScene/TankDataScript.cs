using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class TankDataScript : MonoBehaviour
{
    public int playerIndex; // 0 or 1
    public string country, ally;
    /*
     * country
     * GER
     * SOV
     * ENG
     * USA
     * JAP
     * 
     * ally
     * POL
     * ITA
     * FIN
     * FRA
     * 
     */

    public int HP, armorThickness, hardDamage, softDamage, supplyCapacity, moveSpeed, turnSpeed, bulletSpeed, reloadTime,
        supplyIncreaseTime;
    public float supplyConsumptionPerMove, supplyConsumptionPerTurn;
    public float supplyComsumptionBonus; // <0 -> benificial, only work on move and turn
    public List<float> FireConsumption = new List<float>();
    // real-time datas
    public float cSupply; // c for current
    public int cHP;
    public float armorIntegrity;
    public Stack<int> specialBullets = new Stack<int>();
    public List<string> bulletNames = new List<string>();
    public List<int> items = new List<int>();
    public List<int> itemDatas = new List<int>();
    public List<int> itemCosts = new List<int>();
    public List<string> itemNames = new List<string>();
    public List<string> itemTags = new List<string>();
    public List<bool> effects = new List<bool>();
    public float pinTime;
    public int effect1BulletNum = 0;

    void Start()
    {
        cSupply = supplyCapacity;
        cHP = HP;
        if (country == "JAP") armorIntegrity = 0.8f;
        else armorIntegrity = 1.0f;
    }

}
