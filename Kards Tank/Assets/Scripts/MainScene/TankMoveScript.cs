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
                if (tankDataScript.effects[7] == true) tankLogicScript.removeSmokescreen(); // ÑÌÄ»

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
                if (tankDataScript.effects[7] == true) tankLogicScript.removeSmokescreen(); // ÑÌÄ»

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
            bool use = false;
            if (Input.GetKeyUp(Item1Key) && tankDataScript.items.Count >= 1 && tankDataScript.cSupply >= tankDataScript.getData(0).Cost)
            {
                if (chosenItem == 1) use = true;
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
                if (chosenItem == 2) use = true;
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
                if (chosenItem == 3) use = true;
                else
                {
                    if (chosenItem != 0) tankDataScript.items[chosenItem - 1].GetComponent<ItemLogicScript>().chooseCard(false);
                    chosenItem = 3;
                    tankDataScript.items[2].GetComponent<ItemLogicScript>().chooseCard(true);
                    chooseLastingTimer = 0;
                }
            }
            if (use == true)
            {
                useItem.Invoke();

            }
        }

        



        
    }
}
