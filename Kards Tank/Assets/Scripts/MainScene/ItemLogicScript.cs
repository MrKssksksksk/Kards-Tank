using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Unity.VisualScripting;

public class ItemLogicScript : MonoBehaviour
{
    public GameObject owner;
    public GameObject IM;
    private TankDataScript tankDataScript;
    private TankLogicScript tankLogicScript;
    public ItemAniScript itemAni;
    public ItemDataScript itemDataScript;
    public ItemData MyData;

    public int ownerIndex, slot;
    public bool isChosen = false;
    public bool chosen = false; //辅助判断软更新变量
    public bool isSurplus;
    public bool isDestoried = false;

    void Awake()
    {
        IM = GameObject.FindGameObjectWithTag("ItemManager");
        itemAni = GetComponent<ItemAniScript>();
    }
    public void getData(GameObject Player, int _slot, int id)
    {
        //owner = Player;
        //tankDataScript = owner.GetComponent<TankDataScript>();
        //tankLogicScript = owner.GetComponent<TankLogicScript>();
        //slot = _slot;
        //itemDataScript = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemDataScript>();
        //data = itemDataScript.items[id];
    }

    public void InitData(GameObject Player, ItemData ItemData)
    {
        owner = Player;
        MyData = ItemData;
        GetComponent<SpriteRenderer>().sprite = MyData.sprite;
        ownerIndex = Player.GetComponent<TankDataScript>().playerIndex;
        GetComponent<ItemAniScript>().playerIndex = ownerIndex;
    }


    private void Start()
    {
        
    }

    public void chooseCard(bool isChosen)
    {
        // if (chosen != isChosen)
        // {
        //     chosen = isChosen;
        //     if (isChosen == true)
        //     {
        //         itemAniScript.ChooseCard();
        //     }
        //     else
        //     {
        //         itemAniScript.UnChooseCard();
        //     }
        // }
    }

    public void HandleChosen()
    {
        if (chosen != isChosen)
        {
            chosen = isChosen;
            if (isChosen) itemAni.ChooseCard();
            else itemAni.UnChooseCard();
        }
    }

    public void changeSlot(int x = -1) // 使用道具时由Tank调用
    {
        slot += x;
    }

    public void useItem()
    {

        itemAni.UseCard();
    }

    public void removeItem()
    {
        itemAni.UseCard(); // 暂时用同一个
        isDestoried = true;
    }

    private void Update()
    {
        if (!isSurplus)
        {
            HandleChosen();
            if (isDestoried) Destroy(gameObject);
        }
    }

}
