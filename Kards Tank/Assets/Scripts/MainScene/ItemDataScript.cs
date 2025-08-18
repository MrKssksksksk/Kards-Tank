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
    public string Description; //描述
    public Sprite sprite;
    public string Tags;
    public int data;
    public bool canDevelop;
}

public class ItemDataScript : MonoBehaviour
{
    [SerializeField]
    public List<ItemData> items = new List<ItemData>();

    public void Start()
    {

    }
}
