using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Tank1MoveScript : MonoBehaviour
{
    public TankDataScript tankDataScript;
    public TankLogicScript tankLogicScript;
    public Rigidbody2D rb;
    public GameObject bullet;
    public bool debug = false;
    public float degree;
    public KeyCode UpKey, DownKey, LeftKey, RightKey, SpeedUpKey, FireKey, Item1Key, Item2Key, Item3Key;
    private int horizontal, vertical;
    private Vector2 direction;
    private Vector2 previousPosition;
    private float fireCoolDownTimer;
    private UnityEvent useItem = new UnityEvent();

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
            if (fireCoolDownTimer <= 0 && Input.GetKeyUp(FireKey) && (tankDataScript.cSupply > tankDataScript.FireConsumption[tankDataScript.specialBullets.Count != 0 ? tankDataScript.specialBullets.Peek() : 0] || !tankLogicScript.doConsumeSupply()))
            {
                fireCoolDownTimer = tankDataScript.reloadTime;
                if (tankLogicScript.doConsumeSupply()) tankDataScript.cSupply -= tankDataScript.FireConsumption[tankDataScript.specialBullets.Count != 0 ? tankDataScript.specialBullets.Peek() : 0];
                rb.velocity -= direction * 5;
                GameObject b = Instantiate(bullet, transform.position, transform.rotation);
                b.GetComponent<BulletScript>().getBulletData(gameObject, direction);
            }
        }

        if (tankLogicScript.canUseItem())
        {
            int cItem = 0;
            if (Input.GetKeyUp(Item1Key) && tankDataScript.items.Count >= 1 && tankDataScript.cSupply >= tankDataScript.itemCosts[tankDataScript.items[0]])
            {
                cItem = 1;
            }
            else if (Input.GetKeyUp(Item2Key) && tankDataScript.items.Count >= 2 && tankDataScript.cSupply >= tankDataScript.itemCosts[tankDataScript.items[1]])
            {
                cItem = 2;
            }
            else if (Input.GetKeyUp(Item3Key) && tankDataScript.items.Count >= 3 && tankDataScript.cSupply >= tankDataScript.itemCosts[tankDataScript.items[2]])
            {
                cItem = 3;
            }
            if (cItem != 0)
            {
                useItem.Invoke();

                int itemId = tankDataScript.items[cItem - 1];
                tankLogicScript.removeItem(cItem - 1);
                tankDataScript.cSupply -= tankDataScript.itemCosts[itemId];
                if (itemId == 0) // 银行
                {
                    // 音效
                    item0Timer = 3.0f;
                    item0TimerEnable = true;

                }
                if (itemId == 1) // 压制炮
                {
                    // 
                    tankLogicScript.pushBullet(2); // 85mm
                }
                if (itemId == 2) // 蟑螂车
                {
                    //
                    tankLogicScript.pushBullet(3); // ib
                }
                if (itemId == 3) // 转折点
                {
                    //
                    tankDataScript.effects[5] = true;
                    item3TimerEnable = true;
                    item3Timer = 7f;
                }
                if (itemId == 4) // T-35
                {
                    // nothing happens
                }
                if (itemId == 5) // 列车
                {
                    // nothing happens
                }
                if (itemId == 6) // 红魔
                {
                    // nothing happens
                }
                if (itemId == 7) // 偷袭
                {
                    // 音效
                    tankDataScript.effects[6] = true;
                }
                if (itemId == 8) // 金kv
                {
                    // nothing happens
                }
                if (itemId == 9) // 海军力量
                {
                    tankLogicScript.Enemy.GetComponent<TankLogicScript>().pin();
                    tankDataScript.cHP += 30;
                }
                if (itemId == 10) // 守冲
                {
                    // nothing
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



        if (debug) // scripts only for debug
        {
            if (Input.GetKeyUp(KeyCode.O) && Input.GetKey(KeyCode.Keypad1)) tankLogicScript.pushBullet(1); // snow
            if (Input.GetKeyUp(KeyCode.O) && Input.GetKey(KeyCode.Keypad2)) tankLogicScript.pushBullet(2); // 85mm
            if (Input.GetKeyUp(KeyCode.O) && Input.GetKey(KeyCode.Keypad3)) tankLogicScript.pushBullet(3); // ib
            if (Input.GetKeyUp(KeyCode.O) && Input.GetKey(KeyCode.Keypad4)) tankLogicScript.pushBullet(4); // li
        }
    }
}
