using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEssentialData
{
    public int HP, armorThickness, hardDamage, softDamage, supplyCapacity, moveSpeed, turnSpeed, bulletSpeed, reloadTime,
        supplyIncreaseTime;
    public float supplyConsumptionPerMove, supplyConsumptionPerTurn;
    public float supplyComsumptionBonus;
    public List<float> FireConsumption;
    // public Stack<int> specialBullets;
    public List<int> items;
    // public List<bool> effects;

    public TankEssentialData() { }

    public TankEssentialData(TankDataScript data)
    {
        input(data);
    }

    public void input (TankDataScript data)
    {
        HP = data.HP;
        armorThickness = data.armorThickness;
        hardDamage = data.hardDamage;
        softDamage = data.softDamage;
        supplyCapacity = data.supplyCapacity;
        moveSpeed = data.moveSpeed;
        turnSpeed = data.turnSpeed;
        reloadTime = data.reloadTime;
        supplyIncreaseTime = data.supplyIncreaseTime;
        supplyConsumptionPerMove = data.supplyConsumptionPerMove;
        supplyConsumptionPerTurn = data.supplyConsumptionPerTurn;
        supplyComsumptionBonus = data.supplyComsumptionBonus;
        FireConsumption = data.FireConsumption;
        // specialBullets = data.specialBullets;
        items = new List<int>();
        for (int i = 0; i < data.items.Count; i++)
        {
            items.Add(data.getId(i));
        }
        // effects = data.effects;
    }
    public void output(TankDataScript data)
    {
        data.HP = HP;
        data.armorThickness = armorThickness;
        data.hardDamage = hardDamage;
        data.softDamage = softDamage;
        data.supplyCapacity = supplyCapacity;
        data.moveSpeed = moveSpeed;
        data.turnSpeed = turnSpeed;
        data.reloadTime = reloadTime;
        data.supplyIncreaseTime = supplyIncreaseTime;
        data.supplyConsumptionPerMove = supplyConsumptionPerMove;
        data.supplyConsumptionPerTurn = supplyConsumptionPerTurn;
        data.supplyComsumptionBonus = supplyComsumptionBonus;
        data.FireConsumption = FireConsumption;
        // data.specialBullets = specialBullets;
        data.items = new List<GameObject>(items.Count); // 非Mono，无法使用Instantiate
        // data.effects = effects;
    }
}
