using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int Id;
    public string Name;
    public int Cost; //花费
    public int Weight; //权重
    public int ExistenceTime;//存在时间;
    public string Description; //描述
    public string IconPath; //图标路径
    public string Tags;
}

public class ItemDataScript : MonoBehaviour
{
    [SerializeField]
    public List<ItemData> Item = new List<ItemData>();
    public ItemData Example1 = new ItemData
    {
        Id = 1,
        Cost = 1,
        Weight = 1,
        ExistenceTime = 1,
        Name = "Name",
        Description = "6",
        IconPath = "1",
        Tags = "GER TANK"
    };
    public ItemData Example2 = new ItemData
    {
        Id = 1,
        Cost = 1,
        Weight = 1,
        ExistenceTime = 1,
        Name = "Name",
        Description = "6",
        IconPath = "1",
        Tags = "GER TANK"
    };

    public string S = "ss";

    public void Start()
    {
        Item.Add(Example1);
        Item.Add(Example2);
        Debug.Log(Example1.Id);
    }
}
