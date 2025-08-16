using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLogicScript : MonoBehaviour
{
    public GameObject owner;
    private TankDataScript tankDataScript;
    private TankLogicScript tankLogicScript;
    public ItemAniScript itemAniScript;
    public int ownerIndex, id, slot;

    public void getData(GameObject o, int _slot)
    {
        owner = o;
        tankDataScript = owner.GetComponent<TankDataScript>();
        tankLogicScript = owner.GetComponent<TankLogicScript>();
        slot = _slot;
        id = tankDataScript.items[slot];
    }


    private void Start()
    {
        ownerIndex = tankDataScript.playerIndex;
    }

    public void changeSlot(int x = -1) // 使用道具时由Tank调用
    {
        slot += x;
    }

    public void useItem()
    {
        if (id == 0)
        {

        }
        else if (id == 1)
        {

        }
        // ......
    }

}
