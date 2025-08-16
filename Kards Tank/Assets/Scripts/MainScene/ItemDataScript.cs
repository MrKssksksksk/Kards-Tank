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
    // public int ExistenceTime;//存在时间;
    public string Description; //描述
    public Sprite sprite;
    public string Tags;
    public int data;
}

public class ItemDataScript : MonoBehaviour
{
    [SerializeField]
    public List<ItemData> items = new List<ItemData>();
    public ItemData Example1 = new ItemData
    {
        Id = 1,
        Cost = 1,
        Weight = 1,
        Name = "Name",
        Description = "6",
        Tags = "GER TANK",
        data = 0
    };
    public ItemData Example2 = new ItemData
    {
        Id = 1,
        Cost = 1,
        Weight = 1,
        Name = "Name",
        Description = "6",
        Tags = "GER TANK",
        data = 0
    };

    public void Start()
    {
        items.Add(Example1);
        items.Add(Example2);
        Debug.Log(Example1.Id);
    }
}
