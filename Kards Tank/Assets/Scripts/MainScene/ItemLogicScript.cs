using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ItemLogicScript : MonoBehaviour
{
    private TimerScript timerScript;
    public GameObject owner;
    private TankDataScript tankDataScript;
    private TankLogicScript tankLogicScript;
    public ItemAniScript itemAniScript;
    public ItemDataScript itemDataScript;
    public ItemData data;
    public int ownerIndex, slot;
    public bool chosen = false;
    private bool doDestroy = false;
    private bool surplus;

    public void getData(GameObject Player, int _slot, int id, bool _surplus = false)
    {
        owner = Player;
        tankDataScript = owner.GetComponent<TankDataScript>();
        tankLogicScript = owner.GetComponent<TankLogicScript>();
        slot = _slot;
        itemDataScript = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemDataScript>();
        data = itemDataScript.items[id];
        surplus = _surplus;
    }


    private void Start()
    {
        timerScript = GameObject.FindGameObjectWithTag("Timer").GetComponent<TimerScript>();
        ownerIndex = tankDataScript.playerIndex;
        if (surplus)
        {
            itemAniScript.DrawSurplusCardAnime();
            timerScript.addTimer(3f, destroySelf);
        }
        else
        {
            itemAniScript.DrawCard();
        }
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

        doDestroy = true;
    }

    private void Update()
    {
        if (doDestroy) Destroy(gameObject);
    }

    private void destroySelf()
    {
        Debug.Log("Item Destroyed");
        doDestroy = true;
    }

}
