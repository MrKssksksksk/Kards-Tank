using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TankItemEffectScript : MonoBehaviour
{
    private TankDataScript tankDataScript;
    private TankLogicScript tankLogicScript;
    private TankLogicScript enemyTankLogicScript;
    private ItemManagerScript itemManagerScript;
    private AudioManagerScript audioManagerScript;
    public List<UnityAction> always, useItem, drawItem, enemyUseItem, enemyDrawItem, removeShock, bulletHitEnemy, bulletHitSelf, bulletHitNothing;
    private Dictionary<int, Coroutine> effectLastingTime = new Dictionary<int, Coroutine>();

    private void Start()
    {
        tankDataScript = GetComponent<TankDataScript>();
        tankLogicScript = GetComponent<TankLogicScript>();
        enemyTankLogicScript = tankLogicScript.Enemy.GetComponent<TankLogicScript>();
        itemManagerScript = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManagerScript>();
        audioManagerScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
        always = new List<UnityAction>
        {
            presenceEffect10_1
        };
        useItem = new List<UnityAction>
        {
            itemEffect0, itemEffect1, itemEffect2, itemEffect3,
            itemEffect4, itemEffect5, itemEffect6, itemEffect7,
            itemEffect8, itemEffect9, itemEffect10, itemEffect11,
            itemEffect12, itemEffect13, itemEffect14, itemEffect15
        };
        drawItem = new List<UnityAction>
        {

        };
        enemyUseItem = new List<UnityAction>
        {
            presenceEffect8
        };
        enemyDrawItem = new List<UnityAction>
        {

        };
        removeShock = new List<UnityAction>
        {
            
        };
        bulletHitEnemy = new List<UnityAction>
        {

        };
        bulletHitSelf = new List<UnityAction>
        {
            presenceEffect10_2
        };
        bulletHitNothing = new List<UnityAction>
        {
            presenceEffect10_2
        };
    }

    void itemEffect0() // 银行
    {
        StartCoroutine(itemCoroutine0());
    }
    IEnumerator itemCoroutine0()
    {
        yield return new WaitForSeconds(3f);
        tankDataScript.cSupply += 12;
        tankDataScript.effects[3] = true; // ban道具
    }

    void itemEffect1() // 压制炮
    {
        audioManagerScript.PlaySfx(18); // 苏联炮部署
        tankLogicScript.pushBullet(2); // 85mm
    }

    void itemEffect2() // 蟑螂车
    {
        audioManagerScript.PlaySfx(0); // 小坦克移动
        tankLogicScript.pushBullet(3); // ib
    }

    void itemEffect3() // 转折点
    {
        effectLastingTime[5] = StartCoroutine(itemCoroutine3());
    }
    IEnumerator itemCoroutine3()
    {
        audioManagerScript.PlaySfx(7); // 转折点
        tankDataScript.effects[5] = true;
        yield return new WaitForSeconds(7f);
        tankDataScript.effects[5] = false;
    }

    void itemEffect4() // T-35
    {
        // nothing happens
    }

    void itemEffect5() // 列车
    {
        // nothing happens
    }

    void itemEffect6() // 红魔
    {
        // nothing happens
    }

    void itemEffect7() // 偷袭
    {
        audioManagerScript.PlaySfx(9); // 偷袭
        tankDataScript.effects[6] = true;
    }

    void itemEffect8() // 金kv
    {
        // nothing happens
    }
    void presenceEffect8()
    {
        foreach (GameObject item in itemManagerScript.PlayerItems[gameObject])
        {
            if (item.GetComponent<ItemLogicScript>().MyData.Id == 8) // 金kv
            {
                audioManagerScript.PlaySfx(14);
                tankLogicScript.Enemy.GetComponent<TankLogicScript>().damage(30);
                item.GetComponent<ItemLogicScript>().MyData.data++;
                if (item.GetComponent<ItemLogicScript>().MyData.data >= 4) itemManagerScript.PlayerItems[gameObject].Remove(item);
            }
        }
    }

    void itemEffect9() // 海军力量
    {
        audioManagerScript.PlaySfx(13); // 海军力量
        enemyTankLogicScript.pin();
        tankDataScript.cHP += 30;
    }

    void itemEffect10() // 守冲
    {
        // 无
    }
    void presenceEffect10_1() // 无法失去冲击
    {
        if (tankDataScript.effects[1] == false) tankLogicScript.giveShock();
    }
    void presenceEffect10_2() // 未命中时失去此道具
    {
        foreach (GameObject item in itemManagerScript.PlayerItems[gameObject])
        {
            if (item.GetComponent<ItemLogicScript>().MyData.Id == 10) // 守冲
            {
                // sfx
                itemManagerScript.PlayerItems[gameObject].Remove(item);
            }
        }
    }

    void itemEffect11() // 闪击战法
    {
        audioManagerScript.PlaySfx(11); // 闪击战法
        itemManagerScript.DevelopItem(gameObject, "TANK");
        itemManagerScript.GiveItem(gameObject, 12);
    }

    void itemEffect12() // 竞争战法
    {
        audioManagerScript.PlaySfx(12); // 竞争战法
        itemManagerScript.DevelopItem(gameObject, "TANK");
        for (int i = 0; i < tankDataScript.FireConsumption.Count; i++)
        {
            if (tankDataScript.FireConsumption[i] >= 2.5f) tankDataScript.FireConsumption[i] -= 0.5f; // 最多减到2
        }
        tankDataScript.armorThickness += 5;
        tankDataScript.hardDamage += 5;
    }

    void itemEffect13() // 蒙哥马利
    {
        // 音效
        enemyTankLogicScript.pin();
        itemManagerScript.DevelopItem(gameObject, "ORDER");
    }

    void itemEffect14() // 沙漠之鼠
    {
        // 音效
        enemyTankLogicScript.pin();
        enemyTankLogicScript.damage(10);
    }

    void itemEffect15() // 暗隐袭击
    {
        // 
        tankLogicScript.giveShock();
        tankDataScript.effects[8] = true; // 暗隐袭击：冲击结束后给一个烟幕
    }
}
