using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ItemLogicScript : MonoBehaviour
{
    public GameObject owner;
    public GameObject IM;
    private TankDataScript tankDataScript;
    private TankLogicScript tankLogicScript;
    public ItemAniScript itemAniScript;
    public ItemDataScript itemDataScript;
    public ItemData MyData;

    public int ownerIndex, slot;
    public bool chosen = false;
    private bool isDestoried = false;

    public void getData(GameObject Player, int _slot, int id)
    {
        owner = Player;
        tankDataScript = owner.GetComponent<TankDataScript>();
        tankLogicScript = owner.GetComponent<TankLogicScript>();
        slot = _slot;
        itemDataScript = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemDataScript>();
        //data = itemDataScript.items[id];
    }

    public void InitData(GameObject Player, ItemData ItemData, int _slot)
    {
        owner = Player;
        MyData = ItemData;
        slot = _slot;
        ownerIndex = Player.GetComponent<TankDataScript>().playerIndex;
    }

    void Awake()
    {
        IM = GameObject.FindGameObjectWithTag("ItemManager");
    }

    private void Start()
    {
        // ownerIndex = tankDataScript.playerIndex;
        // itemAniScript.DrawCard();
    }

    public void chooseCard(bool isChosen)
    {
        if (chosen != isChosen)
        {
            chosen = isChosen;
            if (isChosen == true)
            {
                itemAniScript.ChooseCard();
            }
            else
            {
                itemAniScript.UnChooseCard();
            }
        }

        
    }

    public void changeSlot(int x = -1) // 使用道具时由Tank调用
    {
        slot += x;
    }

    public void useItem()
    {

        itemAniScript.UseCard();
    }

    public void removeItem()
    {
        itemAniScript.UseCard(); // 暂时用同一个

        isDestoried = true;
    }

    private void Update()
    {
        if (isDestoried) Destroy(gameObject);
    }

}
