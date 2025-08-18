using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor.Search;

public class ItemManagerScript : MonoBehaviour
{
    //侧边栏道具管理
    public Dictionary<GameObject,List<GameObject>> PlayerItems = new();
    //道具生成
    public GameObject ItemOnMap;
    public int spawnTime;
    private float spawnTimer;
    private List<int> spawnWeight;
    private int weightSum;

    void Start()
    {
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
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnTime)
        {
            spawnTimer = 0;
            generateItemOnMap();
        }
    }

    private int randomId()
    {
        weightSum = spawnWeight.Sum();
        int x = UnityEngine.Random.Range(1, weightSum + 1);
        int s = 0;
        for (int i = 0; i < spawnWeight.Count; i++)
        {
            s += spawnWeight[i];
            if (x <= s)
            {
                return i;
            }
        }
        return 0;
    }

    [ContextMenu("Generate Item")]
    private void generateItemOnMap()
    {
        GameObject itemOnMap = Instantiate(ItemOnMap);
        itemOnMap.GetComponent<ItemMapScript>().getPointId(randomId());
    }

    [ContextMenu("Generate 5 Item")]
    private void generate5ItemOnMap()
    {
        for (int i = 0; i < 5; i++) generateItemOnMap();
    }

}
