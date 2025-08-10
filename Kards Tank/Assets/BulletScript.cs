using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
//using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.Events;

public class BulletScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject hitEffect;
    public SpriteRenderer spriteRenderer;
    public List<Sprite> sprites = new List<Sprite>();
    public  List<int> bulletRunningTime = new List<int>();
    public GameObject owner;
    public UnityEvent bulletHitEnemy = new UnityEvent(), bulletHitNothing = new UnityEvent(), bulletHitSelf = new UnityEvent();
    public UnityEvent effect1end = new UnityEvent();
    private int bulletType;
    private int runningTime = 9999, runningSpeed;
    private Vector2 direction;
    private int hardDamage, softDamage;
    private float timer;
    private bool enable;
    private bool item7enable;
    private bool effect1;

    void Start()
    {
        timer = 0;
        rb.velocity = runningSpeed * direction;
        enable = false;
    }


    public void getBulletData(GameObject o, Vector2 d)
    {
        bulletHitEnemy.AddListener(o.GetComponent<TankLogicScript>().OnBulletHitEnemy);
        bulletHitNothing.AddListener(o.gameObject.GetComponent<TankLogicScript>().OnBulletNotHitEnemy);
        bulletHitSelf.AddListener(o.gameObject.GetComponent<TankLogicScript>().OnBulletNotHitEnemy);

        direction = d;
        owner = o;
        if (o.GetComponent<TankDataScript>().effects[6] == true)
        {
            item7enable = true;
            o.GetComponent<TankDataScript>().effects[6] = false;
        }
        else item7enable = false;

        if (o.GetComponent<TankDataScript>().effects[1] == true) // ³å»÷
        {
            effect1 = true;
            o.GetComponent<TankDataScript>().effects[1] = false;
            o.GetComponent<TankDataScript>().effect1BulletNum++;
        }

        if (o.GetComponent<TankDataScript>().specialBullets.Count != 0)
        {
            bulletType = o.GetComponent<TankDataScript>().specialBullets.Pop();
            if (bulletType == 1) // snow
            {
                spriteRenderer.sprite = sprites[1];
                runningTime = bulletRunningTime[1];
                runningSpeed = (int)(o.GetComponent<TankDataScript>().bulletSpeed * 0.8f);
                hardDamage = o.GetComponent<TankDataScript>().hardDamage;
                softDamage = o.GetComponent<TankDataScript>().softDamage;
            }
            if (bulletType == 2) // 85mm
            {
                spriteRenderer.sprite = sprites[2];
                runningTime = bulletRunningTime[2];
                runningSpeed = o.GetComponent<TankDataScript>().bulletSpeed;
                hardDamage = (int)(o.GetComponent<TankDataScript>().hardDamage * 1.2f);
                softDamage = o.GetComponent<TankDataScript>().softDamage;
            }
            if (bulletType == 3) // ib
            {
                spriteRenderer.sprite = sprites[3];
                runningTime = bulletRunningTime[3];
                runningSpeed = o.GetComponent<TankDataScript>().bulletSpeed;
                hardDamage = (int)(o.GetComponent<TankDataScript>().hardDamage * 1.1f);
                softDamage = o.GetComponent<TankDataScript>().softDamage;
            }
            if (bulletType == 4) // li
            {
                spriteRenderer.sprite = sprites[4];
                runningTime = bulletRunningTime[4];
                runningSpeed = o.GetComponent<TankDataScript>().bulletSpeed;
                hardDamage = (int)(o.GetComponent<TankDataScript>().hardDamage * 0.9f);
                softDamage = (int)(o.GetComponent<TankDataScript>().softDamage * 0.8f);
            }
        }
        else
        {
            bulletType = 0; // basic
            spriteRenderer.sprite = sprites[0];
            runningTime = bulletRunningTime[0];
            runningSpeed = o.GetComponent<TankDataScript>().bulletSpeed;
            hardDamage = o.GetComponent<TankDataScript>().hardDamage;
            softDamage = o.GetComponent<TankDataScript>().softDamage;
        }

    }

    void Update()
    {
        if (enable) timer += Time.deltaTime;
        if (timer > runningTime)
        {
            bulletHitSelf.Invoke();
            if (effect1) owner.GetComponent<TankDataScript>().effect1BulletNum--;
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!enable && collision.gameObject.tag == "Player")
        {
            enable = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enable && collision.gameObject.tag == "Player")
        {
            if (collision.gameObject != owner)
            {
                bulletHitEnemy.Invoke();
            }
            else
            {
                bulletHitSelf.Invoke();
            }

            if (bulletType == 2) collision.gameObject.GetComponent<TankLogicScript>().pin(); // 85mm Ñ¹ÖÆ
            if (bulletType == 3) // ib
            {
                collision.gameObject.GetComponent<TankLogicScript>().giveItem(2);
                collision.gameObject.GetComponent<TankLogicScript>().giveItem(2);
            }
            if (collision.gameObject.GetComponent<TankDataScript>().items.Contains(6))
            {
                int c = collision.gameObject.GetComponent<TankDataScript>().items.FindAll(t => t == 6).Count;
                Debug.Log(c);
                owner.GetComponent<TankDataScript>().cSupply -= c;
            }
            if (item7enable)
            {
                if (collision.gameObject.GetComponent<TankDataScript>().effects[0] == true)
                {
                    softDamage *= 2;
                }
                collision.gameObject.GetComponent<TankLogicScript>().pin();
                
            }


            int armorThickness;
            TankDataScript tankDataScript = collision.gameObject.GetComponent<TankDataScript>();
            float actualHardDamage = hardDamage * Mathf.Cos((timer / runningTime) * Mathf.PI / 2);
            if (UnityEngine.Random.Range(1, 101) > tankDataScript.armorIntegrity * 100)
            {
                armorThickness = 0;
            }
            else armorThickness = tankDataScript.armorThickness;
            if (actualHardDamage > armorThickness)
            {
                collision.gameObject.GetComponent<TankLogicScript>().damage((int)(actualHardDamage - armorThickness + softDamage));
                if (armorThickness != 0) tankDataScript.armorIntegrity -= 0.5f * (actualHardDamage / armorThickness) - 0.4f;
            }
            else
            {
                collision.gameObject.GetComponent<TankLogicScript>().damage((int)(actualHardDamage * 0.05f));
                if (armorThickness != 0) tankDataScript.armorIntegrity -= 0.05f * actualHardDamage / armorThickness;
            }

            Instantiate(hitEffect, transform.position, Quaternion.Euler(0, 0, 0));

            if (effect1) owner.GetComponent<TankDataScript>().effect1BulletNum--;
            Destroy(gameObject);
        }
    }
}
