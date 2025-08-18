using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemMapScript : MonoBehaviour
{
    public AudioManagerScript AMS;
    public ItemManagerScript itemManager;
    public ItemDataScript itemData;
    public GameObject Item;
    public SpriteRenderer spriteRenderer;
    public int id;
    public ItemData MyData;
    public int existenceTime;
    private float existenceTimer = 0;

    void Update()
    {
        existenceTimer += Time.deltaTime;
        if (existenceTimer > existenceTime)
        {
            Destroy(gameObject);
        }
    }

    public void getPointId(int _id)
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        id = _id;
    }

    void Awake()
    {
        AMS = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
        itemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManagerScript>();
        itemData = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemDataScript>();
    }

    void Start() // actually run after getPointId()
    {
        itemData.items.ForEach((itemData) =>
        {
            if (id == itemData.Id) MyData = itemData;
        });
        randomizePosition();
        spriteRenderer.sprite = MyData.sprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (existenceTimer < 0.1f)
        {
            existenceTimer = 0;
            randomizePosition();
            return;
        }
        if (collision.gameObject.layer == 6) // Player
        {
            // TankDataScript tankDataScript = collision.gameObject.GetComponent<TankDataScript>();
            //TankLogicScript tankLogicScript = collision.gameObject.GetComponent<TankLogicScript>();
            HandlerPickedItem(collision.gameObject);
            Debug.Log(collision.gameObject);
            // Item.GetComponent<ItemLogicScript>().owner = player;
            //tankLogicScript.giveItem(id);
            // 后续音效可以添加
            Destroy(gameObject);
        }
    }

    private void randomizePosition()
    {
        transform.position = new Vector3(-7.5f + Random.Range(0, 16), -3.5f + Random.Range(0, 8), 0);
    }

    private void HandlerPickedItem(GameObject Player)
    {
        AMS.PlaySfx(15); //抽牌
        GameObject newItem = Instantiate(Item);
        ItemLogicScript itemLogic = newItem.GetComponent<ItemLogicScript>();
        ItemAniScript itemAni = newItem.GetComponent<ItemAniScript>();
        itemLogic.InitData(Player, MyData);

        //空检查
        if (!itemManager.PlayerItems.ContainsKey(Player))
        {
            itemManager.PlayerItems[Player] = new List<GameObject>();
        }


        if (itemManager.PlayerItems[Player].Count < 3)
        {
            itemManager.PlayerItems[Player].Add(newItem);
            itemAni.slot = itemManager.PlayerItems[Player].Count - 1;
            newItem.GetComponent<ItemAniScript>().DrawCardAmine();
        }
        else
        {
            itemLogic.isSurplus = true;
            itemAni.DrawSurplusCardAnime();
        }
    } 
}
