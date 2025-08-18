using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemMapScript : MonoBehaviour
{
    public GameObject IM; // ItemManager
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
        IM = GameObject.FindGameObjectWithTag("ItemManager");
    }

    void Start() // actually run after getPointId()
    {
        Debug.Log(IM);
        IM.GetComponent<ItemDataScript>().items.ForEach((itemData) =>
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
            GameObject player = collision.gameObject;
            HandlerPickedItem(player);
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
        Instantiate(Item);

        //空检查
        if (!IM.GetComponent<ItemManagerScript>().PlayerItems.ContainsKey(Player))
        {
            IM.GetComponent<ItemManagerScript>().PlayerItems[Player] = new List<GameObject>();
            Debug.Log($"Created new category: {Player}");
        }
        

        if (IM.GetComponent<ItemManagerScript>().PlayerItems[Player].Count <= 3)
        {
            Debug.Log(Player);

            int slot = IM.GetComponent<ItemManagerScript>().PlayerItems[Player].Count;
            IM.GetComponent<ItemManagerScript>().PlayerItems[Player].Add(Item);
            Item.GetComponent<ItemLogicScript>().InitData(Player, MyData, slot);
        }
        else
        {

        }
    } 
}
