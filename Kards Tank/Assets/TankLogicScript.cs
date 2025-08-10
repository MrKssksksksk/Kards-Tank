using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// using static UnityEditor.Progress;

public class TankLogicScript : MonoBehaviour
{
    public GameObject Enemy;
    public TankDataScript tankDataScript;
    public GameObject tankEffect;
    private float supplyIncreaseTimer, pinTimer;
    private float gameTimer;

    private void Start()
    {
        supplyIncreaseTimer = 0;
        gameTimer = 0;
    }

    private void Update()
    {
        gameTimer += Time.deltaTime;
        // ����
        if (supplyIncreaseTimer > tankDataScript.supplyIncreaseTime)
        {
            supplyIncreaseTimer = 0;
            if (tankDataScript.supplyCapacity - tankDataScript.cSupply >= 0
                && tankDataScript .supplyCapacity - tankDataScript.cSupply < 1) tankDataScript.cSupply = tankDataScript.supplyCapacity;
            else tankDataScript.cSupply += 1;
        }
        if (tankDataScript.cSupply <= tankDataScript.supplyCapacity)
        {
            supplyIncreaseTimer += Time.deltaTime;
            if (tankDataScript.effects[3] == true) tankDataScript.effects[3] = false; // ����
        }

        // ѹ��
        if (tankDataScript.effects[0] == true) 
        {
            pinTimer += Time.deltaTime;
            if (pinTimer > tankDataScript.pinTime)
            {
                tankDataScript.effects[0] = false;
            }
        }

        // ���
        if (tankDataScript.effect1BulletNum > 0)
        {
            tankDataScript.effects[4] = true;
        }
        else
        {
            tankDataScript.effects[4] = false;
        }

        // T-35
        if (gameTimer % 5 < Time.deltaTime) // ÿ5sһ�� ��8��
        {
            int t35Num = 0;
            if (tankDataScript.items.Count >= 1 && tankDataScript.items[0] == 4) 
            {
                t35Num++;
                // if (tankDataScript.itemDatas[0] == -1) tankDataScript.itemDatas[0] = 0;
                tankDataScript.itemDatas[0]++;
                if (tankDataScript.itemDatas[0] >= 8) removeItem(0);
            }
            if (tankDataScript.items.Count >= 2 && tankDataScript.items[1] == 4)
            {
                t35Num++;
                // if (tankDataScript.itemDatas[1] == -1) tankDataScript.itemDatas[1] = 0;
                tankDataScript.itemDatas[1]++;
                if (tankDataScript.itemDatas[1] >= 8) removeItem(1);
            }
            if (tankDataScript.items.Count >= 3 && tankDataScript.items[2] == 4)
            {
                t35Num++;
                // if (tankDataScript.itemDatas[2] == -1) tankDataScript.itemDatas[2] = 0;
                tankDataScript.itemDatas[2]++;
                if (tankDataScript.itemDatas[2] >= 8) removeItem(2);
            }
            for (int i = 0; i < t35Num; i++)
            {
                Enemy.GetComponent<TankLogicScript>().damage(5);
                damage(5);
            }
        }

        // װ���г�
        if (gameTimer % 10 < Time.deltaTime) // ÿ10sһ��
        {
            int trainNum = tankDataScript.items.FindAll(t => t == 5).Count;
            for (int i = 0; i < trainNum; i++)
            {
                pushBullet(4);
            }
        }

        // �س�
        if (tankDataScript.items.Contains(10))
        {
            tankDataScript.effects[1] = true;
        }

        

    }

    public void damage(int d)
    {
        if (!isImmune()) tankDataScript.cHP -= d;
    }

    [ContextMenu("pin")]
    public void pin()
    {
        tankDataScript.effects[0] = true;
        pinTimer = 0;
        GameObject e = Instantiate(tankEffect);
        e.GetComponent<TankEffectScript>().getType(0, tankDataScript.pinTime, transform);
    }

    public void giveItem(int id)
    {
        // ���������ʾ��������Ч
        if (tankDataScript.items.Count < 3)
        {
            tankDataScript.items.Add(id);
            // tankDataScript.itemDatas.Add(-1);
            tankDataScript.itemDatas.Add(0);
        }
        else
        {
            // ��������
        }
    }

    public void removeItem(int index)
    {
        tankDataScript.items.RemoveAt(index);
        tankDataScript.itemDatas.RemoveAt(index);
    }

    public void pushBullet(int id)
    {
        // ����
        tankDataScript.specialBullets.Push(id);
    }

    public void OnBulletHitEnemy()
    {

    }

    public void OnBulletNotHitEnemy()
    {
        Debug.Log("bullet not hit enemy");
        if (tankDataScript.items.Contains(10))
        {
            if (tankDataScript.items.Count >= 1 && tankDataScript.items[0] == 10) removeItem(0);
            if (tankDataScript.items.Count >= 2 && tankDataScript.items[1] == 10) removeItem(1);
            if (tankDataScript.items.Count >= 3 && tankDataScript.items[2] == 10) removeItem(2);
        }

    }

    public void OnEnemyUseItem()
    {
        Debug.Log("enemy use item"); 
        int KV1Num = tankDataScript.items.FindAll(t => t == 8).Count;
        if (tankDataScript.items.Count >= 1 && tankDataScript.items[0] == 8)
        {
            KV1Num++;
            tankDataScript.itemDatas[0]++;
            if (tankDataScript.itemDatas[0] >= 4) removeItem(0); // can use 4 times
        }
        if (tankDataScript.items.Count >= 2 && tankDataScript.items[1] == 8)
        {
            KV1Num++;
            tankDataScript.itemDatas[1]++;
            if (tankDataScript.itemDatas[1] >= 4) removeItem(1);
        }
        if (tankDataScript.items.Count >= 3 && tankDataScript.items[2] == 8)
        {
            KV1Num++;
            tankDataScript.itemDatas[2]++;
            if (tankDataScript.itemDatas[2] >= 4) removeItem(2);
        }
        for (int i = 0; i < KV1Num; i++) Enemy.GetComponent<TankLogicScript>().damage(30);
    }

    public bool canUseItem()
    {
        if (tankDataScript.effects[3] == false) return true;
        else return false;
    }

    public bool canMove()
    {
        if (tankDataScript.effects[0] == false) return true; // ѹ��
        return false;
    }

    public bool canFire()
    {
        if (tankDataScript.effects[0] == false) return true; // ѹ��
        return false;
    }

    public bool doConsumeSupply() // do not work when use items
    {
        if (tankDataScript.effects[5] == false) return true; // ת�۵�
        return false;
    }

    public bool isImmune()
    {
        if (tankDataScript.effects[4] == false) return false;
        else return true;
    }
}
