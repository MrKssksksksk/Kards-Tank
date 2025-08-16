using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ItemLogicScript : MonoBehaviour
{
    public GameObject owner;
    private TankDataScript tankDataScript;
    private TankLogicScript tankLogicScript;
    public ItemAniScript itemAniScript;
    public ItemDataScript itemDataScript;
    public ItemData data;
    public int ownerIndex, slot;
    public bool chosen = false;
    private bool dodestory = false;

    public void getData(GameObject o, int _slot, int id)
    {
        owner = o;
        tankDataScript = owner.GetComponent<TankDataScript>();
        tankLogicScript = owner.GetComponent<TankLogicScript>();
        slot = _slot;
        itemDataScript = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemDataScript>();
        data = itemDataScript.items[id];
    }


    private void Start()
    {
        ownerIndex = tankDataScript.playerIndex;
        itemAniScript.DrawCard();
    }

    public void chooseCard(bool c)
    {
        if (chosen != c)
        {
            chosen = c;
            if (c == true)
            {
                itemAniScript.ChooseCard();
            }
            else
            {
                itemAniScript.UnChooseCard();
            }
        }
    }

    public void changeSlot(int x = -1) // ʹ�õ���ʱ��Tank����
    {
        slot += x;
    }

    public async Task useItem()
    {

        await itemAniScript.UseCard();
    }

    public async Task removeItem()
    {
        await itemAniScript.UseCard(); // ��ʱ��ͬһ��

        dodestory = true;
    }

    private void Update()
    {
        if (dodestory) Destroy(gameObject);

        
    }

}
