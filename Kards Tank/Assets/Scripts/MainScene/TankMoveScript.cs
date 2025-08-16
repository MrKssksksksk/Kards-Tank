using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Tank1MoveScript : MonoBehaviour
{
    public TankDataScript tankDataScript;
    public TankLogicScript tankLogicScript;
    public Rigidbody2D rb;
    public AudioManagerScript audioManagerScript;
    public GameObject bullet;
    public float degree;
    public KeyCode UpKey, DownKey, LeftKey, RightKey, SpeedUpKey, FireKey, Item1Key, Item2Key, Item3Key;
    private int horizontal, vertical;
    private Vector2 direction;
    private Vector2 previousPosition;
    private float fireCoolDownTimer;
    private UnityEvent useItem = new UnityEvent();

    private int chosenItem;
    public float chooseLastingTime;
    private float chooseLastingTimer;
    private float item0Timer, item3Timer;
    private bool item0TimerEnable = false, item3TimerEnable = false;


    // Start is called before the first frame update
    void Start()
    {
        horizontal = 0;
        direction = new Vector2(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad));
        // right:0 up:90 left:180 down:270
        fireCoolDownTimer = 0;
        useItem.AddListener(tankLogicScript.Enemy.GetComponent<TankLogicScript>().OnEnemyUseItem);
        chosenItem = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Move & Turn
        if (tankLogicScript.canMove())
        {
            horizontal = (Input.GetKey(LeftKey) ? 1 : 0) - (Input.GetKey(RightKey) ? 1 : 0);
            if (tankDataScript.cSupply > 0 || !tankLogicScript.doConsumeSupply())
            {
                degree += horizontal * tankDataScript.turnSpeed * (Input.GetKey(SpeedUpKey) ? 1.5f : 1f) * Time.deltaTime;
                if (tankLogicScript.doConsumeSupply()) tankDataScript.cSupply -= Mathf.Abs(horizontal) * (1 + tankDataScript.supplyComsumptionBonus) 
                        * tankDataScript.supplyConsumptionPerTurn * (Input.GetKey(SpeedUpKey) ? 2f : 1f) * Time.deltaTime;
            }
            direction = new Vector2(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad));
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, degree));

            vertical = (Input.GetKey(UpKey) ? 1 : 0) - (Input.GetKey(DownKey) ? 1 : 0);
            if (vertical != 0 && (tankDataScript.cSupply > 0 || !tankLogicScript.doConsumeSupply()))
            {
                if (Input.GetKey(SpeedUpKey))
                {
                    rb.velocity = direction * tankDataScript.moveSpeed * vertical * 1.5f;
                    if (tankLogicScript.doConsumeSupply()) tankDataScript.cSupply -= 2 * (1 + tankDataScript.supplyComsumptionBonus) * tankDataScript.supplyConsumptionPerMove * Time.deltaTime;
                }
                else
                {
                    rb.velocity = direction * tankDataScript.moveSpeed * vertical;
                    if (tankLogicScript.doConsumeSupply()) tankDataScript.cSupply -= (1 + tankDataScript.supplyComsumptionBonus) * tankDataScript.supplyConsumptionPerMove * Time.deltaTime;
                }
            }
            else
            {
                rb.velocity *= 0.8f;
                if (rb.velocity.sqrMagnitude < 1.5f) rb.velocity = Vector2.zero;
            }
        }

        if (fireCoolDownTimer > 0) fireCoolDownTimer -= Time.deltaTime;
        if (tankLogicScript.canFire())
        {
            if (fireCoolDownTimer <= 0 && Input.GetKeyUp(FireKey) && 
                (tankDataScript.cSupply > tankDataScript.FireConsumption[tankDataScript.specialBullets.Count != 0 ? tankDataScript.specialBullets.Peek() : 0] || 
                !tankLogicScript.doConsumeSupply()))
            {
                fireCoolDownTimer = tankDataScript.reloadTime;
                if (tankLogicScript.doConsumeSupply()) tankDataScript.cSupply -= tankDataScript.FireConsumption[tankDataScript.specialBullets.Count != 0 ? tankDataScript.specialBullets.Peek() : 0];
                rb.velocity -= direction * 5;
                GameObject b = Instantiate(bullet, transform.position, transform.rotation);
                b.GetComponent<BulletScript>().getBulletData(gameObject, direction);
            }
        }

        chooseLastingTimer += Time.deltaTime;
        if (chosenItem != 0 && chooseLastingTimer > chooseLastingTime)
        {
            chosenItem = 0;
            foreach (GameObject item in tankDataScript.items)
            {
                item.GetComponent<ItemLogicScript>().chooseCard(false);
            }
        }
        if (tankLogicScript.canUseItem())
        {
            bool choose = false;
            if (Input.GetKeyUp(Item1Key) && tankDataScript.items.Count >= 1 && tankDataScript.cSupply >= tankDataScript.getData(0).Cost)
            {
                if (chosenItem == 1) choose = true;
                else
                {
                    if (chosenItem != 0) tankDataScript.items[chosenItem - 1].GetComponent<ItemLogicScript>().chooseCard(false);
                    chosenItem = 1;
                    tankDataScript.items[0].GetComponent<ItemLogicScript>().chooseCard(true);
                    chooseLastingTimer = 0;
                }
            }
            else if (Input.GetKeyUp(Item2Key) && tankDataScript.items.Count >= 2 && tankDataScript.cSupply >= tankDataScript.getData(1).Cost)
            {
                if (chosenItem == 2) choose = true;
                else
                {
                    if (chosenItem != 0) tankDataScript.items[chosenItem - 1].GetComponent<ItemLogicScript>().chooseCard(false);
                    chosenItem = 2;
                    tankDataScript.items[1].GetComponent<ItemLogicScript>().chooseCard(true);
                    chooseLastingTimer = 0;
                }
            }
            else if (Input.GetKeyUp(Item3Key) && tankDataScript.items.Count >= 3 && tankDataScript.cSupply >= tankDataScript.getData(2).Cost)
            {
                if (chosenItem == 3) choose = true;
                else
                {
                    if (chosenItem != 0) tankDataScript.items[chosenItem - 1].GetComponent<ItemLogicScript>().chooseCard(false);
                    chosenItem = 3;
                    tankDataScript.items[2].GetComponent<ItemLogicScript>().chooseCard(true);
                    chooseLastingTimer = 0;
                }
            }
            if (choose == true)
            {
                useItem.Invoke();

                
                for (int i = chosenItem - 1; i < tankDataScript.items.Count; i++)
                {
                    tankDataScript.items[i].GetComponent<ItemLogicScript>().changeSlot(-1);
                }

                int itemId = tankDataScript.getId(chosenItem - 1);
                tankDataScript.cSupply -= tankDataScript.getData(chosenItem - 1).Cost;
                tankLogicScript.useItem(chosenItem - 1);
                chosenItem = 0;

                if (itemId == 0) // 银行
                {
                    // 音效
                    item0Timer = 3.0f;
                    item0TimerEnable = true;

                }
                else if (itemId == 1) // 压制炮
                {
                    audioManagerScript.PlaySfx(18); // 苏联炮部署
                    tankLogicScript.pushBullet(2); // 85mm
                }
                else if (itemId == 2) // 蟑螂车
                {
                    audioManagerScript.PlaySfx(0); // 小坦克移动
                    tankLogicScript.pushBullet(3); // ib
                }
                else if (itemId == 3) // 转折点
                {
                    audioManagerScript.PlaySfx(7); // 转折点
                    tankDataScript.effects[5] = true;
                    item3TimerEnable = true;
                    item3Timer = 7f;
                }
                else if (itemId == 4) // T-35
                {
                    // nothing happens
                }
                else if (itemId == 5) // 列车
                {
                    // nothing happens
                }
                else if (itemId == 6) // 红魔
                {
                    // nothing happens
                }
                else if (itemId == 7) // 偷袭
                {
                    audioManagerScript.PlaySfx(9); // 偷袭
                    tankDataScript.effects[6] = true;
                }
                else if (itemId == 8) // 金kv
                {
                    // nothing happens
                }
                else if (itemId == 9) // 海军力量
                {
                    audioManagerScript.PlaySfx(13); // 海军力量
                    tankLogicScript.Enemy.GetComponent<TankLogicScript>().pin();
                    tankDataScript.cHP += 30;
                }
                else if (itemId == 10) // 守冲
                {
                    // nothing
                }
                else if (itemId == 11) // 闪击战法
                {
                    audioManagerScript.PlaySfx(11); // 闪击战法
                    int x = tankLogicScript.developItem("TANK");
                    if (x != -1)
                    {
                        tankLogicScript.giveItem(x);
                    }
                    tankLogicScript.giveItem(12);
                }
                else if (itemId == 12) // 竞争战法
                {
                    audioManagerScript.PlaySfx(12); // 竞争战法
                    int x = tankLogicScript.developItem("TANK");
                    if (x != -1)
                    {
                        tankLogicScript.giveItem(x);
                    }
                    for (int i = 0; i < tankDataScript.FireConsumption.Count; i++)
                    {
                        if (tankDataScript.FireConsumption[i] >= 2.5f) tankDataScript.FireConsumption[i] -= 0.5f; // 最多减到2
                    }
                    tankDataScript.armorThickness += 5;
                    tankDataScript.hardDamage += 5;
                }

            }
        }

        if (item0TimerEnable) // 银行
        {
            item0Timer -= Time.deltaTime;
            if (item0Timer <= 0)
            {
                item0TimerEnable = false;

                tankDataScript.cSupply += 12;
                    tankDataScript.effects[3] = true; // ban道具
            }
        }
        if (item3TimerEnable)
        {
            item3Timer -= Time.deltaTime;
            if (item3Timer <= 0)
            {
                item3TimerEnable = false;

                tankDataScript.effects[5] = false;
            }
        }



        
    }
}
