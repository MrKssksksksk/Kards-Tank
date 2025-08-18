using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePointSpawnerScript : MonoBehaviour
{
    //道具生成
    public GameObject resourcePoint;
    private ItemDataScript itemDataScript;
    public int spawnTime;
    private float spawnTimer;
    public List<int> spawnWeight = new List<int>(); // 设定为public，但此列表会被初始化，public的目的是为了调试时随时修改
    private int weightSum;

    void Start()
    {
        itemDataScript = GetComponent<ItemDataScript>();
        spawnWeight = new List<int>();
        
        foreach (ItemData i in itemDataScript.items) // 只读取一次，确保可以游戏内调试
        {
            spawnWeight.Add(i.Weight);
        }
        spawnTimer = 0;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnTime)
        {
            spawnTimer = 0;
            generateResourcePoint();
        }
    }

    private int randomId()
    {
        weightSum = spawnWeight.Sum(); // 每次都重新计算，方便调试
        int x = Random.Range(1, weightSum + 1);
        int s = 0;
        for (int i = 0; i < spawnWeight.Count; i++)
        {
            s += spawnWeight[i];
            if (x <= s)
            {
                return i;
            }
        }
        return -1;
    }

    [ContextMenu("Generate Resource Point")]
    private void generateResourcePoint()
    {
        GameObject r = Instantiate(resourcePoint);
        r.GetComponent<ResourcePointScript>().getPointId(randomId());
    }

    [ContextMenu("Generate 5 Resource Point")]
    private void generate5ResourcePoint()
    {
        for (int i = 0; i < 5; i++) generateResourcePoint();
    }

}
