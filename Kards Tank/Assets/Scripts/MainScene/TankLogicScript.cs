using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// using static UnityEditor.Progress;
// using static UnityEditor.Progress;

public class TankLogicScript : MonoBehaviour
{
    private AudioManagerScript audioManagerScript;
    public GameObject Enemy;
    private TankDataScript tankDataScript;
    private TankItemEffectScript tankItemEffectScript;
    public GameObject tankEffect;
    public GameObject Item;
    private ItemDataScript itemDataScript;
    private ItemManagerScript itemManagerScript;
    private SpriteRenderer spriteRenderer;
    private float supplyIncreaseTimer, pinTimer;
    private float gameTimer;

    private void Start()
    {
        audioManagerScript = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        tankDataScript = GetComponent<TankDataScript>();
        tankItemEffectScript = GetComponent<TankItemEffectScript>();
        itemDataScript = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemDataScript>();
        itemManagerScript = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManagerScript>();
        if (StaticData.Instance.turn > 1) // 多回合初始化
        {
            StaticData.Instance.playerDatas[tankDataScript.playerIndex].output(tankDataScript);
            tankDataScript.supplyCapacity += 1;
            for (int i = 0; i < tankDataScript.items.Count; i++)
            {
                //GameObject e = Instantiate(Item);
                //e.GetComponent<ItemLogicScript>().getData(gameObject, i, StaticData.Instance.playerDatas[tankDataScript.playerIndex].items[i]);
                //tankDataScript.items.Add(e);
            }
            Enemy = GameObject.FindGameObjectWithTag(tankDataScript.playerIndex == 0 ? "Player2" : "Player1");
        }
        supplyIncreaseTimer = 0;
        gameTimer = 0;
    }

    private void Update()
    {
        gameTimer += Time.deltaTime;
        // 补给
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
            if (tankDataScript.effects[3] == true) tankDataScript.effects[3] = false; // 银行
        }

        // 压制
        if (tankDataScript.effects[0] == true) 
        {
            pinTimer += Time.deltaTime;
            if (pinTimer > tankDataScript.pinTime)
            {
                tankDataScript.effects[0] = false;
            }
        }

        // 子弹冲击免疫
        if (tankDataScript.effect1BulletNum > 0)
        {
            tankDataScript.effects[4] = true;
        }
        else
        {
            tankDataScript.effects[4] = false;
        }

        foreach (UnityAction action in tankItemEffectScript.always) action();

        // T-35
        if (gameTimer % 5 < Time.deltaTime) // 每5s一次 共8次
        {
            foreach (GameObject item in itemManagerScript.PlayerItems[gameObject])
            {
                if (item.GetComponent<ItemLogicScript>().MyData.Id == 4) // T-35
                {
                    audioManagerScript.PlaySfx(1); // 小坦克射击
                    Enemy.GetComponent<TankLogicScript>().damage(5);
                    damage(5);
                    item.GetComponent<ItemLogicScript>().MyData.data++;
                    if (item.GetComponent<ItemLogicScript>().MyData.data >= 8) itemManagerScript.PlayerItems[gameObject].Remove(item);
                }
            }
        }

        // 装甲列车
        if (gameTimer % 10 < Time.deltaTime) // 每10s一次
        {
            int trainNum = countItem(5);
            for (int i = 0; i < trainNum; i++)
            {
                audioManagerScript.PlaySfx(5);
                pushBullet(4);
            }
        }

        //// 守冲
        //if (countItem(10) > 0)
        //{
        //    tankDataScript.effects[1] = true;
        //}

        

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

    [ContextMenu("give Smokescreen")]
    public void giveSmokescreen() // 烟幕
    {
        tankDataScript.effects[7] = true;
        gameObject.layer = LayerMask.NameToLayer("Smokescreen");
        setTransparent();
    }

    public void removeSmokescreen()
    {
        tankDataScript.effects[7] = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
        setTransparent();
    }

    public void giveShock()
    {
        tankDataScript.effects[1] = true;
    }

    public void removeShock()
    {
        tankDataScript.effects[1] = false;

        if (tankDataScript.effects[8] == true) // 暗隐袭击
        {
            tankDataScript.effects[8] = false;
            giveSmokescreen();
        }

        foreach (UnityAction action in tankItemEffectScript.removeShock) action();

    }

    public void giveItem(int id)
    {
        itemManagerScript.GiveItem(gameObject, id);
    }

    public void pushBullet(int id)
    {
        // 动画
        tankDataScript.specialBullets.Push(id);
    }

    public void setTransparent()
    {
        Color color = spriteRenderer.color;
        if (tankDataScript.effects[7] == false)
        {
            color.a = 1f; // 透明度 0透明 1实心
        } 
        else
        {
            color.a = 0.5f;
        }
        
        spriteRenderer.color = color;
    }

    public void OnBulletHitEnemy()
    {
        Debug.Log("bullet hit enemy");

        foreach (UnityAction action in tankItemEffectScript.bulletHitEnemy) action();
    }

    public void OnBulletSelf()
    {
        Debug.Log("bullet hit self");

        foreach (UnityAction action in tankItemEffectScript.bulletHitSelf) action();
    }

    public void OnBulletHitNothing()
    {
        Debug.Log("bullet hit nothing");

        foreach (UnityAction action in tankItemEffectScript.bulletHitNothing) action();
    }

    public void OnEnemyUseItem()
    {
        Debug.Log("enemy use item"); 

        foreach (UnityAction action in tankItemEffectScript.enemyUseItem) action();
    }

    public void OnEnemyDrawItem()
    {
        Debug.Log("enemy use item");

        foreach (UnityAction action in tankItemEffectScript.enemyDrawItem) action();
    }

    public int countItem(int id)
    {
        int count = 0;
        itemManagerScript.PlayerItems[gameObject].ForEach(item =>
        {
            if (item.GetComponent<ItemLogicScript>().MyData.Id == id) count++;
        });
        return count;
    }

    public bool canUseItem()
    {
        if (tankDataScript.effects[3] == false) return true;
        else return false;
    }

    public bool canMove()
    {
        if (tankDataScript.effects[0] == false) return true; // 压制
        return false;
    }

    public bool canFire()
    {
        if (tankDataScript.effects[0] == false) return true; // 压制
        return false;
    }

    public bool doConsumeSupply() // do not work when use items
    {
        if (tankDataScript.effects[5] == false) return true; // 转折点
        return false;
    }

    public bool isImmune()
    {
        if (tankDataScript.effects[4] == false) return false;
        else return true;
    }
}
