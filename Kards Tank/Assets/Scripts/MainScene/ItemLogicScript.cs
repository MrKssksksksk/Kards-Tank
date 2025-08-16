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
    private bool dodestory = false;

    public void getData(GameObject o, int _slot, int id)
    {
        owner = o;
        tankDataScript = owner.GetComponent<TankDataScript>();
        tankLogicScript = owner.GetComponent<TankLogicScript>();
        slot = _slot;
        data = itemDataScript.items[id];
    }


    private void Start()
    {
        ownerIndex = tankDataScript.playerIndex;
        itemAniScript.DrawCard();
    }

    public void changeSlot(int x = -1) // 使用道具时由Tank调用
    {
        slot += x;
    }

    public async Task useItem()
    {
        int id = data.Id;
        if (id == 0)
        {
             // ...
        }
        else if (id == 1)
        {
            // ...
            dodestory = true;
        }
        // ......


        await itemAniScript.UseCard();
    }

    public async Task removeItem()
    {
        await itemAniScript.UseCard(); // 暂时用同一个

        dodestory = true;
    }

    private void Update()
    {
        if (dodestory) Destroy(gameObject);
    }

}
