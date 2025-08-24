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

    void itemEffect0() // ����
    {
        StartCoroutine(itemCoroutine0());
    }
    IEnumerator itemCoroutine0()
    {
        yield return new WaitForSeconds(3f);
        tankDataScript.cSupply += 12;
        tankDataScript.effects[3] = true; // ban����
    }

    void itemEffect1() // ѹ����
    {
        audioManagerScript.PlaySfx(18); // �����ڲ���
        tankLogicScript.pushBullet(2); // 85mm
    }

    void itemEffect2() // ��복
    {
        audioManagerScript.PlaySfx(0); // С̹���ƶ�
        tankLogicScript.pushBullet(3); // ib
    }

    void itemEffect3() // ת�۵�
    {
        effectLastingTime[5] = StartCoroutine(itemCoroutine3());
    }
    IEnumerator itemCoroutine3()
    {
        audioManagerScript.PlaySfx(7); // ת�۵�
        tankDataScript.effects[5] = true;
        yield return new WaitForSeconds(7f);
        tankDataScript.effects[5] = false;
    }

    void itemEffect4() // T-35
    {
        // nothing happens
    }

    void itemEffect5() // �г�
    {
        // nothing happens
    }

    void itemEffect6() // ��ħ
    {
        // nothing happens
    }

    void itemEffect7() // ͵Ϯ
    {
        audioManagerScript.PlaySfx(9); // ͵Ϯ
        tankDataScript.effects[6] = true;
    }

    void itemEffect8() // ��kv
    {
        // nothing happens
    }
    void presenceEffect8()
    {
        foreach (GameObject item in itemManagerScript.PlayerItems[gameObject])
        {
            if (item.GetComponent<ItemLogicScript>().MyData.Id == 8) // ��kv
            {
                audioManagerScript.PlaySfx(14);
                tankLogicScript.Enemy.GetComponent<TankLogicScript>().damage(30);
                item.GetComponent<ItemLogicScript>().MyData.data++;
                if (item.GetComponent<ItemLogicScript>().MyData.data >= 4) itemManagerScript.PlayerItems[gameObject].Remove(item);
            }
        }
    }

    void itemEffect9() // ��������
    {
        audioManagerScript.PlaySfx(13); // ��������
        enemyTankLogicScript.pin();
        tankDataScript.cHP += 30;
    }

    void itemEffect10() // �س�
    {
        // ��
    }
    void presenceEffect10_1() // �޷�ʧȥ���
    {
        if (tankDataScript.effects[1] == false) tankLogicScript.giveShock();
    }
    void presenceEffect10_2() // δ����ʱʧȥ�˵���
    {
        foreach (GameObject item in itemManagerScript.PlayerItems[gameObject])
        {
            if (item.GetComponent<ItemLogicScript>().MyData.Id == 10) // �س�
            {
                // sfx
                itemManagerScript.PlayerItems[gameObject].Remove(item);
            }
        }
    }

    void itemEffect11() // ����ս��
    {
        audioManagerScript.PlaySfx(11); // ����ս��
        itemManagerScript.DevelopItem(gameObject, "TANK");
        itemManagerScript.GiveItem(gameObject, 12);
    }

    void itemEffect12() // ����ս��
    {
        audioManagerScript.PlaySfx(12); // ����ս��
        itemManagerScript.DevelopItem(gameObject, "TANK");
        for (int i = 0; i < tankDataScript.FireConsumption.Count; i++)
        {
            if (tankDataScript.FireConsumption[i] >= 2.5f) tankDataScript.FireConsumption[i] -= 0.5f; // ������2
        }
        tankDataScript.armorThickness += 5;
        tankDataScript.hardDamage += 5;
    }

    void itemEffect13() // �ɸ�����
    {
        // ��Ч
        enemyTankLogicScript.pin();
        itemManagerScript.DevelopItem(gameObject, "ORDER");
    }

    void itemEffect14() // ɳĮ֮��
    {
        // ��Ч
        enemyTankLogicScript.pin();
        enemyTankLogicScript.damage(10);
    }

    void itemEffect15() // ����Ϯ��
    {
        // 
        tankLogicScript.giveShock();
        tankDataScript.effects[8] = true; // ����Ϯ��������������һ����Ļ
    }
}
