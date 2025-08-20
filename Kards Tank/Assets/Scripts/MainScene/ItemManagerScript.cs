using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class ItemManagerScript : MonoBehaviour
{
    private ItemDataScript itemDataScript;

    //侧边栏道具管理
    public int ItemUpperLimit; //道具上限
    public GameObject Item;
    public GameObject Player1;
    public GameObject Player2;
    public Dictionary<GameObject, List<GameObject>> PlayerItems = new();
    public KeyCode P1First, P1Second, P1Third, P2First, P2Second, P2Third;
    //道具生成
    public GameObject ItemOnMap;
    public int spawnTime;
    private float spawnTimer;
    private List<int> spawnWeight;
    private int weightSum;

    //跟踪状态时使用
    private Dictionary<GameObject, Coroutine> SelectionCoroutines = new(); //正在进行协程的字典
    private Dictionary<GameObject, int> SelectedSlots = new(); //每个玩家选择的插槽 -1为未选择


    void Start()
    {
        itemDataScript = GetComponent<ItemDataScript>();
        PlayerItems.Add(Player1, new List<GameObject>());
        PlayerItems.Add(Player2, new List<GameObject>());
        SelectedSlots.Add(Player1, -1);
        SelectedSlots.Add(Player2, -1);
        //权重
        spawnWeight = new List<int>();
        GetComponent<ItemDataScript>().items.ForEach((item) =>
        {
            spawnWeight.Add(item.Weight);
        });
        spawnTimer = 0;
    }

    void Update()
    {
        HandleGenerateItem();
        UpdateItemStatus();
        HandleInput();
    }

    private void UpdateItemStatus() //更新道具状态
    {
        foreach (KeyValuePair<GameObject, List<GameObject>> pair in PlayerItems)
        {
            CleanupItemLists(pair.Value);
            int index = 0;
            foreach (GameObject itm in pair.Value)
            {
                GameObject player = pair.Key;
                ItemLogicScript itemLogic = itm.GetComponent<ItemLogicScript>();
                itemLogic.slot = index;
                itemLogic.isChosen = (SelectedSlots[player] == index);
                //Debug.Log($"{player}的道具槽{itemLogic.slot}中的{itemLogic.MyData.Name}是否被选中 {itemLogic.isChosen}");
                index++;
            }
        }
    }

    public void HandleInput()
    {
        // Player1 按键检测
        if (Input.GetKeyDown(P1First)) HandleItemInput(Player1, 0);
        else if (Input.GetKeyDown(P1Second)) HandleItemInput(Player1, 1);
        else if (Input.GetKeyDown(P1Third)) HandleItemInput(Player1, 2);
        
        // Player2 按键检测
        if (Input.GetKeyDown(P2First)) HandleItemInput(Player2, 0);
        else if (Input.GetKeyDown(P2Second)) HandleItemInput(Player2, 1);
        else if (Input.GetKeyDown(P2Third)) HandleItemInput(Player2, 2);
    }

    private void HandleItemInput(GameObject Player, int slot)
    {
        List<GameObject> items = PlayerItems[Player];
        if (slot >= 0 && slot < items.Count && Player.GetComponent<TankDataScript>().cSupply >= PlayerItems[Player][slot].GetComponent<ItemLogicScript>().MyData.Cost)
        {
            if (SelectedSlots[Player] != slot) //选中了新的道具！
            {
                //如果有正在执行的协程，终止它！
                if (SelectionCoroutines.ContainsKey(Player))
                    StopCoroutine(SelectionCoroutines[Player]);
                SelectedSlots[Player] = slot;
                SelectionCoroutines[Player] = StartCoroutine(ItemSelectionTimer(Player, slot));
            }
            else //选中了原来的道具，也就是使用
            {
                //如果有正在执行的协程，终止它！
                if (SelectionCoroutines.ContainsKey(Player))
                    StopCoroutine(SelectionCoroutines[Player]);
                //这里放使用道具的代码
                Debug.Log($"{Player} 使用了道具槽 {slot} 的道具");
                SelectedSlots[Player] = -1;
                PlayerItems[Player][slot].GetComponent<ItemLogicScript>().useItem();
                Player.GetComponent<TankDataScript>().cSupply -= PlayerItems[Player][slot].GetComponent<ItemLogicScript>().MyData.Cost;
                PlayerItems[Player].RemoveAt(slot);
            }
        }
    }

    private IEnumerator ItemSelectionTimer(GameObject Player,int slot)
    {
        yield return new WaitForSeconds(2f);
        if (SelectedSlots[Player] == slot)
        {
            SelectedSlots[Player] = -1; //取消选中
            Debug.Log("选择超时");
        }
    }

    private void CleanupItemLists(List<GameObject> items)
    {
        foreach (var itm in items)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i] == null || itm == null)
                {
                    items.RemoveAt(i);
                }
            }
        }
    }

    public void DevelopItem(GameObject Player, string tag)
    {
        if (PlayerItems[Player].Count > 2)
            return;
        List<ItemData> StandardItems = new();
        GetComponent<ItemDataScript>().items.ForEach((item) =>
        {
            if (item.Tags.Contains(tag) && item.canDevelop) StandardItems.Add(item);
        });
        int randomIndex = Random.Range(0, StandardItems.Count);
        ItemData _data = StandardItems[randomIndex];
        GameObject newItem = Instantiate(Item);
        ItemLogicScript itemLogic = newItem.GetComponent<ItemLogicScript>();
        ItemAniScript itemAni = newItem.GetComponent<ItemAniScript>();
        itemLogic.InitData(Player, _data);
        PlayerItems[Player].Add(newItem);
        itemAni.DrawCardAmine();
    }

    public void GiveItem(GameObject Player, ItemData _data)
    {
        GameObject newItem = Instantiate(Item);
        ItemLogicScript itemLogic = newItem.GetComponent<ItemLogicScript>();
        ItemAniScript itemAni = newItem.GetComponent<ItemAniScript>();
        itemLogic.InitData(Player, _data);
        Debug.Log(PlayerItems[Player].Count);
        if (PlayerItems[Player].Count < ItemUpperLimit)
        {
            PlayerItems[Player].Add(newItem);
            itemAni.slot = PlayerItems[Player].Count - 1;
            itemAni.DrawCardAmine();
        }
        else
        {
            itemLogic.isSurplus = true;
            itemAni.DrawSurplusCardAnime(ItemUpperLimit);
        }
    }

    public void GiveItem(GameObject Player, int index)
    {
        GiveItem(Player, itemDataScript.items[index]);
    }

    //道具生成
    private ItemData RandomItem()
    {
        // weightSum = spawnWeight.Sum();
        // int x = Random.Range(1, weightSum + 1);
        // int s = 0;
        // for (int i = 0; i < spawnWeight.Count; i++)
        // {
        //     s += spawnWeight[i];
        //     if (x <= s)
        //     {
        //         return i;
        //     }
        // }
        // return 0;
        List<ItemData> WeightedItems = new();
        GetComponent<ItemDataScript>().items.ForEach((item) =>
        {
            for (int i = 1; i <= item.Weight; i++) WeightedItems.Add(item);
        });
        ItemData RandomData = WeightedItems[Random.Range(0, WeightedItems.Count)];
        return RandomData;
    }

    [ContextMenu("Generate Item")]
    private void generateItemOnMap()
    {
        GameObject itemOnMap = Instantiate(ItemOnMap);
        itemOnMap.GetComponent<ItemMapScript>().InitData(RandomItem());
    }

    [ContextMenu("Generate 5 Item")]
    private void generate5ItemOnMap()
    {
        for (int i = 0; i < 5; i++) generateItemOnMap();
    }

    private void HandleGenerateItem()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnTime)
        {
            spawnTimer = 0;
            generateItemOnMap();
        }
    }

}
